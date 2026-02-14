using deeprockitems.Common.EntitySources;
using deeprockitems.Common.PlayerLayers;
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

            TimeToEndCooldown = 110f;
            ShotsUntilCooldown = 18f;

            Item.value = Item.sellPrice(0, 5, 30, 0);

        }
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("SludgePump")
                .Tier(1)
                    .Upgrade("VisualCalculus", Assets.Upgrades.Focus)
                        .Behavior<ItemHoldItem>((item, player) => {
                            player.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw = true;
                        })
                        .Ingredient(ItemID.MechanicalLens, 1)
                        .Ingredient(ItemID.Gel, 30)
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
                        .Ingredient(ItemID.Glowstick, 15)
                        .Ingredient(ItemID.Gel, 30)
                .Tier(2)
                    .Upgrade("EfficientCharge", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper)
                            {
                                helper.ChargeShotCooldownMultiplier = 1.5f;
                            }
                        })
                        .Ingredient(ItemID.HellstoneBar, 6)
                        .Ingredient(ItemID.Gel, 30)
                    .Upgrade("QuickCharge", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeHelper helper)
                            {
                                helper.ChargeTime *= 0.75f;
                            }
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .Ingredient(ItemID.SwiftnessPotion, 3)
                .Tier(3)
                    .Upgrade("SpreadingSludge", Assets.Upgrades.GooBall)
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .Ingredient(ItemID.PinkGel, 15)
                    .Upgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((item) => {
                            item.damage = (int)(item.OriginalDamage * 1.25f);
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .Ingredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                .Tier(4)
                    .Upgrade("MoreFragments", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.ModProjectile is SludgeBall ball)
                            {
                                ball.NumProjectilesToSpawn += 4;
                            }
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient(ItemID.Gel, 30)
                    .Upgrade("WasteOrdnance", Assets.Upgrades.Penetrate)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is not SludgeBall ball) return;
                            if (!source.SourceProjectile.HasReachedFullCharge) return;
                            ball.ShouldExplode = true;
                            ball.ShouldSplatter = false;
                        })
                        .Behavior<ProjectilePreKill>((projectile, timeLeft) => {
                            if (projectile.ModProjectile is SludgeBall ball)
                            {
                                if (!ball.ShouldExplode) return true;

                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)(projectile.damage * 2f), 0f, Owner: projectile.owner);
                                return false;
                            }
                            else if (projectile.ModProjectile is SludgeFragment fragment)
                            {
                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SmallSludgeExplosion>(), (int)(projectile.damage * 1f), 0f, Owner: projectile.owner);
                                return false;
                            }
                            return true;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 6)
                        .Ingredient(ItemID.Bomb, 15)
                .Tier(5)
                    .Upgrade("StrongSludge", Assets.Upgrades.Heat)
                        .Ingredient([ItemID.CobaltBar, ItemID.OrichalcumBar], 8)
                        .Ingredient(ItemID.Stinger, 6)
                    .Upgrade("SlowingPoison", Assets.Upgrades.Stun)
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .Ingredient(ItemID.HoneyComb, 6)
                .Overclock("OvertunedNozzles", Assets.Upgrades.Damage, Overclock.OverclockType.Clean)
                    .Behavior<ItemStatChange>((Item item) => {
                        item.damage = (int)(item.damage * 1.4f);
                        (item.ModItem as SludgePump).ShotsUntilCooldown *= 1.25f;
                        (item.ModItem as SludgePump).TimeToEndCooldown *= 0.85f;
                    })
                .Overclock("SludgeBlast", Assets.Upgrades.Focus, Overclock.OverclockType.Balanced)
                    .Behavior<ItemStatChange>((Item item) => {
                        (item.ModItem as SludgePump).TimeToEndCooldown *= 1.2f;
                    })
                    .Behavior<HeldProjectileShoot>((HeldProjectileBase helper, Item item, Player player, EntitySource_FromHeldProjectile source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                        // Shoot 4 projectiles in a cone-ish shape, with slight spread
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(source, position, Main.rand.NextFloat(0.9f, 1.1f) * velocity.RotatedBy(0.05f * (i - 1.5f)).RotatedByRandom(0.02f), type, damage / 2, knockback, owner: player.whoAmI);
                            if (helper.HasReachedFullCharge && proj.ModProjectile is SludgeBall ball)
                            {
                                ball.ShouldSplatter = true;
                                ball.NumProjectilesToSpawn /= 2;
                            }
                        }
                        return false;
                    })
                .Overclock("GooBomberSpecial", Assets.Upgrades.SpecialStar, Overclock.OverclockType.Unstable)
                    .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                        if (projectile.ModProjectile is not SludgeHelper helper) return;
                        helper.ChargeTimeMultiplier *= 1.33f;
                    })
                    .Behavior<ProjectileAI>((Projectile projectile) => {
                        if (projectile.ModProjectile is not SludgeBall ball) return;
                        if (!(ball.ShouldSplatter || ball.ShouldExplode)) return;
                        ball.ShouldExplode = false;
                        ball.ShouldSplatter = true;
                        projectile.ai[2]++;
                        if (projectile.ai[2] % (16 - (int)(ball.NumProjectilesToSpawn / 1.4f)) == 0)
                        {
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, new Vector2(0f, -2f), ModContent.ProjectileType<SludgeFragment>(), 2*projectile.damage, projectile.knockBack, Owner: projectile.owner);
                        }
                    })
                    .Behavior<ProjectilePreKill>((Projectile projectile, int timeLeft) => {
                        if (projectile.ModProjectile is not SludgeBall) return true;
                        return false;
                    })
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