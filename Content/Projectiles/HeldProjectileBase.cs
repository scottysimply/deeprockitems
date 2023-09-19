using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static System.Math;
using System.Reflection.Metadata;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using static Humanizer.In;
using Terraria.ID;
using Microsoft.CodeAnalysis;
using deeprockitems.Content.Items.Upgrades.M1000;
using deeprockitems.Content.Items.Upgrades.PlasmaPistol;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Projectiles;
/// <summary>
/// This class handles the spawning of held projectiles. Override to make a new held projectile.
/// </summary>
public abstract class HeldProjectileBase : ModProjectile
{
    /// <summary>
    /// This is the projectile that the held projectile will spawn on death (when the player stops charging)
    /// </summary>
    public int ProjectileToSpawn { get; set; }
    /// <summary>
    /// This is the time it will take to charge the projectile, in ticks.
    /// </summary>
    public virtual float CHARGE_TIME { get; set; } // This is mostly constant per each projectile type, so the screaming snake case stays.
    /// <summary>
    /// The sound the projectile will make when the projectile reaches max charge.
    /// </summary>
    public virtual SoundStyle Charge_Sound { get; set; }
    /// <summary>
    /// The sound the projectile will make upon being fired / spawned by the held projectile
    /// </summary>
    public virtual SoundStyle Fire_Sound { get; set; }
    /// <summary>
    /// Used for manually changing the use-time of the weapon. If this is not set, it defaults to the item's use time.
    /// </summary>
    public int Cooldown { get; set; } = -1;
    /// <summary>
    /// The spread (in radians) that the resultant projectile will have. Defaults to no spread.
    /// </summary>
    public double Spread { get; set; } = 0;

