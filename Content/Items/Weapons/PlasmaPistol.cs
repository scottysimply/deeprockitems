using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Humanizer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class PlasmaPistol : UpgradableWeapon
    {
        public override void NewSetDefaults() {
            Item.damage = 20;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 18f;
            Item.channel = true;
            Item.noMelee = true;
            Item.height = 28;
            Item.width = 30;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 1, 60, 0);
        }
        public override void ResetStats() {
            this.ShotsUntilCooldown = 12f;
            this.TimeToEndCooldown = 75f;
        }
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("PlasmaPistol",
                new UpgradeTier(1,
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.10f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 1)
                    },
                    new Upgrade("IncreasedBattery", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as UpgradableWeapon).ShotsUntilCooldown = 24f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 8)
                                    .AddIngredient(ItemID.FallenStar, 5)
                    },
                    new Upgrade("IncreasedChargeDamage", Assets.Upgrades.AreaOfEffect.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not BigPlasma) return;
                                proj.damage = (int)(proj.damage * 1.25f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.MeteoriteBar, 6)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("SuperSpeedPlasma", Assets.Upgrades.ProjectileVelocity.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not PlasmaBullet) return;
                                proj.velocity *= 1.25f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                        .AddIngredient(ItemID.MeteoriteBar, 6)
                                        .AddIngredient(ItemID.SwiftnessPotion, 3)
                    },
                    new Upgrade("QuickCharge", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not PlasmaPistolHelper helper) return;
                                helper.ChargeTimeMultiplier = 0.75f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 6)
                                    .AddIngredient(ItemID.SwiftnessPotion, 3)
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("ArmorBreak", Assets.Upgrades.ArmorBreak.Value) {
                        Behavior = {
                            Projectile_ModifyHitNPCHook = (Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                                modifiers.ScalingArmorPenetration += 0.25f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 8)
                                    .AddIngredient(ItemID.SharkToothNecklace, 1)
                    },
                    new Upgrade("FireRateIncrease", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = (int)(item.useTime * 0.67f);
                                item.useAnimation = (int)(item.useAnimation * 0.67f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 6)
                                    .AddIngredient(ItemID.FallenStar, 5)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("PlasmaSplash", Assets.Upgrades.AreaOfEffect.Value) {
                        Behavior = {
                            Projectile_OnTileCollideHook = (proj, oldVelocity) => {
                                if (proj.ModProjectile is not PlasmaBullet plasma) return true;
                                if (proj.owner != Main.myPlayer) return false;
                                if (plasma.IsExploding) return true;
                                plasma.Explode();
                                return false;
                            },
                            Projectile_OnHitNPCHook = (proj, npc, hit, damageDone) => {
                                if (proj.ModProjectile is not PlasmaBullet plasma) return;
                                if (proj.owner != Main.myPlayer) return;
                                if (plasma.IsExploding) return;
                                plasma.Explode();
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 8)
                                    .AddIngredient(ItemID.Grenade, 15)
                    },
                    new Upgrade("FlyingNightmare", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not BigPlasma) return;
                                proj.penetrate = -1;
                                proj.usesLocalNPCImmunity = true;
                                proj.localNPCHitCooldown = 10;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 8)
                                    .AddCandidateIngredient([ItemID.Vilethorn, ItemID.CrimsonRod], 1)
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("ThinContainmentField", Assets.Upgrades.SpecialStar.Value) {
                        Behavior = {
                            // When big projectile intersects small projectile, spawn an explosion and kill both
                            Projectile_AIHook = (proj) => {
                                // If this isn't a small projectile, return
                                if (proj.ModProjectile is not PlasmaBullet) return;
                                int intersection = proj.IsCollidingWithProjectile(ModContent.ProjectileType<BigPlasma>());
                                // If no intersection found, return
                                if (intersection == -1) return;
                                // If the owner of the intersecting projectile isn't the owner of the small plasma, return
                                if (proj.owner != Main.myPlayer && Main.projectile[intersection].owner != proj.owner) return;

                                // We are safe to explode this projectile
                                // Explode
                                Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), proj.damage * 3, 0f, proj.owner);
                                // Kill other projectiles
                                Main.projectile[intersection].Kill();
                                proj.Kill();
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 10)
                                    .AddIngredient(ItemID.Dynamite, 10)
                    },
                    new Upgrade("HeatDump", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (Projectile proj, NPC npc, NPC.HitInfo hit, int damageDone) => {
                                int heatAmount = proj.ModProjectile switch {
                                    BigPlasma => 64,
                                    _ => 24,
                                };
                                npc.ChangeTemperature(heatAmount);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 10)
                                    .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                    }
                )
            );
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            type = ModContent.ProjectileType<Projectiles.PlasmaProjectiles.PlasmaPistolHelper>();
        }
        public override void AddRecipes() {
            Recipe.Create(ModContent.ItemType<PlasmaPistol>())
                .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
                .AddIngredient(ItemID.Amethyst, 8)
                .AddIngredient(ItemID.FallenStar, 5)
                .Register();
        }
    }
}