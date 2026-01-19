using deeprockitems.Common.EntitySources;
using deeprockitems.Common.NPCs;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.Globals;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Projectiles.ZhukovProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradableWeapon
    {
        public override void NewSetDefaults() {
            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 34;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 12;
            Item.knockBack = 1f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            ShotsUntilCooldown = 20f;
            TimeToEndCooldown = 80f;
        }
        public override void AddRecipes() {
            Recipe.Create(ModContent.ItemType<Zhukovs>())
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(ItemID.Bone, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override UpgradeList InitializeUpgrades() {
            //Dictionary<int, List<Vector2>> pointsToElectrify = [];
            return UpgradeBuilder.CreateUpgradeList("Zhukovs")
                .Tier()
                    .Upgrade("HighCaliber1", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.damage * 1.15f);
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    .Upgrade("FireRate", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.useTime = (int)(item.useTime * 0.75f);
                            item.useAnimation = (int)(item.useAnimation * 0.75f);
                        })
                        .Ingredient(ItemID.Bone, 10)
                        .Ingredient(ItemID.IllegalGunParts)
                .Tier()
                    .Upgrade("MagSize1", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as Zhukovs).ShotsUntilCooldown += 10f;
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.MusketBall, 99)
                    .Upgrade("ReloadSpeed", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as Zhukovs).TimeToEndCooldown -= 30f;
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.Feather, 5)
                    .Upgrade("Accuracy", Assets.Upgrades.Focus)
                        .Behavior<ItemModifyShootStats>((Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                            spread *= 0.5f;
                        })
                        .Ingredient(ItemID.HellstoneBar, 4)
                        .Ingredient(ItemID.Glass, 25)
                .Tier()
                    .Upgrade("ArmorPenetration", Assets.Upgrades.ArmorBreak)
                        .Behavior<ProjectileModifyHitNPC>((Projectile projectile, NPC npc, ref NPC.HitModifiers modifiers) => {
                            modifiers.ScalingArmorPenetration += 0.25f;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient([ItemID.CopperBar, ItemID.TinBar], 4)
                    .Upgrade("HighCaliber2", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 6;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .Tier()
                    .Upgrade("MagSize2", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            (item.ModItem as Zhukovs).ShotsUntilCooldown += 10f;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 4)
                        .Ingredient(ItemID.MusketBall, 99)
                    .Upgrade("Stun", Assets.Upgrades.Stun)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC npc, NPC.HitInfo hit, int damageDone) => {
                            if (Main.rand.NextFloat() <= .3f)
                            {
                                npc.AddBuff(ModContent.BuffType<StunnedEnemy>(), 480);
                            }
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 4)
                        .Ingredient([ItemID.CopperBar, ItemID.TinBar], 4)
                    .Upgrade("Blowthrough", Assets.Upgrades.Penetrate)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            projectile.penetrate += 1;
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 12;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 4)
                        .Ingredient([ItemID.IronBar, ItemID.LeadBar], 4)
                .Tier()
                    .Upgrade("HighCaliber3", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 6;
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                        .Ingredient(ItemID.SoulofMight, 4)
                    .Upgrade("BattleFrenzy", Assets.Upgrades.Haste)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC npc, NPC.HitInfo hit, int damageDone) => {
                            if (projectile.owner == Main.myPlayer)
                            {
                                Main.LocalPlayer.AddBuff(ModContent.BuffType<Haste>(), 119);
                            }
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 4)
                        .Ingredient(ItemID.SoulofMight, 4)
                //.WithOverclock("StaticBlast", Assets.Upgrades.Electricity, Overclock.OverclockType.Clean)
                //    .WithBehavior<ProjectileAI>((Projectile projectile) => {
                //        if (Main.rand.Next(0, projectile.timeLeft % 60) % 60 == 0)
                //        {
                //            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, Scale: 0.4f);
                //        }
                //        if (projectile.timeLeft % 40 == 0)
                //        {
                //            List<Vector2> points = [];
                //            int hitNPCs = 0;
                //            foreach (var npc in Main.npc.Where(n => n.active).OrderBy(n => projectile.Center.DistanceSQ(n.Center)))
                //            {
                //                if (hitNPCs >= 1) continue;
                //                if (npc.immortal) continue;
                //                if (projectile.Center.DistanceSQ(npc.Center) > 16f * 16f * 15f * 15f) continue;
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
                //        target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
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
                //            Main.EntitySpriteDraw(Assets.ElectricityArc.Value, midpoint - Main.screenPosition, sourceFrame, Color.White, rotation, sourceFrame.Size() / 2f, new Vector2(multiplier, frame), SpriteEffects.None);
                //        }
                //        pointsToElectrify[projectile.whoAmI] = [];
                //        return true;
                //    })
                //.WithOverclock("CryoMinelets", Assets.Upgrades.Cryo, Overclock.OverclockType.Clean)
                //    .WithBehavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                //        if (projectile.type == ModContent.ProjectileType<CryoMineletProjectile>()) return false;
                //        if (projectile.owner == Main.myPlayer)
                //        {
                //            Point spawnTile = projectile.Center.ToTileCoordinates();
                //            // Move projectile right
                //            if (oldVelocity.X > projectile.velocity.X)
                //            {
                //                spawnTile.X++;
                //            }
                //            // Move projectile left
                //            if (oldVelocity.X < projectile.velocity.X)
                //            {
                //                spawnTile.X--;
                //            }
                //            // Move projectile down
                //            if (oldVelocity.Y > projectile.velocity.Y)
                //            {
                //                spawnTile.Y++;
                //            }
                //            // Move projectile up
                //            if (oldVelocity.Y < projectile.velocity.Y)
                //            {
                //                spawnTile.Y--;
                //            }
                //            Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<CryoMineletProjectile>(), projectile.damage, 0f, projectile.owner, ai0: 40f, ai1: spawnTile.X, ai2: spawnTile.Y);
                //            proj.position += projectile.velocity * 2f;
                //        }
                //        return true;
                //    })
                //.WithOverclock("EmbeddedDetonators", Assets.Upgrades.AreaOfEffect, Overclock.OverclockType.Unstable)
                //    .WithBehavior<ItemStatChange>((Item item) => {
                //        item.damage = (int)(item.damage * 0.8f);
                //        item.useTime = (int)Math.Ceiling(item.useTime * 1.2f);
                //        item.useAnimation = (int)Math.Ceiling(item.useAnimation * 1.2f);
                //    })
                //    .WithBehavior<ItemAltFunctionUse>((Item item, Player player) => {
                //        if (player.itemAnimation > 0) return false;
                //        return true;
                //    })
                //    .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                //        if (player.altFunctionUse != 2) return true;
                //        foreach (var npc in Main.ActiveNPCs)
                //        {
                //            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn) && edn.CanDetonatorsActivate())
                //            {
                //                (item.ModItem as Zhukovs).AddCooldownOnShoot(player, 999f);
                //                return false;
                //            }
                //        }
                //        return false;
                //    })
                //    .WithBehavior<ItemCooldownStart>((Item item, Player player) => {
                //        foreach (var npc in Main.ActiveNPCs)
                //        {
                //            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn))
                //            {
                //                edn.TryDetonateOnNPC(npc, player);
                //            }
                //        }
                //    })
                //    .WithBehavior<ItemOffCooldown>((Item item, Player player, bool cooldownJustEnded) => {
                //        if (!cooldownJustEnded) return;
                //        foreach (var npc in Main.ActiveNPCs)
                //        {
                //            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn))
                //            {
                //                _ = edn.ConvertDetonatorsToWeaker();
                //            }
                //        }
                //    })
                //    .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
                //        if (target.TryGetGlobalNPC(out EmbeddedDetsNPC npc))
                //        {
                //            npc.IncrementDetonators();
                //        }
                //    })
                .Seal();
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 21f;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            return true;
        }
    }
}
