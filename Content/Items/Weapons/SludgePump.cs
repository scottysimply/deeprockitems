using deeprockitems.Common.EntitySources;
using deeprockitems.Common.PlayerLayers;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class SludgePump : UpgradableWeapon
    {
        public override void NewSetDefaults() {
            Item.damage = 52;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.width = 70;
            Item.height = 36;
            Item.mana = 10;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<SludgeHelper>();
            Item.shootSpeed = 22f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;

            TimeToEndCooldown = 80f;
            ShotsUntilCooldown = 16f;

            Item.value = Item.sellPrice(0, 5, 30, 0);

        }
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("SludgePump")
                .Tier(1)
                    .Upgrade("VisualCalculus", Assets.Upgrades.Focus)
                        .Behavior<ItemHoldItem>((item, player) => {
                            player.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw = true;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient(ItemID.MechanicalLens, 1)
                    .Upgrade("Glowstick", Assets.Upgrades.Heat)
                        .Behavior<ProjectilePreDraw>((projectile, lightColor) => {
                            if (projectile.ModProjectile is not SludgeBall) return true;

                            Main.EntitySpriteDraw(new DrawData(TextureAssets.Projectile[projectile.type].Value, projectile.getRect(), Color.White));
                            return true;
                        })
                        .Behavior<ProjectileAI>((projectile) => {
                            if (projectile.ModProjectile is not SludgeBall) return;

                            Lighting.AddLight(projectile.position, new Vector3(0.05f, 0.9f, 0.05f));
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient(ItemID.Glowstick, 10)
                .Tier(2)
                    .Upgrade("HardenedGooMix", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper && !helper.HasReachedFullCharge)
                            {
                                proj.damage += 10;
                            }
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient(ItemID.SlimeBlock, 30)
                    .Upgrade("DyseNozzle", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper && helper.HasReachedFullCharge)
                            {
                                proj.damage += 20;
                            }
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient(ItemID.SoulofNight, 4)
                .Tier(3)
                    .Upgrade("OvertunedNozzle", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is SludgeHelper helper)
                            {
                                helper.ChargeTimeMultiplier = 0.75f;
                            }
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                        .Ingredient(ItemID.SoulofFlight, 4)
                    .Upgrade("Exorcist", Assets.Upgrades.Damage)
                        .Behavior<ItemModifyManaCost>((Player player, ref float reduce, ref float mult) => {
                            mult -= 0.25f;
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                        .Ingredient(ItemID.FallenStar, 10)
                    .Upgrade("PerforatedNozzle", Assets.Upgrades.Focus)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is SludgeBall ball)
                            {
                                ball.NumProjectilesToSpawn += 4;
                            }
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .Tier(4)
                    .Upgrade("BiochemicalWarfare", Assets.Upgrades.SpecialStar)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC npc, NPC.HitInfo hit, int damageDone) => {
                            if (npc.HasInstancedBuff(out Sludged buff))
                            {
                                buff.AmContagious = true;
                            }
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.SoulofSight, 4)
                    .Upgrade("HydrofluoricAcid", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC npc, NPC.HitInfo hit, int damageDone) => {
                            if (npc.HasInstancedBuff(out Sludged buff))
                            {
                                buff.StrongSludge = true;
                            }
                        })
                        .Ingredient(ItemID.HallowedBar, 4)
                        .Ingredient(ItemID.SoulofMight, 4)
                .Tier(5)
                    .Upgrade("HardenedGooMix", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 10;
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 4)
                        .Ingredient(ItemID.SlimeBlock, 30)
                    .Upgrade("PerforatedNozzle", Assets.Upgrades.Damage)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is SludgeBall ball)
                            {
                                ball.NumProjectilesToSpawn += 4;
                            }
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 4)
                        .Ingredient(ItemID.IronBar, 4)
                    .Upgrade("DyseNozzle", Assets.Upgrades.Damage)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper && helper.HasReachedFullCharge)
                            {
                                proj.damage += 20;
                            }
                        })
                        .Ingredient(ItemID.ChlorophyteBar, 4)
                        .Ingredient(ItemID.SoulofNight, 4)
            .Seal();
        }
        public override void AddRecipes() {
            Recipe.Create(ModContent.ItemType<SludgePump>())
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Gel, 50)
            .AddIngredient(ItemID.Bone, 15)
            .AddTile(TileID.Solidifier)
            .Register();
        }
    }
}