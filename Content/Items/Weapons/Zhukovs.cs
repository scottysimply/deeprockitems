using deeprockitems.Common.EntitySources;
using deeprockitems.Common.NPCs;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;
using deeprockitems.Content.Projectiles.ZhukovProjectiles;
using deeprockitems.Content.Upgrades;
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

            Item.value = Item.sellPrice(0, 6, 50, 0);
            ShotsUntilCooldown = 30f;
        }
        public override void AddRecipes() {
            Recipe.Create(ModContent.ItemType<Zhukovs>())
                .AddIngredient(ItemID.PhoenixBlaster)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override UpgradeList InitializeUpgrades() {
            Dictionary<int, List<Vector2>> pointsToElectrify = [];
            return UpgradeBuilder.CreateUpgradeList("Zhukovs")
                .WithTier()
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((item) => {
                            item.damage = (int)(item.OriginalDamage * 1.10f);
                        })
                        .WithIngredient([ItemID.HellstoneBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    .WithUpgrade("FireRate", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((item) => {
                            item.useTime = (int)(item.useTime * 0.8f);
                            item.useAnimation = (int)(item.useAnimation * 0.8f);
                        })
                        .WithIngredient([ItemID.HellstoneBar], 8)
                        .WithIngredient([ItemID.SoulofLight], 6)
                .WithTier()
                    .WithUpgrade("ReducedSpread", Assets.Upgrades.Focus)
                        .WithBehavior<ItemModifyShootStats>((Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                            spread /= 2f;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    .WithUpgrade("HighVelocityRounds", Assets.Upgrades.ProjectileVelocity)
                        .WithBehavior<ItemModifyShootStats>((Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                            velocity *= 1.25f;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient(ItemID.HighVelocityBullet, 60)
                .WithTier()
                    .WithUpgrade("DamageUpgrade2", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((item) => {
                            item.damage = (int)(item.OriginalDamage * 1.20f);
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 6)
                    .WithUpgrade("BiggerMagazine", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((item) => {
                            (item.ModItem as UpgradableWeapon).ShotsUntilCooldown *= 1.75f;
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient(ItemID.AmmoReservationPotion, 6)
                .WithTier()
                    .WithUpgrade("GetInGetOut", Assets.Upgrades.Haste)
                        .WithBehavior<ProjectileOnHitNPC>((proj, npc, hit, damage) => {
                            Main.player[proj.owner].AddBuff(ModContent.BuffType<Haste>(), 119);
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient([ItemID.HermesBoots, ItemID.FlurryBoots, ItemID.SailfishBoots, ItemID.SandBoots], 1)
                    .WithUpgrade("Blowthrough", Assets.Upgrades.Penetrate)
                        .WithBehavior<ProjectileOnSpawn>((proj, source) => {
                            if (proj.penetrate >= 3) return;
                            proj.penetrate++;
                        })
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
                            // Fixes the projectile's iframes
                            projectile.localNPCHitCooldown = 20;
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient(ItemID.MeteoriteBar, 6)
                .WithTier()
                    .WithUpgrade("DamageUpgrade3", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage = (int)(item.damage * 1.2f);
                        })
                        .WithIngredient([ItemID.HallowedBar], 8)
                        .WithIngredient(ItemID.HellstoneBar, 6)
                    .WithUpgrade("LightningReload", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            (item.ModItem as Zhukovs).OverheatCooldown *= 0.5f;
                        })
                        .WithIngredient([ItemID.HallowedBar], 8)
                        .WithIngredient(ItemID.FrostCore, 1)
                .WithOverclock("StaticBlast", Assets.Upgrades.Electricity, Overclock.OverclockType.Clean)
                    .WithBehavior<ProjectileAI>((Projectile projectile) => {
                        if (Main.rand.Next(0, projectile.timeLeft % 60) % 60 == 0)
                        {
                            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, Scale: 0.4f);
                        }
                        if (projectile.timeLeft % 40 == 0)
                        {
                            List<Vector2> points = [];
                            int hitNPCs = 0;
                            foreach (var npc in Main.npc.Where(n => n.active).OrderBy(n => projectile.Center.DistanceSQ(n.Center)))
                            {
                                if (hitNPCs >= 1) continue;
                                if (npc.immortal) continue;
                                if (projectile.Center.DistanceSQ(npc.Center) > 16f * 16f * 10f * 10f) continue;
                                // Arc damage
                                NPC.HitInfo hit = npc.CalculateHitInfo(projectile.damage, 1, damageType: DamageClass.Magic);
                                Main.player[projectile.owner].StrikeNPCDirect(npc, hit);
                                npc.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 180);
                                points.Add(npc.Center);
                                hitNPCs++;
                            }
                            pointsToElectrify[projectile.whoAmI] = points;
                        }
                    })
                    .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
                        target.AddBuff(ModContent.BuffType<ElectrifiedEnemy>(), 300);
                    })
                    .WithBehavior<ProjectilePreDraw>((Projectile projectile, Color lightColor) => {
                        if (projectile.ModProjectile is not BigPlasma || !pointsToElectrify.TryGetValue(projectile.whoAmI, out List<Vector2> list_of_points) || list_of_points.Count == 0) return true;
                        foreach (var point in list_of_points)
                        {
                            float rotation = projectile.Center.DirectionTo(point).ToRotation();
                            Vector2 midpoint = new((projectile.Center.X + point.X) / 2f, (projectile.Center.Y + point.Y) / 2f);

                            // Calculate scale via distance
                            // the arc is 48 pixels, 3 blocks long at 1f scale.
                            float pixelDistance = projectile.Center.Distance(point);

                            int frame = Main.rand.Next(0, 3);
                            int frameHeight = Assets.ElectricityArc.Value.Height / 3;
                            Rectangle sourceFrame = new(0, frame * frameHeight, Assets.ElectricityArc.Value.Width, frameHeight);

                            // Get scale from distance between control points
                            float multiplier = pixelDistance / 48f;

                            // Draw
                            Main.EntitySpriteDraw(Assets.ElectricityArc.Value, midpoint - Main.screenPosition, sourceFrame, Color.White, rotation, sourceFrame.Size() / 2f, new Vector2(multiplier, frame), SpriteEffects.None);
                        }
                        pointsToElectrify[projectile.whoAmI] = [];
                        return true;
                    })
                .WithOverclock("CryoMinelets", Assets.Upgrades.Cryo, Overclock.OverclockType.Balanced)
                    .WithBehavior<ProjectileOnTileCollide>((Projectile projectile, Vector2 oldVelocity) => {
                        if (projectile.owner == Main.myPlayer)
                        {
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position - 0.25f * oldVelocity, Vector2.Zero, ModContent.ProjectileType<CryoMineletProjectile>(), 0, 0f, Owner: projectile.owner);
                        }
                        return true;
                    })
                .WithOverclock("EmbeddedDetonators", Assets.Upgrades.AreaOfEffect, Overclock.OverclockType.Unstable)
                    .WithBehavior<ItemAltFunctionUse>((Item item, Player player) => {
                        if (player.itemAnimation > 0) return false;
                        return true;
                    })
                    .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                        if (player.altFunctionUse != 2) return true;
                        foreach (var npc in Main.ActiveNPCs)
                        {
                            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn) && edn.CanDetonatorsActivate())
                            {
                                (item.ModItem as Zhukovs).AddCooldownOnShoot(player, 999f);
                                return false;
                            }
                        }
                        return false;
                    })
                    .WithBehavior<ItemCooldownStart>((Item item, Player player) => {
                        foreach (var npc in Main.ActiveNPCs)
                        {
                            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn))
                            {
                                edn.TryDetonateOnNPC(npc, player);
                            }
                        }
                    })
                    .WithBehavior<ItemOffCooldown>((Item item, Player player, bool cooldownJustEnded) => {
                        if (!cooldownJustEnded) return;
                        foreach (var npc in Main.ActiveNPCs)
                        {
                            if (npc.TryGetGlobalNPC(out EmbeddedDetsNPC edn))
                            {
                                _ = edn.ConvertDetonatorsToWeaker();
                            }
                        }
                    })
                    .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damage) => {
                        if (target.TryGetGlobalNPC(out EmbeddedDetsNPC npc))
                        {
                            npc.IncrementDetonators();
                        }
                    })
                .Seal();
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 24f;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            return true;
        }
    }
}
