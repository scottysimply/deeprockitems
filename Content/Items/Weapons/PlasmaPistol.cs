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
using Terraria.Audio;
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
                .Tier()
                    .Upgrade("DenserField1", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.OriginalDamage * 1.15f);
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient(ItemID.FallenStar, 3)
                    .Upgrade("VacuumDielectric1", Assets.Upgrades.AreaOfEffect)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (!source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.damage = (int)(projectile.damage * 1.5f);
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient(ItemID.Glass, 30)
                    .Upgrade("MagneticRails", Assets.Upgrades.BigArrow)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.velocity *= 1.5f;
                        })
                        .Ingredient([ItemID.DemoniteBar, ItemID.CrimtaneBar], 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .Tier()
                    .Upgrade("OversizedBattery", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as PlasmaPistol).ShotsUntilCooldown += 8f;
                        })
                        .Ingredient(ItemID.MeteoriteBar, 4)
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                    .Upgrade("CrystalCapacitors", Assets.Upgrades.Focus)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not PlasmaPistolHelper helper) return;

                            helper.ChargeTimeMultiplier = 0.5f;
                        })
                        .Ingredient(ItemID.MeteoriteBar, 4)
                        .Ingredient(ItemID.Diamond, 1)
                    .Upgrade("HeatPipe", Assets.Upgrades.FireRate)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not PlasmaPistolHelper helper) return;

                            helper.ChargeShotCooldownMultiplier *= 0.66f;
                        })
                        .Ingredient(ItemID.MeteoriteBar, 4)
                        .Ingredient([ItemID.CopperBar, ItemID.TinBar], 4)
                .Tier()
                    .Upgrade("DenserField2", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 10;
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient(ItemID.FallenStar, 3)
                    .Upgrade("VacuumDielectric2", Assets.Upgrades.AreaOfEffect)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (!source.SourceProjectile.HasReachedFullCharge) return;

                            projectile.damage += 45;
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient(ItemID.Glass, 30)
                .Tier()
                    .Upgrade("Exorcist", Assets.Upgrades.AreaOfEffect)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.mana = (int)(item.mana * 0.66f);
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.ManaCrystal, 2)
                    .Upgrade("OversizedBattery", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as PlasmaPistol).ShotsUntilCooldown += 8f;
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient([ItemID.GoldBar, ItemID.PlatinumBar], 4)
                .Tier()
                    .Upgrade("ThinContainmentField", Assets.Upgrades.SpecialStar)
                        .Behavior<ProjectileAI>((Projectile projectile) => {
                            if (projectile.ModProjectile is not PlasmaBullet) return;

                            int intersection = projectile.IsCollidingWithProjectile(ModContent.ProjectileType<BigPlasma>());
                            if (intersection == -1) return;
                            if (projectile.owner != Main.myPlayer && Main.projectile[intersection].owner != projectile.owner) return;

                            
                            Projectile.NewProjectile(projectile.GetSource_FromAI(), Main.projectile[intersection].Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), projectile.damage * 3, 0f, projectile.owner);
                            SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -1f, PitchVariance = 0.1f }, Main.projectile[intersection].Center);
                            Main.projectile[intersection].Kill();
                            projectile.Kill();
                        })
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not BigPlasma plasma) return;
                            plasma.CancelAoE = true;
                        })
                        .Ingredient(ItemID.Bubble, 30)
                        .Ingredient(ItemID.Dynamite, 10)
                    .Upgrade("FlyingNightmare", Assets.Upgrades.Penetrate)
                        .Behavior<HeldProjectilePostSpawn>((Projectile projectile, EntitySource_FromHeldProjectile source) => {
                            if (projectile.ModProjectile is not BigPlasma plasma) return;

                            plasma.CancelAoE = true;
                            projectile.penetrate = -1;
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 10;
                        })
                        .Ingredient(ItemID.Bubble, 30)
                        .Ingredient(ItemID.Bomb, 30)
                    .Upgrade("PlasmaSplash", Assets.Upgrades.AreaOfEffect)
                        .Behavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                            if (projectile.ModProjectile is not PlasmaBullet plasma) return true;
                            if (projectile.owner != Main.myPlayer) return false;
                            if (plasma.IsExploding) return true;
                            plasma.Explode();
                            return false;
                        })
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            if (projectile.ModProjectile is not PlasmaBullet plasma) return;
                            if (projectile.owner != Main.myPlayer) return;
                            if (plasma.IsExploding) return;
                            plasma.Explode();
                        })
                        .Ingredient(ItemID.Bubble, 30)
                        .Ingredient(ItemID.Grenade, 99)
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