    protected Player projectileOwner;
    protected Item sourceItem;
    protected int ammoUsed = 0;
    protected const int BUFFER_TIME = 900; // Time in ticks that the weapon should stay charged for. Projectile should despawn after this time.
    public override void SetDefaults()
    {
        Projectile.height = 2;
        Projectile.width = 2;
        Projectile.timeLeft = 2;
        Projectile.hide = true;
        Projectile.tileCollide = false;
    }
    public override string Texture => "Terraria/Images/MagicPixel";
    public override void OnSpawn(IEntitySource source)
    {
        projectileOwner = Main.player[Projectile.owner];

        // Check for upgrades. Due to my new backend, we don't have to check if this upgrade is valid! So we can run this on any item.
        if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate parent_weapon} )
        {
            if (Cooldown == -1)
            {
                Cooldown = parent_weapon.Item.useTime;
            }
            sourceItem = parent_weapon.Item;
            if (parent_weapon.Upgrades.Contains(ModContent.ItemType<QuickCharge>()))
            {
                CHARGE_TIME *= .75f;
            }
            if (parent_weapon.Upgrades.Contains(ModContent.ItemType<SupercoolOC>()))
            {
                CHARGE_TIME *= 1.33f;
            }
            if (source is EntitySource_ItemUse_WithAmmo { AmmoItemIdUsed: int ammo})
            {
                ammoUsed = ammo;
            }
            Projectile.ai[2] = parent_weapon.Overclock;
        }
        Projectile.timeLeft = BUFFER_TIME + (int)CHARGE_TIME; // Set timeleft to be 15 seconds + time it takes to charge the projectile
        SpecialOnSpawn(source);
    }
    public virtual void SpecialOnSpawn(IEntitySource source) { }

    private int shakeTimer = 0;
    public override void AI()
    {

        if (Main.LocalPlayer == projectileOwner && projectileOwner.channel)
        {
            HoldItemOut(projectileOwner);
            Projectile.Center = projectileOwner.Center;
            if (Projectile.timeLeft == BUFFER_TIME) // Projectile has been charged, I repeat, projectile has been charged
            {
                AtFullCharge();
                SoundEngine.PlaySound(Charge_Sound with { PitchVariance = .2f, MaxInstances = 1, Volume = .7f });
            }
            else if (Projectile.timeLeft < BUFFER_TIME)
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
                    Terraria.Dust dust = Terraria.Dust.NewDustDirect(projectileOwner.position, projectileOwner.width, projectileOwner.height, DustID.Obsidian, dustSpeedX, dustSpeedY);
                    //dust.noGravity = true;
                }

                shakeTimer++;
            }
        }
        else
        {
            Projectile.Kill();
        }
        // SpecialAI() is post-ai.
        SpecialAI();
    }
    /// <summary>
    /// This hook allows for custom AI. Ran after AI() is called. Override PreAI() to run code before normal AI is called, or to cancel the AI entirely.
    /// </summary>
    public virtual void SpecialAI() { }
    /// <summary>
    /// This hook is for enabling special functionality when the projectile becomes charged. Only called once, when the projectile is fully charged
    /// </summary>
    public virtual void AtFullCharge() { }
    /// <summary>
    /// This hook is for enabling special functionality while the projectile is fully charged. Called every frame that the projectile is being channeled and at max charge.
    /// </summary>
    public virtual void WhileHeldAtCharge() { }

    // This is for when the projectile is killed. Spawn the new projectile, play sound, etc.
    public override void Kill(int timeLeft)
    {

        if (!SpecialKill(timeLeft)) { return; }
        if (timeLeft == 0 || sourceItem is null)
        {
            return; // Do nothing if the timer expires or we can't get the item for some reason.
        }
        else
        {
            if (Main.myPlayer == Projectile.owner)
            {
                // Play the sound the projectile makes when the bullet spawns
                SoundEngine.PlaySound(Fire_Sound with { PitchVariance = .1f, MaxInstances = 5, Volume = .4f });

                float shoot_speed = Projectile.velocity.Distance(new(0, 0)); // This is the magnitude of the velocity
                Projectile.velocity = shoot_speed * projectileOwner.Center.DirectionTo(Main.MouseWorld); // A vector is just magnitude and direction

                Vector2 adjusted_speed = Projectile.velocity.RotatedByRandom(Spread);

                Projectile proj = Projectile.NewProjectileDirect(projectileOwner.GetSource_ItemUse_WithPotentialAmmo(sourceItem, ammoUsed), projectileOwner.Center, adjusted_speed, ProjectileToSpawn, Projectile.damage, Projectile.knockBack, projectileOwner.whoAmI, ai0: timeLeft, ai2: Projectile.ai[2]);

                // Make sure the projectile goes the right direction after charging

                proj.rotation = new Vector2(0, 0).DirectionTo(proj.velocity).ToRotation() - (float)PI / 2; // No sideways projectiles!

            }
        }
    }
    /// <summary>
    /// Allows special behavior when the projectile is killed. Return false to override the base class' code.
    /// </summary>
    /// <param name="timeLeft"></param>
    /// <returns></returns>
    public virtual bool SpecialKill(int timeLeft) { return true; }

    public override bool? CanDamage()
    {
        return false; // This weapon is invisible and intangible. We don't want the player to know it exists.
    }
    public override bool ShouldUpdatePosition()
    {
        return false; // We want this projectile to stay on the player, which will be done manually.
    }



    // So what's going on in this method? Deconstruction time!
    private void HoldItemOut(Player player)
    {
        // So fun fact about the way the game handles rotation: values go from -Pi to +Pi. There is no 0 to 2Pi.
        // For some god awful reason though, when the mouse is in Quadrant IV, itemRotation doesn't match DirectionTo().ToRotation() of the mouse.

        // Make sure the player appears to actually hold the projectile.
        player.itemTime = player.itemAnimation = Cooldown;

        // If cursor is to the right of the player
        if (Main.MouseWorld.X > player.Center.X)
        {
            // See, this is easy!
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            player.direction = 1; // Make the player face right
            return;
        }
        // If cursor is above the player
        if (Main.MouseWorld.Y < player.Center.Y)
        {
            // Here's where it messes up. If the cursor is in quadrant II, it needs to add PI
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
        }
        // If cursor is below the player
        else
        {
            // But if the cursor is in Quadrant III, it has to subtract. guh??
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - (float)PI;
        }
        player.direction = -1; // Make the player face left

    }
}