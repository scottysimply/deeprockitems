using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Projectiles.CryoCannonProjectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Content.Buffs;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using deeprockitems.Common.EntitySources;

namespace deeprockitems.Content.Items.Weapons
{
    public class CryoCannon : UpgradableWeapon
    {
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
            return UpgradeBuilder.CreateUpgradeList("CryoCannon")
                .WithTier()
                    .WithUpgrade("IncreasedCooling", Assets.Upgrades.Cryo)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount -= 2f;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient([ItemID.IceBlock], 50)
                    .WithUpgrade("FartherStream", Assets.Upgrades.BigArrow)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.VelocityDecay += 0.02f;
                        })
                        .WithIngredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .WithIngredient([ItemID.SoulofNight], 6)
                .WithTier()
                    .WithUpgrade("ReducedManaCost", Assets.Upgrades.SpecialStar)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.mana -= 1;
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.FallenStar], 5)
                    .WithUpgrade("FireRate", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.useTime -= 4;
                            item.useAnimation -= 4;
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.SoulofLight], 6)
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 2;
                        })
                        .WithIngredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 2)
                .WithTier()
                    .WithUpgrade("FartherStream", Assets.Upgrades.BigArrow)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.VelocityDecay += 0.02f;
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient([ItemID.SoulofNight], 6)
                    .WithUpgrade("ReloadSpeed", Assets.Upgrades.FireRate)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            OverheatCooldown *= 0.75f;
                        })
                        .WithIngredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .WithIngredient([ItemID.ManaRegenerationPotion], 6)
                .WithTier()
                    .WithUpgrade("IncreasedCooling", Assets.Upgrades.Cryo)
                        .WithBehavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount -= 2f;
                        })
                        .WithIngredient([ItemID.HallowedBar], 8)
                        .WithIngredient([ItemID.IceBlock], 50)
                    .WithUpgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.damage += 2;
                        })
                        .WithIngredient([ItemID.HallowedBar], 8)
                        .WithIngredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    .WithUpgrade("ReducedManaCost", Assets.Upgrades.SpecialStar)
                        .WithBehavior<ItemStatChange>((Item item) => {
                            item.mana -= 1;
                        })
                        .WithIngredient([ItemID.HallowedBar], 8)
                        .WithIngredient([ItemID.FallenStar], 5)
                .WithTier()
                    .WithUpgrade("ColdRadiance", Assets.Upgrades.Cryo)
                        .WithBehavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            // Query for enemies nearby the player (5 tiles)
                            var npcs = Main.npc.Where(npc => npc.active && npc.Center.DistanceSQ(player.Center) <= 6400);
                            foreach (var npc in npcs)
                            {
                                npc.ChangeTemperature(-8, player.whoAmI);
                            }
                            return true;
                        })
                        .WithIngredient([ItemID.ChlorophyteBar], 8)
                        .WithIngredient([ItemID.InfernoPotion], 3)
                    .WithUpgrade("ReversedEntropy", Assets.Upgrades.SpecialStar)
                        .WithBehavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
                            if (target.GetTemperature() > -25) return;
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
                                    potentialNPC.ChangeTemperature(-1, projectile.owner);
                                    // Continue the chain
                                    ChainTemperature(potentialNPC);
                                }
                            };
                            // Begin the lag spikening
                            ChainTemperature(target);
                        })
                        .WithIngredient([ItemID.ChlorophyteBar], 8)
                        .WithIngredient([ItemID.FrostCore], 1)
            .Seal();
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
