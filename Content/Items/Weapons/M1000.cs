using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : UpgradableWeapon
    {
        private int original_projectile;
        public override void NewSetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 60;
            Item.height = 12;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 9, 25, 0);
            Item.consumable = false;
            Item.autoReuse = true;
            this.ShotsUntilCooldown = 24f;
            this.TimeToEndCooldown = 80f;
        }
        public float AmmoChance { get; set; } = 1f;
        public override bool CanConsumeAmmo(Item ammo, Player player) {
            return Main.rand.NextBool((int)(100 * AmmoChance), 100);
        }
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("M1000",
                new UpgradeTier(1,
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.25f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 6)
                                    .AddIngredient(ItemID.SoulofNight, 4)
                    },
                    new Upgrade("BiggerClip", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as M1000).ShotsUntilCooldown *= 1.5f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 6)
                                    .AddIngredient(ItemID.SoulofLight, 4)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("QuickCharge", Assets.Upgrades.Focus.Value) {
                        Behavior = { 
                            Projectile_OnSpawnHook = (projectile, source) => {
                            if (projectile.ModProjectile is not HeldProjectileBase modProj) return;

                            modProj.ChargeTime *= 0.5f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 6)
                                    .AddIngredient(ItemID.SwiftnessPotion, 3)
                    },
                    new Upgrade("EfficientCharge", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (projectile, source) => {
                                if (projectile.ModProjectile is not HeldProjectileBase modProj) return;

                                modProj.ChargeShotCooldownMultiplier = 1f;
                            } 
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 6)
                                    .AddIngredient(ItemID.MusketBall, 99)
                    },
                    new Upgrade("BumpFire", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = item.useAnimation = (int)(item.useTime * 0.66f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 6)
                                    .AddIngredient(ItemID.IllegalGunParts, 1)
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("FocusDamage", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (projectile, source) => {
                                if (source is not EntitySource_FromHeldProjectile newSource) return;

                                if (newSource.SourceProjectile.Projectile.timeLeft >= newSource.SourceProjectile.ProjectileTime) return;

                                projectile.damage = (int)(projectile.damage * 1.85f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 6)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => { 
                                item.damage = (int)(item.damage * 1.25f); 
                            } 
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 6)
                                    .AddIngredient(ItemID.SoulofNight, 4)
                    },
                    new Upgrade("ArmorPiercing", Assets.Upgrades.ArmorBreak.Value) {
                        Behavior = {
                            Projectile_ModifyHitNPCHook = (Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                                modifiers.ScalingArmorPenetration += 0.25f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 6)
                                    .AddIngredient(ItemID.SharkToothNecklace, 1)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("DiggingRounds", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (projectile, source) => {
                                projectile.tileCollide = false;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 6)
                                    .AddIngredient(ItemID.SoulofMight, 4)
                    },
                    new Upgrade("QuickReload", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as M1000).TimeToEndCooldown *= 0.5f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 6)
                                    .AddIngredient(ItemID.SoulofSight, 4)
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("Blowthrough", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (projectile, source) => {
                                projectile.penetrate = projectile.maxPenetrate = 5;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddIngredient(ItemID.SoulofFright, 4)
                    },
                    new Upgrade("HollowPointRounds", Assets.Upgrades.Stun.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (projectile, target, hitInfo, damageDone) => {
                                target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 60);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.ChlorophyteBar, 8)
                                    .AddCandidateIngredient([ItemID.CopperBar, ItemID.TinBar], 4)
                    }
                )
            );
        }
        public override void ResetStats()
        {
            Item.damage = Item.OriginalDamage;
            AmmoChance = 1f;
            TimeToEndCooldown = 75f;
            ShotsUntilCooldown = 12f;
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread)
        {
            // Store the projectile that would've been shot.
            original_projectile = type;

            // Set type to be the "helper" projectile.
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            if (proj.ModProjectile is HeldProjectileBase modProj)
            {
                // Make the helper spawn the original projectile when it despawns/dies, to make it look like the original projectile was shot.
                modProj.ProjectileToSpawn = original_projectile;
                // Replace musket balls with high-velocity bullet
                if (original_projectile == ProjectileID.Bullet)
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }

                // Sorry, until this weird gravity issue gets fixed: No modded bullets!
                if (!ModInformation.IsProjectileVanilla(original_projectile) && !ModInformation.IsProjectileMyMod(original_projectile))
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<M1000>())
            .AddIngredient(ItemID.Musket, 1)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.HellstoneBar, 12)
            .Register();

            Recipe.Create(ModContent.ItemType<M1000>())
            .AddIngredient(ItemID.TheUndertaker, 1)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.HellstoneBar, 12)
            .Register();
        }
    }
}