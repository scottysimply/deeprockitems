using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            Item.shoot = ModContent.ProjectileType<PlasmaPistolHelper>();
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
            return UpgradeBuilder.CreateUpgradeList("PlasmaPistol")
                .WithTier()
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.1f);
                        })
                        .WithIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 1)
                    .WithUpgrade("IncreasedBattery", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as UpgradableWeapon).ShotsUntilCooldown = 24f;
                        })
                        .WithIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 8)
                        .WithIngredient(ItemID.Amethyst, 5)
                    .WithUpgrade("IncreasedChargeDamage", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (!source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.damage = (int)(projectile.damage * 1.25f);
                        })
                        .WithIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 8)
                        .WithIngredient(ItemID.FallenStar, 5)
                .WithTier()
                    .WithUpgrade("SuperSpeedPlasma", Assets.Upgrades.BigArrow)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            projectile.velocity *= 1.25f;
                        })
                        .WithIngredient(ItemID.MeteoriteBar, 8)
                        .WithIngredient([ItemID.HermesBoots, ItemID.FlurryBoots, ItemID.SandBoots, ItemID.SailfishBoots])
                    .WithUpgrade("QuickCharge", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not PlasmaPistolHelper helper) return;

                            helper.ChargeTimeMultiplier = 0.75f;
                        })
                        .WithIngredient(ItemID.MeteoriteBar, 8)
                        .WithIngredient(ItemID.SwiftnessPotion, 3)
                .WithTier()
                    .WithUpgrade("ArmorBreak", Assets.Upgrades.ArmorBreak)
                        .WithBehavior<ProjectileModifyHitNPC>((Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) => {
                            modifiers.ScalingArmorPenetration += 0.25f;
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient(ItemID.SharkToothNecklace)
                    .WithUpgrade("FireRateIncrease", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.useTime = item.useAnimation = (int)(item.useTime * 0.67f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient(ItemID.Amethyst, 5)
                    .WithUpgrade("MediumDamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.25f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 2)
                .WithTier()
                    .WithUpgrade("PlasmaSplash", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                            if (projectile.ModProjectile is not PlasmaBullet plasma) return true;
                            if (projectile.owner != Main.myPlayer) return false;
                            if (plasma.IsExploding) return true;
                            plasma.Explode();
                            return false;
                        })
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            if (projectile.ModProjectile is not PlasmaBullet plasma) return;
                            if (projectile.owner != Main.myPlayer) return;
                            if (plasma.IsExploding) return;
                            plasma.Explode();
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.Grenade, 10)
                    .WithUpgrade("FlyingNightmare", Assets.Upgrades.Penetrate)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is not BigPlasma) return;

                            projectile.penetrate = -1;
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 10;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient([ItemID.Vilethorn, ItemID.CrimsonRod])
                .WithTier()
                    .WithUpgrade("ThinContainmentField", Assets.Upgrades.SpecialStar)
                        .WithBehavior<ProjectileAI>((Projectile projectile) => {
                            if (projectile.ModProjectile is not PlasmaBullet) return;

                            int intersection = projectile.IsCollidingWithProjectile(ModContent.ProjectileType<BigPlasma>());
                            if (intersection == -1) return;
                            if (projectile.owner != Main.myPlayer && Main.projectile[intersection].owner != projectile.owner) return;

                            
                            Projectile.NewProjectile(projectile.GetSource_FromAI(), Main.projectile[intersection].Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), projectile.damage * 3, 0f, projectile.owner);
                            Main.projectile[intersection].Kill();
                            projectile.Kill();
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.Dynamite, 10)
                    .WithUpgrade("HeatDump", Assets.Upgrades.Heat)
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            int heatAmount = projectile.ModProjectile switch {
                                BigPlasma => 64,
                                _ => 24,
                            };
                            target.ChangeTemperature(heatAmount);
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.AncientBattleArmorMaterial)
            .Seal();
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 64f;
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