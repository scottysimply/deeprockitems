using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Projectiles.CryoCannonProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Content.Buffs;
using System.Linq;
using System.Collections.Generic;
using Terraria.Audio;
using static deeprockitems.Content.Upgrades.UpgradeBehavior;

namespace deeprockitems.Content.Items.Weapons
{
    public class CryoCannon : UpgradableWeapon
    {
        private Dictionary<int, List<Upgrade>> upgrades;
        public override void NewSetStaticDefaults() {
            upgrades = UpgradeFactory.CreateUpgradeList("test")
                        .WithTier(1)
                            .WithUpgrade("ExampleUpgrade", Assets.Upgrades.Damage.Value)
                                .WithBehavior<ProjectilePreKill>((Projectile projectile, int timeLeft) => {
                                    Main.NewText("test");
                                    return true;
                                }).SealUpgrade()
                        .Seal();
        }
        public override void NewSetDefaults()
        {
            Item.width = 40;
            Item.height = 32;
            Item.mana = 6;
            Item.damage = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<CryoProjectile>();
            Item.useAnimation = Item.useTime = 6;
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            this.ShotsUntilCooldown = 40f;
            this.TimeToEndCooldown = 180f;
        }
        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("CryoCannon",
                new UpgradeTier(1,
                    new Upgrade("IncreasedCooling", Assets.Upgrades.Cryo.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not CryoProjectile cryo) return;
                                cryo.CoolingAmount -= 2f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddIngredient(ItemID.IceBlock, 50)
                    },
                    new Upgrade("FartherStream", Assets.Upgrades.BigArrow.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                    if (proj.ModProjectile is not CryoProjectile cryo) return;
                                    cryo.VelocityDecay += 0.02f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddIngredient(ItemID.SoulofNight, 6)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("ReducedManaCost", Assets.Upgrades.SpecialStar.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.mana -= 1;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddIngredient(ItemID.FallenStar, 5)
                    },
                    new Upgrade("FireRate", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime -= 4;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddIngredient(ItemID.SoulofLight, 6)
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage += 2;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("FartherStream", Assets.Upgrades.BigArrow.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                    if (proj.ModProjectile is not CryoProjectile cryo) return;
                                    cryo.VelocityDecay += 0.02f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                                    .AddIngredient(ItemID.SoulofNight, 6)
                    },
                    new Upgrade("ReloadSpeed", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                OverheatCooldown *= 0.75f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                                    .AddIngredient(ItemID.ManaRegenerationPotion, 6)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("IncreasedCooling", Assets.Upgrades.Cryo.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.ModProjectile is not CryoProjectile cryo) return;
                                cryo.CoolingAmount -= 2f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddIngredient(ItemID.IceBlock, 50)
                    },
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage += 2;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    },
                    new Upgrade("ReducedManaCost", Assets.Upgrades.SpecialStar.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.mana -= 1;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddIngredient(ItemID.FallenStar, 5)
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("ColdRadiance", Assets.Upgrades.Cryo.Value) {
                        Behavior = {
                            Item_OnShootHook = (item, player, source, projectile) => {
                                // Query for enemies nearby the player (5 tiles)
                                var npcs = Main.npc.Where(npc => npc.active && npc.Center.DistanceSQ(player.Center) <= 6400);
                                foreach (var npc in npcs)
                                {
                                    npc.ChangeTemperature(-8, player.whoAmI);
                                }
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.ChlorophyteBar, 8)
                                    .AddIngredient(ItemID.InfernoPotion, 6)
                    },
                    new Upgrade("ReversedEntropy", Assets.Upgrades.SpecialStar.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                if (npc.GetTemperature() > -25) return;
                                // This ensures that we only try freezing each NPC once
                                bool[] frozenWhoAmIs = new bool[Main.npc.Length];
                                // This function will recursively freeze NPCs
                                void ChainTemperature(NPC npcToFreeze) {
                                    // Don't continue the chain if the NPC doesn't have the correct temperature
                                    if (npcToFreeze.GetTemperature() > -25) return;
                                    foreach (var potentialNPC in Main.npc)
                                    {
                                        // No inactive NPCs, no selves, NPCs we've frozen before, NPCs far away
                                        if (!potentialNPC.active || npcToFreeze.whoAmI == potentialNPC.whoAmI || frozenWhoAmIs[potentialNPC.whoAmI] || npcToFreeze.Center.DistanceSQ(potentialNPC.Center) > 4096) continue;
                                        // Set as frozen
                                        frozenWhoAmIs[potentialNPC.whoAmI] = true;
                                        // Change temperature
                                        potentialNPC.ChangeTemperature(-1, proj.owner);
                                        // Continue the chain
                                        ChainTemperature(potentialNPC);
                                    }
                                };
                                // Begin the lag spikening
                                ChainTemperature(npc);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.ChlorophyteBar, 8)
                                    .AddIngredient(ItemID.FrostCore, 1)
                    }
                )
            );
        }
        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 40;
            SoundEngine.PlaySound(SoundID.Item13 with { Pitch = -0.5f, PitchVariance = 0.25f}, position: position);
        }
        public override void AddRecipes() {
            Recipe.Create(Type)
                .AddRecipeGroup(nameof(ItemID.CobaltBar), 12)
                .AddIngredient(ItemID.FrostCore, 1)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
