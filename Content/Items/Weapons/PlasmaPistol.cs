using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class PlasmaPistol : UpgradableWeapon
    {
        public override void NewSetDefaults() {
            Item.damage = 24;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<PlasmaPistolHelper>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 18f;
            Item.channel = true;
            Item.noMelee = true;
            Item.height = 28;
            Item.width = 30;
            Item.autoReuse = true;

            ShotsUntilCooldown = 16f;
            TimeToEndCooldown = 90f;


            Item.value = Item.sellPrice(0, 1, 60, 0);
        }
        public override UpgradeList InitializeUpgrades() {
            Dictionary<int, List<Vector2>> pointsToElectrify = [];
            return UpgradeBuilder.CreateUpgradeList("PlasmaPistol")
                .WithTier()
                    .WithUpgrade("DenserField1", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.15f);
                        })
                        .WithIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .WithIngredient(ItemID.FallenStar, 3)
                    .WithUpgrade("VacuumDielectric1", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (!source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.damage = (int)(projectile.damage * 1.33f);
                        })
                        .WithIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .WithIngredient(ItemID.Glass, 30)
                    .WithUpgrade("MagneticRails", Assets.Upgrades.BigArrow)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.velocity *= 1.5f;
                        })
                        .WithIngredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .WithIngredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .WithTier()
                    .WithUpgrade("OversizedBattery", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as PlasmaPistol).ShotsUntilCooldown += 8f;
                        })
                        .WithIngredient(ItemID.MeteoriteBar, 4)
                        .WithIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                    .WithUpgrade("CrystalCapacitors", Assets.Upgrades.Focus)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not PlasmaPistolHelper helper) return;

                            helper.ChargeTimeMultiplier = 0.5f;
                        })
                        .WithIngredient(ItemID.MeteoriteBar, 4)
                        .WithIngredient(ItemID.Diamond, 1)
                    .WithUpgrade("HeatPipe", Assets.Upgrades.ArmorBreak)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not PlasmaPistolHelper helper) return;

                            helper.ChargeShotCooldownMultiplier *= 0.75f;
                        })
                        .WithIngredient(ItemID.MeteoriteBar, 4)
                        .WithIngredient([ItemID.CopperBar, ItemID.TinBar], 4)
                .WithTier()
                    .WithUpgrade("DenserField2", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 10;
                        })
                        .WithIngredient(ItemID.Bone, 10)
                        .WithIngredient(ItemID.FallenStar, 3)
                    .WithUpgrade("VacuumDielectric2", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (!source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.damage += 30;
                        })
                        .WithIngredient(ItemID.Bone, 10)
                        .WithIngredient(ItemID.Glass, 30)
                .WithTier()
                    .WithUpgrade("Exorcist", Assets.Upgrades.AreaOfEffect)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.mana = (int)(item.mana * 0.66f);
                        })
                        .WithIngredient(ItemID.HellstoneBar, 4)
                        .WithIngredient(ItemID.ManaCrystal, 2)
                    .WithUpgrade("OversizedBattery", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as PlasmaPistol).ShotsUntilCooldown += 8f;
                        })
                        .WithIngredient(ItemID.HellstoneBar, 4)
                        .WithIngredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
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
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not BigPlasma plasma) return;
                            plasma.CancelAoE = true;
                        })
                        .WithIngredient(ItemID.Bubble, 30)
                        .WithIngredient(ItemID.Dynamite, 10)
                    .WithUpgrade("FlyingNightmare", Assets.Upgrades.Heat)
                        .WithBehavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is not BigPlasma plasma) return;

                            plasma.CancelAoE = true;
                            projectile.penetrate = -1;
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 10;
                        })
                        .WithIngredient(ItemID.Bubble, 30)
                        .WithIngredient(ItemID.Bomb, 30)
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
                        .WithIngredient(ItemID.Bubble, 30)
                        .WithIngredient(ItemID.Grenade, 99)
            //.WithOverclock("PlasmaSplash", Assets.Upgrades.Stun, Overclock.OverclockType.Clean)
            //    .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
            //        if (projectile.ModProjectile is BigPlasma)
            //        {
            //            target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 60);
            //        }
            //        else if (projectile.ModProjectile is PlasmaBullet)
            //        {
            //            target.AddBuff(ModContent.BuffType<StunnedEnemy>(), 20);
            //        }
            //    })
            //.WithOverclock("HeavyHitter", Assets.Upgrades.Damage, Overclock.OverclockType.Balanced)
            //    .WithBehavior<ItemStatChange>((Item item) => {
            //        item.damage = (int)(item.damage * 1.33f);
            //    })
            //    .WithBehavior<HeldProjectileModifyShootStats>((HeldProjectileBase projectile, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
            //        projectile.ChargeShotDamageMultiplier /= 2f;
            //    })
            //.WithOverclock("Ionosphere", Assets.Upgrades.Electricity, Overclock.OverclockType.Unstable)
            //    .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
            //        if (projectile.ModProjectile is not BigPlasma) return;
            //        projectile.velocity *= 0.33f;
            //    })
            //    .WithBehavior<HeldProjectileModifyShootStats>((HeldProjectileBase projectile, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
            //        projectile.ChargeShotDamageMultiplier = 1f;
            //    })
            //    .WithBehavior<ProjectileAI>((Projectile projectile) => {
            //        if (projectile.ModProjectile is not BigPlasma) return;
            //        if (projectile.timeLeft % 20 == 0)
            //        {
            //            List<Vector2> points = [];
            //            int hitNPCs = 0;
            //            foreach (var npc in Main.npc.Where(n => n.active).OrderBy(n => projectile.Center.DistanceSQ(n.Center)))
            //            {
            //                if (hitNPCs >= 5) continue;
            //                if (npc.immortal) continue;
            //                if (projectile.Center.DistanceSQ(npc.Center) > 16f * 16f * 20f * 20f) continue;
            //                // Arc damage
            //                NPC.HitInfo hit = npc.CalculateHitInfo(projectile.damage, 1, damageType: DamageClass.Magic);
            //                Main.player[projectile.owner].StrikeNPCDirect(npc, hit);
            //                npc.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 180);
            //                points.Add(npc.Center);
            //                hitNPCs++;
            //            }
            //            pointsToElectrify[projectile.whoAmI] = points;    
            //        }
            //    })
            //    .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
            //        if (projectile.ModProjectile is BigPlasma)
            //        {
            //            target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
            //        }
            //        else
            //        {
            //            target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 180);
            //        }
            //    })
            //    .WithBehavior<ProjectilePreDraw>((Projectile projectile, Color lightColor) => {
            //        if (projectile.ModProjectile is not BigPlasma || !pointsToElectrify.TryGetValue(projectile.whoAmI, out List<Vector2> list_of_points) || list_of_points.Count == 0) return true;
            //        foreach (var point in list_of_points)
            //        {
            //            float rotation = projectile.Center.DirectionTo(point).ToRotation();
            //            Vector2 midpoint = new((projectile.Center.X + point.X) / 2f, (projectile.Center.Y + point.Y) / 2f);

            //            // Calculate scale via distance
            //            // the arc is 48 pixels, 3 blocks long at 1f scale.
            //            float pixelDistance = projectile.Center.Distance(point);

            //            int frame = Main.rand.Next(0, 3);
            //            int frameHeight = Assets.ElectricityArc.Value.Height / 3;
            //            Rectangle sourceFrame = new(0, frame * frameHeight, Assets.ElectricityArc.Value.Width, frameHeight);

            //            // Get scale from distance between control points
            //            float multiplier = pixelDistance / 48f;

            //            // Draw
            //            Main.EntitySpriteDraw(Assets.ElectricityArc.Value, midpoint - Main.screenPosition, sourceFrame, Color.MediumPurple, rotation, sourceFrame.Size() / 2f, new Vector2(multiplier, frame), SpriteEffects.None);
            //        }
            //        pointsToElectrify[projectile.whoAmI] = [];
            //        return true;
            //    })
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