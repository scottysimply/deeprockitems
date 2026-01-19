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
            Item.CloneDefaults(ItemID.Boomstick);
            Item.UseSound = null;
            Item.material = false; // Prevents the weapon being erronously being called a material after upgrading
            Item.damage = 8;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            TimeToEndCooldown = 90f;
            ShotsUntilCooldown = 2f;
            SpreadMultiplier = 1f;
            PelletCount = 3;
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
        public int PelletCount { get; set; }
        public override UpgradeList InitializeUpgrades() {
            int initialType = ProjectileID.Bullet;
            return UpgradeBuilder.CreateUpgradeList("JuryShotgun")
                .Tier()
                    .Upgrade("PelletCount1", Assets.Upgrades.Pellets)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as JuryShotgun).PelletCount += 2;
                        })
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                        .Ingredient(ItemID.MusketBall, 30)
                    .Upgrade("Buckshot1", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 3;
                        })
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                        .Ingredient([ItemID.RottenChunk, ItemID.Vertebrae], 3)
                .Tier()
                    .Upgrade("DoubleTrigger", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.useTime = 8;
                            item.useAnimation = 15;
                        })
                        .Behavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            _shotsFired++;
                            if (_shotsFired >= 2)
                            {
                                _shotsFired = 0;
                                player.itemTime = 30;
                                player.itemAnimation = 30;
                            }
                            return true;
                        })
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                        .Ingredient(ItemID.Vine, 1)
                    .Upgrade("ReloadSpeed", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as JuryShotgun).TimeToEndCooldown -= 45;
                        })
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                        .Ingredient(ItemID.Stinger, 3)
                .Tier()
                    .Upgrade("PelletCount2", Assets.Upgrades.Pellets)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as JuryShotgun).PelletCount += 2;
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient(ItemID.MusketBall, 99)
                    .Upgrade("Buckshot2", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 3;
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient([ItemID.ShadowScale, ItemID.TissueSample], 8)
                    .Upgrade("Blowthrough", Assets.Upgrades.Penetrate)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            projectile.penetrate += 2;
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 30;
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient(ItemID.Diamond, 2)
                .Tier()
                    .Upgrade("ExtendedBarrel", Assets.Upgrades.Focus)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as JuryShotgun).SpreadMultiplier *= 0.25f;
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    .Upgrade("StunChance", Assets.Upgrades.Stun)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 360);
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient([ItemID.SandBlock, ItemID.EbonsandBlock, ItemID.CrimsandBlock, ItemID.PearlsandBlock], 25)
                    .Upgrade("QuadrupleBarrel", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as JuryShotgun).ShotsUntilCooldown += 2f;
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient(ItemID.QuadBarrelShotgun)
                .Tier()
                    .Upgrade("WhitePhosphorusShells", Assets.Upgrades.Heat)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            target.ChangeTemperature(75 / PelletCount, projectile.owner);
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.MeteoriteBar, 4)
                    .Upgrade("Shockwave", Assets.Upgrades.AreaOfEffect)
                        .Behavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            int radius = 10;
                            foreach (var npc in Main.ActiveNPCs)
                            {
                                if (player.Center.DistanceSQ(npc.Center) > (radius * radius * 16 * 16)) continue;

                                var hitinfo = npc.CalculateHitInfo(item.damage * 3, -1);
                                player.StrikeNPCDirect(npc, hitinfo);
                                npc.AddBuff(ModContent.BuffType<StunnedEnemy>(), 30);

                            }

                            return true;
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.Grenade, 15)
            /*                .WithOverclock("TheSlug", Assets.Upgrades.Damage, Overclock.OverclockType.Clean)
                                .WithBehavior<ItemModifyShootStats>((Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                                    spread = 0;
                                    initialType = type;
                                    type = ProjectileID.BlackBolt;
                                })
                                .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Owner: player.whoAmI);
                                    return false;
                                })
                                .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                                    if (projectile.type != ProjectileID.BlackBolt) return;
                                    projectile.timeLeft = 90;
                                    projectile.penetrate += 1;
                                })
                                .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                                    if (projectile.type != ProjectileID.BlackBolt) return;
                                    const int dust_count = 3;
                                    for (int i = 0; i < dust_count; i++)
                                    {
                                        // Gray
                                        Vector2 velocity = Main.rand.NextVector2Circular(.2f, .2f);
                                        Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, SpeedX: 3 * velocity.X + 0.5f * projectile.velocity.X, SpeedY: 3 * velocity.Y + 0.5f * projectile.velocity.Y, newColor: Color.DarkSlateGray, Scale: 1f);
                                    }
                                    for (int i = 0; i < dust_count; i++)
                                    {
                                        // Purple
                                        Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);
                                        Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, SpeedX: 3 * velocity.X + 0.5f * projectile.velocity.X, SpeedY: 3 * velocity.Y + 0.5f * projectile.velocity.Y, Scale: 1f);
                                    }
                                })
                                .WithBehavior<ProjectilePreKill>((Projectile projectile, int timeLeft) => {
                                    if (projectile.type != ProjectileID.BlackBolt) return true;
                                    const int dust_count = 10;
                                    for (int i = 0; i < dust_count; i++)
                                    {
                                        // Gray
                                        Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);
                                        Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, SpeedX: 3*velocity.X, SpeedY: 3*velocity.Y, newColor: Color.DarkSlateGray, Scale: 1f);
                                    }
                                    for (int i = 0; i < dust_count; i++)
                                    {
                                        // Purple
                                        Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);
                                        Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, SpeedX: 3 * velocity.X, SpeedY: 3 * velocity.Y, Scale: 1f);
                                    }
                                    float interval = 2 * MathHelper.Pi / (PelletCount + 9);
                                    for (int i = 0; i < PelletCount + 9; i++)
                                    {
                                        Vector2 adjustedVelocity = Main.rand.NextFloat(VelocityLowerBound, 1f) * projectile.velocity.RotatedBy(i * interval).RotatedByRandom(MathHelper.Pi / 18f);
                                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, adjustedVelocity, initialType, (int)(projectile.damage * 0.75f), projectile.knockBack, Owner: projectile.owner);
                                    }
                                    return false;
                                })
                            .WithOverclock("SpecialPowder", Assets.Upgrades.Powder, Overclock.OverclockType.Clean)
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
                            .WithOverclock("StuffedShells", Assets.Upgrades.Stun, Overclock.OverclockType.Unstable)
                                .WithBehavior<ItemStatChange>((Item item) => {
                                    (item.ModItem as JuryShotgun).PelletCount = (int)((item.ModItem as JuryShotgun).PelletCount * 1.5f);
                                    (item.ModItem as JuryShotgun).TimeToEndCooldown += 40f;
                                })
                                .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                                    projectile.ai[2] = 4f;
                                })
                                .WithBehavior<ProjectileAI>((Projectile projectile) => {
                                    projectile.velocity.Y += 0.1f;
                                })
                                .WithBehavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                                    if (projectile.ai[2] <= 0f)
                                    {
                                        return true;
                                    }
                                    projectile.ai[2]--;
                                    if (oldVelocity.Y != projectile.velocity.Y)
                                    {
                                        projectile.velocity.Y = -0.85f * oldVelocity.Y;
                                    }
                                    if (oldVelocity.X != projectile.velocity.X)
                                    {
                                        projectile.velocity.X = -0.85f * oldVelocity.X;
                                    }
                                    return false;
                                })*/
            .Seal();
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 11;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread)
        {
            SoundEngine.PlaySound(SoundID.Item36 with { PitchVariance = 0.08f }, position);
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
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<JuryShotgun>())
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddRecipeGroup(nameof(ItemID.VilePowder), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}