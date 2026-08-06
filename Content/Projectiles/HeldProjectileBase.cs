using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles
{
    /// <summary>
    /// This class handles the spawning of held projectiles. Override to make a new held projectile.
    /// </summary>
    public abstract class HeldProjectileBase : ModProjectile
    {
        /// <summary>
        /// This is the projectile that the held projectile will spawn on death (when the player stops charging).
        /// </summary>
        public virtual int ProjectileToSpawn { get; set; }
        /// <summary>
        /// This is the time it will take to charge the projectile, in ticks.
        /// </summary>
        public abstract float ChargeTime { get; set; }
        /// <summary>
        /// The sound the projectile will make when the projectile reaches max charge.
        /// </summary>
        public virtual SoundStyle? ChargeSound { get; set; } = null;
        /// <summary>
        /// The sound the projectile will make upon being fired / spawned by the held projectile
        /// </summary>
        public virtual SoundStyle? FireSound { get; set; } = null;
        /// <summary>
        /// Used for manually changing the use-time of the weapon. If this is not set, it defaults to the item's use time.
        /// </summary>
        public virtual int? Cooldown { get; set; } = null;
        public float ChargeTimeMultiplier { get; set; } = 1f;
        /// <summary>
        /// The spread (in radians) that the resultant projectile will have. Defaults to no spread.
        /// </summary>
        public virtual float Spread { get; set; } = 0;
        public float ChargeShotCooldownMultiplier { get; set; } = 1f;
        public float ChargeShotDamageMultiplier { get; set; } = 1f;

        protected Player projectileOwner;
        protected UpgradableWeapon sourceItem;
        protected int ammoUsed = 0;
        /// <summary>
        /// The time that this projectile will live for after reaching
        /// </summary>
        public virtual int ProjectileTime { get; set; } = 15 * 60;
        public bool HasReachedFullCharge { get; set; } = false;
        public virtual void NewSetDefaults() { }
        public override void SetDefaults() {
            Projectile.height = 2;
            Projectile.width = 2;
            Projectile.timeLeft = 2;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            NewSetDefaults();
        }
        public override string Texture => "Terraria/Images/MagicPixel";
        public override void OnSpawn(IEntitySource source) {
            projectileOwner = Main.player[Projectile.owner];

            // Check for upgrades. Due to my new backend, we don't have to check if this upgrade is valid! So we can run this on any item.
            if (source is EntitySource_FromUpgradableWeapon newSource)
            {
                if (Cooldown is null)
                {
                    Cooldown = newSource.Item.Item.useTime;
                }
                sourceItem = newSource.Item;
                if (source is EntitySource_FromUpgradableWeapon { AmmoItemIdUsed: int ammo })
                {
                    ammoUsed = ammo;
                }
            }
            Projectile.timeLeft = ProjectileTime + (int)(ChargeTime * ChargeTimeMultiplier); // Set timeleft to be 15 seconds + time it takes to charge the projectile
            SpecialOnSpawn(source);
        }
        public int TimeSinceSpawning { get; set; } = 0;
        public virtual void SpecialOnSpawn(IEntitySource source) { }

        private int shakeTimer = 0;
        public override void AI() {

            if (Main.LocalPlayer == projectileOwner && projectileOwner.channel)
            {
                HoldItemOut(projectileOwner);
                Projectile.Center = projectileOwner.Center;
                // Projectile has been fully charged
                if (TimeSinceSpawning == (int)(ChargeTime * ChargeTimeMultiplier))
                {
                    HasReachedFullCharge = true;
                    WhenReachedFullCharge();
                    if (ChargeSound is not null)
                    {
                        SoundEngine.PlaySound((SoundStyle)ChargeSound with { PitchVariance = .2f, MaxInstances = 1, Volume = .7f });
                    }
                }
                else if (TimeSinceSpawning > (int)(ChargeTime * ChargeTimeMultiplier))
                {
                    WhileHeldAtCharge();

                    if (shakeTimer % 2 == 0)
                    {
                        projectileOwner.itemLocation = projectileOwner.ShakeWeapon();
                    }
                    if (shakeTimer % 25 == 0)
                    {
                        float dustSpeedX = Main.rand.NextFloat(-.1f, .1f);
                        float dustSpeedY = Main.rand.NextFloat(-.1f, .1f);
                        Dust.NewDust(projectileOwner.position, projectileOwner.width, projectileOwner.height, DustID.Obsidian, dustSpeedX, dustSpeedY);
                    }

                    shakeTimer++;
                }
            }
            else
            {
                Projectile.Kill();
                return;
            }
            // Functions as PostAI, but runs before it.
            SpecialAI();
        }
        public override void PostAI() {
            base.PostAI();
            TimeSinceSpawning++;
            if (TimeSinceSpawning >= ProjectileTime + ChargeTime * ChargeTimeMultiplier)
            {
                Projectile.Kill();
            }
        }
        /// <summary>
        /// This hook allows for custom AI. Ran after AI() is called. Override PreAI() to run code before normal AI is called, or to cancel the AI entirely.
        /// </summary>
        public virtual void SpecialAI() { }
        /// <summary>
        /// This hook is for enabling special functionality when the projectile becomes charged. Only called once, when the projectile is fully charged
        /// </summary>
        public virtual void WhenReachedFullCharge() { }
        /// <summary>
        /// This hook is for enabling special functionality while the projectile is fully charged. Called every frame that the projectile is being channeled and at max charge. By default, the weapon will automatically fire when held at full charge for 5 ticks, if autofire is enabled.
        /// </summary>
        public virtual void WhileHeldAtCharge() {
            // If autofire, shoot after 5 ticks
            if (!Main.player[Projectile.owner].autoReuseAllWeapons) return;

            _heldChargeTimer++;
            if (_heldChargeTimer == 8)
            {
                Projectile.Kill();
                _heldChargeTimer = 0; ;
            }
        }
        private int _heldChargeTimer = 0;
        public virtual void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {

        }
        public virtual bool Shoot(Item item, Player player, EntitySource_FromHeldProjectile source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => true;
        // This is for when the projectile is killed. Spawn the new projectile, play sound, etc.
        public override void OnKill(int timeLeft) {

            if (!SpecialKill(timeLeft)) { return; }
            if (timeLeft == 0 || sourceItem is null)
            {
                return; // Do nothing if the timer expires or we can't get the item for some reason.
            }
            else
            {
                if (FireSound is not null)
                {
                    SoundEngine.PlaySound((SoundStyle)FireSound with { PitchVariance = .1f, MaxInstances = 5, Volume = .4f });
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 position = projectileOwner.Center;
                    Vector2 velocity = Projectile.velocity.Length() * projectileOwner.Center.DirectionTo(Main.MouseWorld);
                    int type = ProjectileToSpawn;
                    int damage = HasReachedFullCharge ? (int)(Projectile.damage * ChargeShotDamageMultiplier) : Projectile.damage;
                    float knockback = Projectile.knockBack;
                    float spread = Spread;
                    ModifyShootStats(sourceItem.Item, projectileOwner, ref position, ref velocity, ref type, ref damage, ref knockback, ref spread);
                    foreach (var upgrade in sourceItem.GetEquippedUpgrades())
                    {
                        upgrade.Behavior.HeldProjectile_ModifyShootStatsHook?.Invoke(this, sourceItem.Item, projectileOwner, ref position, ref velocity, ref type, ref damage, ref knockback, ref spread);
                    }
                    EntitySource_FromHeldProjectile source = new(projectileOwner, sourceItem, ammoUsed, this);
                    bool upgradeReturn = true;
                    foreach (var upgrade in sourceItem.GetEquippedUpgrades())
                    {
                        upgradeReturn &= upgrade.Behavior.HeldProjectile_ShootHook?.Invoke(this, sourceItem.Item, projectileOwner, source, position, velocity, type, damage, knockback, spread) ?? true;
                    }
                    if (upgradeReturn && Shoot(sourceItem.Item, projectileOwner, source, position, velocity, type, damage, knockback, spread))
                    {
                        Vector2 adjusted_speed = velocity.RotatedByRandom(spread);
                        Projectile proj = Projectile.NewProjectileDirect(source, position, adjusted_speed, type, damage, knockback, projectileOwner.whoAmI);
                        // This stops projectiles from coming out sideways
                        proj.rotation = new Vector2(0, 0).DirectionTo(proj.velocity).ToRotation() - MathHelper.Pi / 2;
                        ModifyProjectileAfterSpawning(proj);
                        foreach (var upgrade in sourceItem.GetEquippedUpgrades())
                        {
                            upgrade.Behavior.HeldProjectile_PostSpawnHook?.Invoke(proj, source);
                        }
                    }
                    float multiplier = HasReachedFullCharge ? ChargeShotCooldownMultiplier : 1f;
                    sourceItem.AddCooldownOnShoot(Main.LocalPlayer, multiplier);
                }
            }
        }
        /// <summary>
        /// Modifies the resultant projectile after spawning. Does not modify this proejctile.
        /// </summary>
        /// <param name="projectile"></param>
        public virtual void ModifyProjectileAfterSpawning(Projectile projectile) { }
        /// <summary>
        /// Allows special behavior when the projectile is killed. Return false to override the base class' code.
        /// </summary>
        /// <param name="timeLeft"></param>
        /// <returns></returns>
        public virtual bool SpecialKill(int timeLeft) { return true; }

        public override bool? CanDamage() {
            return false;
        }
        public override bool ShouldUpdatePosition() {
            return false;
        }
        private void HoldItemOut(Player player) {
            // So fun fact about the way the game handles rotation: values go from -Pi to +Pi. There is no 0 to 2Pi.
            // For some god awful reason though, when the mouse is in Quadrant II, itemRotation doesn't match DirectionTo().ToRotation() of the mouse.

            player.itemTime = player.itemAnimation = Cooldown is not null ? (int)Cooldown : 0;

            if (Main.MouseWorld.X > player.Center.X)
            {
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
                player.ChangeDir(1);
                return;
            }
            if (Main.MouseWorld.Y < player.Center.Y)
            {
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.Pi;
            }
            else
            {
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.Pi;
            }
            player.ChangeDir(-1);

        }
    }
}