using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
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
            this.ShotsUntilCooldown = 16f;
            this.TimeToEndCooldown = 80f;
            _smartBulletPenetrate = 1;
        }
        int _smartBulletPenetrate = 1;
        public override UpgradeList InitializeUpgrades() {
            bool smartBullet = false;
            return UpgradeBuilder.CreateUpgradeList("M1000")
                .WithTier()
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.15f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 1)
                    .WithUpgrade("BiggerClip", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as M1000).ShotsUntilCooldown *= 1.5f;
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient([ItemID.AmmoReservationPotion], 1)
                    .WithUpgrade("FocusDamage", Assets.Upgrades.Damage)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient(ItemID.SoulofNight, 3)
                .WithTier()
                    .WithUpgrade("QuickCharge", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;

                            helper.ChargeTime *= 0.5f;
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.SwiftnessPotion], 6)
                    .WithUpgrade("FocusDamage", Assets.Upgrades.Damage)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.SoulofNight], 6)

                    .WithUpgrade("BumpFire", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.useTime = (int)(_oldUseTime * 0.67f);
                            item.useAnimation = (int)(_oldUseAnimation * 0.67f);
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient(ItemID.SwiftnessPotion, 3)
                .WithTier()
                    .WithUpgrade("ArmorPiercing", Assets.Upgrades.ArmorBreak)
                        .WithBehavior<ProjectileModifyHitNPC>((Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                            modifiers.ScalingArmorPenetration += 0.25f;
                        })
                        .WithIngredient(ItemID.HallowedBar, 8)
                        .WithIngredient(ItemID.SharkToothNecklace, 1)
                    .WithUpgrade("UmaniteBullets", Assets.Upgrades.Powder)
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            target.AddInstancedBuff<Irradiated>(60, out _);
                        })
                .WithTier()
                    .WithUpgrade("HighDamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.damage * 1.75f);
                        })
                        .WithIngredient(ItemID.ChlorophyteBar, 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    .WithUpgrade("EfficientCharge", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;

                            helper.ChargeShotCooldownMultiplier = 1;
                        })
                        .WithIngredient(ItemID.ChlorophyteBar, 8)
                        .WithIngredient(ItemID.MusketBall, 99)

                .WithTier()
                    .WithUpgrade("Something", Assets.Upgrades.Penetrate)

                    // This upgrade functions like magic bullets for the bulldog in drg: focused bullets rebound automatically to targets
                    .WithUpgrade("MagicBullets", Assets.Upgrades.SpecialStar)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true})
                            {
                                var newSource = (source as EntitySource_FromHeldProjectile);
                                smartBullet = true;
                                if (newSource.SourceProjectile.ProjectileToSpawn == ProjectileID.Bullet)
                                {
                                    projectile.maxPenetrate = projectile.penetrate = 3;
                                }
                            }
                            else
                            {
                                smartBullet = false;
                            }
                        })
                        .WithBehavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                            if (!smartBullet) return true;
                            var query = Main.npc.Where(n => n.active && !n.friendly && !n.immortal).OrderBy(n => n.Center.DistanceSQ(projectile.Center));
                            foreach (var npc in query)
                            {
                                if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                                {
                                    float angle = projectile.AngleTo(npc.Center);
                                    projectile.velocity = oldVelocity.Length() * projectile.Center.DirectionTo(npc.Center);
                                    projectile.penetrate--;
                                    return false;
                                }
                            }
                            return true;
                        })
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            if (!smartBullet) return;
                            var query = Main.npc.Where(n => n.active && !n.friendly && !n.immortal).OrderBy(n => n.Center.DistanceSQ(projectile.Center));
                            foreach (var npc in query)
                            {
                                if (target.whoAmI == npc.whoAmI) continue;
                                if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                                {
                                    float angle = projectile.AngleTo(npc.Center);
                                    projectile.velocity = projectile.velocity.Length() * projectile.Center.DirectionTo(npc.Center);
                                    return;
                                }
                            }
                        })
                        .WithBehavior<ProjectilePreDraw>((Projectile projectile, Color lightColor) => {
                            if (!smartBullet) return true;
                            int textureWidth = TextureAssets.Projectile[projectile.type].Value.Width;
                            var query = Main.npc.Where(n => n.active && projectile.Center.DistanceSQ(n.Center) <= textureWidth * textureWidth);
                            foreach (var npc in query)
                            {
                                if (projectile.Center.DistanceSQ(npc.Center) <= MathF.Pow((textureWidth / 2f), 2))
                                {
                                    return false;
                                }
                            }
                            return true;
                        })
                        .WithIngredient(ItemID.ChlorophyteBar, 8)
                        .WithIngredient(ItemID.Nanites, 6)
            .Seal();                        
        }
        /*        public override UpgradeList InitializeUpgrades() {
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
        */
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread)
        {
            // original_projectile will get passed into the helper in NewShoot()
            original_projectile = type;
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            if (proj.ModProjectile is HeldProjectileBase modProj)
            {
                modProj.ProjectileToSpawn = original_projectile;
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