using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.Globals;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Mono.Cecil;
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
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 60;
            Item.height = 12;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 8;
            Item.useAnimation = 8;
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
                    .Upgrade("HighCaliberRounds", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = item.OriginalDamage + 12;
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.SoulofFright, 4)
                    .Upgrade("LaserSights", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.Ruby, 4)
                    .Upgrade("ExtendedGrip", Assets.Upgrades.Damage)
                        .Behavior<HeldProjectileModifyShootStats>((HeldProjectileBase projectile, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                            spread *= 0.5f;
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.SoulofSight, 4)
                .Tier()
                    .Upgrade("CompactClips", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as M1000).ShotsUntilCooldown += 8f;
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    .Upgrade("QuickEject", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as M1000).TimeToEndCooldown *= 0.66f;
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.SoulofFlight, 4)

                    .Upgrade("FastChargingCoils", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;
                            helper.ChargeTimeMultiplier = 0.8f;
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                .Tier()
                    .Upgrade("HighCaliberRounds", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = item.OriginalDamage + 16;
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 4)
                        .Ingredient(ItemID.Ruby, 4)
                    .Upgrade("LaserSights", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (source is not EntitySource_FromHeldProjectile { SourceProjectile.HasReachedFullCharge: true }) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                .Tier()
                    .Upgrade("HardenedRounds", Assets.Upgrades.ArmorBreak)
                        .Behavior<ProjectileModifyHitNPC>((Projectile projectile, NPC npc, ref NPC.HitModifiers modifiers) => {
                            modifiers.ScalingArmorPenetration += 0.25f;
                        })
                        .Ingredient(ItemID.BeetleHusk, 8)
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                    .Upgrade("HittingWhereItHurts", Assets.Upgrades.Stun)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC npc, NPC.HitInfo hit, int damage) => {
                            if (Main.rand.Next() % 4 == 0)
                            {
                                npc.AddBuff(ModContent.BuffType<StunnedEnemy>(), 90);
                            }
                        })
                        .Ingredient(ItemID.BeetleHusk, 8)
                        .Ingredient(ItemID.ExplosivePowder, 30)
                    .Upgrade("FastChargingCoils", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not M1000Helper helper) return;
                            helper.ChargeTimeMultiplier = 0.8f;
                        })
                        .Ingredient(ItemID.BeetleHusk, 8)
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                .Tier()
                    .Upgrade("HighCaliberRounds", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = item.OriginalDamage + 16;
                        })
                        .Ingredient(ItemID.FragmentVortex, 4)
                        .Ingredient(ItemID.SoulofFright, 4)
                    .Upgrade("CompactClips", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as M1000).ShotsUntilCooldown += 12f;
                        })
                        .Ingredient(ItemID.FragmentVortex, 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    .Upgrade("BlowthroughRounds", Assets.Upgrades.Penetrate)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            projectile.penetrate += 2;
                            projectile.maxPenetrate += 2;
                        })
                        .Ingredient(ItemID.FragmentVortex, 4)
                        .Ingredient(ItemID.Diamond, 4)
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
            .AddIngredient(ItemID.HallowedBar, 8)
            .AddIngredient(ItemID.SoulofFright, 6)
            .Register();

            Recipe.Create(ModContent.ItemType<M1000>())
            .AddIngredient(ItemID.TheUndertaker, 1)
            .AddIngredient(ItemID.HallowedBar, 8)
            .AddIngredient(ItemID.SoulofFright, 6)
            .Register();
        }
    }
}