using deeprockitems.Common.PlayerLayers;
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
            Item.damage = 34;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.width = 70;
            Item.height = 36;
            Item.mana = 10;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<SludgeHelper>();
            Item.shootSpeed = 22f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;

            Item.value = Item.sellPrice(0, 5, 30, 0);

        }
        public override void ResetStats() {
            Item.damage = Item.OriginalDamage;
            TimeToEndCooldown = 110f;
            ShotsUntilCooldown = 24f;
        }
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("SludgePump")
                .WithTier(1)
                    .WithUpgrade("VisualCalculus", Assets.Upgrades.Focus)
                        .WithBehavior<ItemHoldItem>((item, player) => {
                            player.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw = true;
                        })
                        .WithIngredient(ItemID.MechanicalLens, 1)
                        .WithIngredient(ItemID.Gel, 30)
                    .WithUpgrade("Glowstick", Assets.Upgrades.Heat)
                        .WithBehavior<ProjectilePreDraw>((projectile, lightColor) => {
                            if (projectile.ModProjectile is not SludgeBall) return true;

                            Main.EntitySpriteDraw(new DrawData(TextureAssets.Projectile[projectile.type].Value, projectile.getRect(), Color.White));
                            return true;
                        })
                        .WithBehavior<ProjectileAI>((projectile) => {
                            if (projectile.ModProjectile is not SludgeBall) return;

                            Lighting.AddLight(projectile.position, new Vector3(0.05f, 0.9f, 0.05f));
                        })
                        .WithIngredient(ItemID.Glowstick, 15)
                        .WithIngredient(ItemID.Gel, 30)
                .WithTier(2)
                    .WithUpgrade("EfficientCharge", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper)
                            {
                                helper.ChargeShotCooldownMultiplier = 1.5f;
                            }
                        })
                        .WithIngredient(ItemID.HellstoneBar, 6)
                        .WithIngredient(ItemID.Gel, 30)
                    .WithUpgrade("QuickCharge", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper)
                            {
                                helper.ChargeTime *= 0.75f;
                            }
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.SwiftnessPotion, 3)
                .WithTier(3)
                    .WithUpgrade("SpreadingSludge", Assets.Upgrades.GooBall)
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.PinkGel, 15)
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((item) => {
                            item.damage = (int)(item.OriginalDamage * 1.25f);
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                .WithTier(4)
                    .WithUpgrade("MoreFragments", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeBall ball)
                            {
                                ball.NumProjectilesToSpawn += 4;
                            }
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient(ItemID.Gel, 30)
                    .WithUpgrade("WasteOrdnance", Assets.Upgrades.Penetrate)
                        .WithBehavior<ProjectilePreKill>((projectile, timeLeft) => {
                            if (projectile.ModProjectile is not SludgeBall ball) return true;
                            if (!ball.ShouldSplatter) return true;

                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)(projectile.damage * 2f), 0f, Owner: projectile.owner);
                            return false;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 6)
                        .WithIngredient(ItemID.Bomb, 15)
                .WithTier(5)
                    .WithUpgrade("StrongSludge", Assets.Upgrades.Heat)
                        .WithIngredient([ItemID.CobaltBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient(ItemID.Stinger, 6)
                    .WithUpgrade("SlowingPoison", Assets.Upgrades.Stun)
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.HoneyComb, 6)
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