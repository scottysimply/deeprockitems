using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.Globals;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Upgrades;
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
        }
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("M1000")
                .Tier()
                    .Upgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.15f);
                        })
                        .Ingredient(ItemID.HellstoneBar, 8)
                        .Ingredient([ItemID.RagePotion, ItemID.WrathPotion], 1)
                    .Upgrade("BiggerClip", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as M1000).ShotsUntilCooldown *= 1.5f;
                        })
                        .Ingredient(ItemID.HellstoneBar, 8)
                        .Ingredient([ItemID.AmmoReservationPotion], 1)
                    .Upgrade("FocusDamage", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .Ingredient(ItemID.HellstoneBar, 8)
                        .Ingredient(ItemID.SoulofNight, 3)
                .Tier()
                    .Upgrade("QuickCharge", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;

                            helper.ChargeTime *= 0.5f;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient([ItemID.SwiftnessPotion], 6)
                    .Upgrade("FocusDamage", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient([ItemID.SoulofNight], 6)

                    .Upgrade("BumpFire", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.useTime = (int)(_oldUseTime * 0.67f);
                            item.useAnimation = (int)(_oldUseAnimation * 0.67f);
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient(ItemID.SwiftnessPotion, 3)
                .Tier()
                    .Upgrade("ArmorPiercing", Assets.Upgrades.ArmorBreak)
                        .Behavior<ProjectileModifyHitNPC>((Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                            modifiers.ScalingArmorPenetration += 0.25f;
                        })
                        .Ingredient(ItemID.HallowedBar, 8)
                        .Ingredient(ItemID.SharkToothNecklace, 1)
                    .Upgrade("UmaniteBullets", Assets.Upgrades.Powder)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            target.AddInstancedBuff<Irradiated>(60, out _);
                        })
                .Tier()
                    .Upgrade("HighDamageUpgrade", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.damage * 1.75f);
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 8)
                        .Ingredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    .Upgrade("EfficientCharge", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;

                            helper.ChargeShotCooldownMultiplier = 1;
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 8)
                        .Ingredient(ItemID.MusketBall, 99)

                .Tier()
                    .Upgrade("WhereItHurts", Assets.Upgrades.SpecialStar)
                        .Behavior<ProjectileModifyHitNPC>((Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                            if (projectile.GetSource() is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            modifiers.ModifyHitInfo += (ref NPC.HitInfo info) => {
                                if (target.immortal || !target.active) return;
                                if (info.Damage >= target.lifeMax * 0.05f) return;

                                info.Damage = (int)(target.lifeMax * 0.05f);
                            };
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 8)
                        .Ingredient(ItemID.FragmentVortex, 6)
                    // This upgrade functions like magic bullets for the bulldog in drg: focused bullets rebound automatically to targets
                    .Upgrade("MagicBullets", Assets.Upgrades.Penetrate)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (source.SourceProjectile.HasReachedFullCharge)
                            {
                                projectile.penetrate += 2;
                            }
                            projectile.penetrate += 2;

                        })
                        .Behavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
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
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            var query = Main.npc.Where(n => n.active && !n.friendly && !n.immortal && n.immune[projectile.owner] < 10).OrderBy(n => n.Center.DistanceSQ(projectile.Center));
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
                        .Behavior<ProjectilePreDraw>((Projectile projectile, Color lightColor) => {
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
                        .Ingredient(ItemID.ChlorophyteBar, 8)
                        .Ingredient(ItemID.Nanites, 6)
                .Overclock("TheWidowmaker", Assets.Upgrades.Haste, Overclock.OverclockType.Clean)
                    .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
                        if (target.immortal) return;
                        if (target.life <= 0)
                        {
                            OverheatCooldown -= 4 * (100f / ShotsUntilCooldown);
                            if (OverheatCooldown <= 0)
                            {
                                OverheatCooldown = 0f;
                            }
                        }
                        else
                        {
                            OverheatCooldown -= 100f / ShotsUntilCooldown;
                            if (OverheatCooldown <= 0)
                            {
                                OverheatCooldown = 0f;
                            }
                        }
                    })
                .Overclock("Hipster", Assets.Upgrades.Focus, Overclock.OverclockType.Balanced)
                    .Behavior<HeldProjectileModifyShootStats>((HeldProjectileBase projectile, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack, ref float spread) => {
                        spread *= 0.2f;
                        projectile.ChargeShotDamageMultiplier = 1.5f;
                        if (!projectile.HasReachedFullCharge)
                        {
                            damage = (int)(damage * 1.25f);
                        }
                    })
                .Overclock("SupercoolingChamber", Assets.Upgrades.Damage, Overclock.OverclockType.Unstable)
                    .Behavior<ItemStatChange>((Item item) => {
                        (item.ModItem as M1000).TimeToEndCooldown *= 1.5f;
                    })
                    .Behavior<HeldProjectileModifyShootStats>((HeldProjectileBase helper, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                        helper.ChargeShotDamageMultiplier *= 3;
                        if (!helper.HasReachedFullCharge)
                        {
                            damage = (int)(damage * 0.75f);
                        }
                    })
            .Seal();                        
        }
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