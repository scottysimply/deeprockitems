using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradableWeapon
    {
        public override void NewSetDefaults()
        {
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
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<Zhukovs>())
                .AddIngredient(ItemID.PhoenixBlaster)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
/*        public override UpgradeList InitializeUpgrades() {
            return new UpgradeList("Zhukovs",
                new UpgradeTier(1,
                    new Upgrade("DamageUpgrade", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.10f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    },
                    new Upgrade("FireRate", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.useTime = (int)(item.useTime * 0.8f);
                                item.useAnimation = (int)(item.useAnimation * 0.8f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HellstoneBar, 8)
                                    .AddIngredient(ItemID.SoulofLight, 6)
                    }
                ),
                new UpgradeTier(2,
                    new Upgrade("ReducedSpread", Assets.Upgrades.Focus.Value) {
                        Behavior = {
                            Item_ModifyShootStatsHook = (Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                                spread /= 2f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddCandidateIngredient([ItemID.IronBar, ItemID.LeadBar], 4)
                    },
                    new Upgrade("HighVelocityRounds", Assets.Upgrades.ProjectileVelocity.Value) {
                        Behavior = {
                            Item_ModifyShootStatsHook = (Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) => {
                                velocity *= 1.25f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                                    .AddIngredient(ItemID.HighVelocityBullet, 60)
                    }
                ),
                new UpgradeTier(3,
                    new Upgrade("DamageUpgrade2", Assets.Upgrades.Damage.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                item.damage = (int)(item.OriginalDamage * 1.20f);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddCandidateIngredient([ItemID.RagePotion, ItemID.WrathPotion], 6)
                    },
                    new Upgrade("BiggerMagazine", Assets.Upgrades.FireRate.Value) {
                        Behavior = {
                            Item_ModifyStats = (item) => {
                                (item.ModItem as UpgradableWeapon).ShotsUntilCooldown *= 1.75f;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                                    .AddIngredient(ItemID.AmmoReservationPotion, 6)
                    }
                ),
                new UpgradeTier(4,
                    new Upgrade("GetInGetOut", Assets.Upgrades.Haste.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                Main.player[proj.owner].AddBuff(ModContent.BuffType<Haste>(), 119);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                                    .AddCandidateIngredient([ItemID.HermesBoots, ItemID.FlurryBoots, ItemID.SailfishBoots, ItemID.SandBoots], 1)
                    },
                    new Upgrade("Blowthrough", Assets.Upgrades.Penetrate.Value) {
                        Behavior = {
                            Projectile_OnSpawnHook = (proj, source) => {
                                if (proj.penetrate > 5) return;
                                proj.penetrate = 5;
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddCandidateIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                                    .AddIngredient(ItemID.MeteoriteBar, 6)
                    }
                ),
                new UpgradeTier(5,
                    new Upgrade("FireRounds", Assets.Upgrades.Heat.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                npc.ChangeTemperature(10, proj.owner);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddIngredient(ItemID.HellstoneBar, 6)
                    },
                    new Upgrade("CryoRounds", Assets.Upgrades.Cryo.Value) {
                        Behavior = {
                            Projectile_OnHitNPCHook = (proj, npc, hit, damage) => {
                                npc.ChangeTemperature(-10, proj.owner);
                            }
                        },
                        Recipe = new UpgradeRecipe()
                                    .AddIngredient(ItemID.HallowedBar, 8)
                                    .AddIngredient(ItemID.FrostCore, 1)
                    }
                )
            );
        }
*/        public override void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            spread = MathHelper.Pi / 24f;
        }
        public override bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread)
        {
            SoundEngine.PlaySound(SoundID.Item41, player.Center);
            return true;
        }
    }
}
