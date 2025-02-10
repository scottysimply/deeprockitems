using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using deeprockitems.Content.Upgrades;
using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;
using Terraria.Audio;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : UpgradableWeapon
    {
        public override void NewSetDefaults()
        {
            ResetStats();
            Item.CloneDefaults(ItemID.Boomstick);
            Item.UseSound = null;
            Item.material = false; // Prevents the weapon being erronously being called a material after upgrading
            Item.damage = 15;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            TimeToEndCooldown = 120f;
        }
        private int _shotsFired = 0;
        /// <summary>
        /// The multiplier given to the number of projectiles this shotgun shoots.
        /// </summary>
        public float ProjectileMultiplier { get; set; } = 1f;
        /// <summary>
        /// The multiplier given to the spread of this shotgun.
        /// </summary>
        public float SpreadMultiplier { get; set; } = 1f;
        /// <summary>
        /// The lower bound of the shotgun velocity
        /// </summary>
        public float VelocityLowerBound { get; set; } = 0.8f;
        public int PelletCount { get; set; } = 3;
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("JuryShotgun")
                .WithTier()
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.2f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient(ItemID.Bone, 5)
                    .WithUpgrade("Sniper", Assets.Upgrades.Focus)
                        .WithBehavior<ItemModifyShootStats>((Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                            spread *= 0.66f;
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .WithTier()
                    .WithUpgrade("QuickFire", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.useTime = 8;
                            item.useAnimation = 15;
                        })
                        .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            _shotsFired++;
                            if (_shotsFired >= 2)
                            {
                                _shotsFired = 0;
                                player.itemTime = 30;
                                player.itemAnimation = 30;
                            }
                            return true;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.Feather, 4)
                    .WithUpgrade("ReloadSpeed", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as UpgradableWeapon).TimeToEndCooldown *= 0.5f;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.Deathweed, 5)
                .WithTier()
                    .WithUpgrade("Birdshot", Assets.Upgrades.Focus)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 20;
                            (item.ModItem as JuryShotgun).PelletCount += 5;
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.SoulofLight, 4)
                    .WithUpgrade("Buckshot", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 40;

                            (item.ModItem as JuryShotgun).PelletCount += 2;
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.SoulofNight, 4)
                .WithTier()
                    .WithUpgrade("WhitePhosphorusShells", Assets.Upgrades.Heat)
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            target.ChangeTemperature(125 / PelletCount, projectile.owner);
                        })
                        .WithIngredient(ItemID.HallowedBar, 8)
                        .WithIngredient(ItemID.HellstoneBar, 6)
                    .WithUpgrade("Shockwave", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            int radius = 10;
                            foreach (var npc in Main.ActiveNPCs)
                            {
                                if (player.Center.DistanceSQ(npc.Center) > (radius*radius*16*16)) continue;

                                var hitinfo = npc.CalculateHitInfo(item.damage * 3, -1);
                                player.StrikeNPCDirect(npc, hitinfo);
                                npc.AddBuff(ModContent.BuffType<StunnedEnemy>(), 60);

                            }

                            return true;
                        })
                        .WithIngredient(ItemID.HallowedBar, 8)
                        .WithIngredient(ItemID.Bomb, 6)
                    .WithUpgrade("Blowthrough", Assets.Upgrades.Penetrate)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            projectile.penetrate += 2;
                        })
                        .WithIngredient(ItemID.HallowedBar, 8)
                        .WithIngredient(ItemID.HighVelocityBullet, 99)
                .WithTier()
                    .WithUpgrade("QuadrupleBarrel", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as UpgradableWeapon).ShotsUntilCooldown *= 2;
                        })
                        .WithIngredient(ItemID.ChlorophyteBar, 8)
                        .WithIngredient(ItemID.QuadBarrelShotgun)
                    .WithUpgrade("HeavyDamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 40;
                        })
                        .WithIngredient(ItemID.ChlorophyteBar, 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                .WithOverclocks()
                    .WithUpgrade("SpecialPowder", Assets.Upgrades.Powder)
                        .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            Vector2 mousePos = Main.MouseWorld - player.Center;
                            player.velocity -= Vector2.Normalize(mousePos) * 10;
                            // Cap x speed but not y
                            if (Math.Abs(player.velocity.X) > 15f)
                            {
                                player.velocity.X = 15f * Math.Sign(player.velocity.X);
                            }
                            // Cancel fall damage
                            if (player.velocity.Y < 5f)
                            {
                                player.fallStart = (int)player.position.Y / 16;
                            }
                            return true;
                        })
            .Seal();
        }
        public override void ResetStats() {
            PelletCount = 3;
            Item.damage = Item.OriginalDamage;
            TimeToEndCooldown = 75f;
            ShotsUntilCooldown = 2f;
            SpreadMultiplier = 1f;
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 13;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread)
        {
            SoundEngine.PlaySound(SoundID.Item30 with { PitchVariance = 0.15f }, position);
            // Change player's direction to face the cursor
            if (Main.MouseWorld.X > player.Center.X)
            {
                player.direction = 1;
            }
            else
            {
                player.direction = -1;
            }

            // Shoot logic
            int numberProjectiles = PelletCount + Main.rand.Next(0, 1);

            // This block is for the projectile spread.
            int projectilesWithMultiplier = (int)Math.Floor(ProjectileMultiplier * numberProjectiles);
            for (int i = 0; i < projectilesWithMultiplier; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(spread * SpreadMultiplier) * Main.rand.NextFloat(VelocityLowerBound, 1.2f); // random velocity effect
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, numberProjectiles);
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<JuryShotgun>())
                .AddIngredient(ItemID.Boomstick, 1)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient(ItemID.Hellstone, 12)
                .AddRecipeGroup(nameof(ItemID.VilePowder), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}