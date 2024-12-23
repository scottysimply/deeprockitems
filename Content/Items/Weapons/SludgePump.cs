using deeprockitems.Common.PlayerLayers;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Content.Tiles;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
        /*public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("SludgePump",
                new UpgradeTier(1,
                    new Upgrade("VisualCalculus", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_HoldItemHook = (item, player) => {
                                player.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw = true;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.MechanicalLens, 1)
                                    .AddIngredient(ItemID.Gel, 30)
                    },
                    new Upgrade("Glowstick", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_PreDrawHook = (projectile, lightColor) => {
                                if (projectile.ModProjectile is not SludgeBall) return true;

                                Main.EntitySpriteDraw(new DrawData(TextureAssets.Projectile[projectile.type].Value, projectile.getRect(), Color.White));
                                return true;
                            },
                            Projectile_AIHook = (projectile) => {
                                if (projectile.ModProjectile is not SludgeBall) return;

                                Lighting.AddLight(projectile.position, new Vector3(0.05f, 0.9f, 0.05f));
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.Glowstick, 15)
                                    .AddIngredient(ItemID.Gel, 30)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("EfficientCharge", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is SludgeHelper helper)
                                {
                                    helper.ChargeShotCooldownMultiplier = 1.5f;
                                }
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 6)
                                    .AddIngredient(ItemID.Gel, 30)
                    },
                    new Upgrade("QuickCharge", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is SludgeHelper helper)
                                {
                                    helper.ChargeTime *= 0.75f;
                                }
                            },
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddIngredient(ItemID.SwiftnessPotion, 3)
                    }
                ),
                new UpgradeTier(3,
                    *//*new Upgrade("LingeringSludge", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Projectile_OnTileCollideHook = (proj, oldVelocity) => {
                                // get instance of modsystem
                                SludgeTileSystem system = ModContent.GetInstance<SludgeTileSystem>();
                                // Convert to center of the tile
                                Vector2 collisionPoint = proj.Center;
                                // Get the tile collision points of this projectile.
                                Point topLeftTileIntersection = new Point((int)Math.Floor((proj.position.X - 0.5f * proj.width) / 16f),
                                                                          (int)Math.Floor((proj.position.Y - 0.5f * proj.height) / 16f));
                                Point bottomRightTileIntersection = new Point((int)Math.Ceiling((proj.position.X + 1.5f * proj.width) / 16f),
                                                                              (int)Math.Ceiling((proj.position.Y + 1.5f * proj.height) / 16f));
                                // We can then for-each these tiles and try drawing a line to the center of each face of them.
                                for (int i = topLeftTileIntersection.X; i <= bottomRightTileIntersection.X; i++)
                                {
                                    for (int j = topLeftTileIntersection.Y; j <= bottomRightTileIntersection.Y; j++)
                                    {
                                        // preliminary check to make sure we arent sludging air blocks
                                        if (!Main.tile[i, j].HasTile || !Main.tileSolid[Main.tile[i, j].TileType] || Main.tileFrameImportant[Main.tile[i, j].TileType]) continue;

                                        // Get the rough positions of the centers of tile faces
                                        Vector2 topFace = new Vector2(i * 16f + 8f, j * 16f - 2f);
                                        Vector2 rightFace = new Vector2(i * 16f + 18f, j * 16f + 8f);
                                        Vector2 bottomFace = new Vector2(i * 16f + 8f, j * 16f + 18f);
                                        Vector2 leftFace = new Vector2(i * 16f - 2f, j * 16f + 8f);
                                        // If we can hit that rough center, then that surface should receive sludge.
                                        SludgeSurfaces surfaces = SludgeSurfaces.None;
                                        if (Collision.CanHitLine(collisionPoint - oldVelocity, 1, 1, topFace, 1, 1))
                                        {
                                            surfaces |= SludgeSurfaces.Top;
                                        }
                                        if (Collision.CanHitLine(collisionPoint - oldVelocity, 1, 1, rightFace, 1, 1))
                                        {
                                            surfaces |= SludgeSurfaces.Right;
                                        }
                                        if (Collision.CanHitLine(collisionPoint - oldVelocity, 1, 1, bottomFace, 1, 1))
                                        {
                                            surfaces |= SludgeSurfaces.Bottom;
                                        }
                                        if (Collision.CanHitLine(collisionPoint - oldVelocity, 1, 1, leftFace, 1, 1))
                                        {
                                            surfaces |= SludgeSurfaces.Left;
                                        }
                                        // Send tile data
                                        if (surfaces > 0)
                                        {
                                            system.AddNewSludgeTile(i, j, 600, surfaces);
                                        }
                                    }
                                }
                                return true; // Continue vanilla behavior.
                            }
                        }*//*
                    new Upgrade("SpreadingSludge", Assets.Upgrades.GooBall.Value) {
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddIngredient(ItemID.PinkGel, 15)
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.25f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("MoreFragments", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is SludgeBall ball)
                                {
                                    ball.NumProjectilesToSpawn += 4;
                                }
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddIngredient(ItemID.Gel, 30)
                    },
                    new Upgrade("WasteOrdnance", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_PreKillHook = (projectile, timeLeft) => {
                                if (projectile.ModProjectile is not SludgeBall ball) return true;
                                // Spawn the waste ordance if the projectile is fully charged
                                if (!ball.ShouldSplatter) return true;

                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)(projectile.damage * 2f), 0f, Owner: projectile.owner);
                                return false;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 6)
                                    .AddIngredient(ItemID.Bomb, 15)
                    }
                ),
                new UpgradeTier(5,
                    // All 3 upgrades in this tier will change how the buff behaves.
                    new Upgrade("StrongSludge", Assets.Upgrades.Heat.Value) {
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.OrichalcumBar], 8)
                                    .AddIngredient(ItemID.Stinger, 6)
                    },
                    new Upgrade("SlowingPoison", Assets.Upgrades.Stun.Value) {
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                                    .AddIngredient(ItemID.HoneyComb, 6)
                    }
                )
            );
        }*/
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