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
            Item.useAnimation = Item.useTime = 8;
            Item.noMelee = true;
            Item.shootSpeed = 16f;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            this.ShotsUntilCooldown = 20f;
            this.TimeToEndCooldown = 140f;
        }
        public override UpgradeList InitializeUpgrades() {
            return UpgradeBuilder.CreateUpgradeList("CryoCannon")
                .Tier()
                    .Upgrade("IncreasedCooling", Assets.Upgrades.Cryo)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount -= 2f;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .Ingredient([ItemID.IceBlock], 50)
                    .Upgrade("FartherStream", Assets.Upgrades.BigArrow)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.VelocityDecay += 0.01f;
                        })
                        .Ingredient([ItemID.CobaltBar, ItemID.PalladiumBar], 8)
                        .Ingredient([ItemID.SoulofNight], 6)
                .Tier()
                    .Upgrade("ReducedManaCost", Assets.Upgrades.SpecialStar)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.mana -= 1;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient([ItemID.FallenStar], 5)
                    .Upgrade("FireRate", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.useTime -= 3;
                            item.useAnimation -= 3;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient([ItemID.SoulofLight], 6)
                    .Upgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 2;
                        })
                        .Ingredient([ItemID.MythrilBar, ItemID.OrichalcumBar], 8)
                        .Ingredient([ItemID.RagePotion, ItemID.WrathPotion], 2)
                .Tier()
                    .Upgrade("FartherStream", Assets.Upgrades.BigArrow)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.VelocityDecay += 0.01f;
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .Ingredient([ItemID.SoulofNight], 6)
                    .Upgrade("ReloadSpeed", Assets.Upgrades.FireRate)
                        .Behavior<ItemStatChange>((Item item) => {
                            OverheatCooldown *= 0.75f;
                        })
                        .Ingredient([ItemID.AdamantiteBar, ItemID.TitaniumBar], 8)
                        .Ingredient([ItemID.ManaRegenerationPotion], 6)
                .Tier()
                    .Upgrade("IncreasedCooling", Assets.Upgrades.Cryo)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount -= 2f;
                        })
                        .Ingredient([ItemID.HallowedBar], 8)
                        .Ingredient([ItemID.IceBlock], 50)
                    .Upgrade("DamageUpgrade", Assets.Upgrades.Damage)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage += 2;
                        })
                        .Ingredient([ItemID.HallowedBar], 8)
                        .Ingredient([ItemID.RagePotion, ItemID.WrathPotion], 3)
                    .Upgrade("ReducedManaCost", Assets.Upgrades.SpecialStar)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.mana -= 1;
                        })
                        .Ingredient([ItemID.HallowedBar], 8)
                        .Ingredient([ItemID.FallenStar], 5)
                .Tier()
                    .Upgrade("ColdRadiance", Assets.Upgrades.Cryo)
                        .Behavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            // Query for enemies nearby the player (5 tiles)
                            var npcs = Main.npc.Where(npc => npc.active && npc.Center.DistanceSQ(player.Center) <= 6400);
                            foreach (var npc in npcs)
                            {
                                npc.ChangeTemperature(-12, player.whoAmI);
                            }
                            return true;
                        })
                        .Ingredient([ItemID.ChlorophyteBar], 8)
                        .Ingredient([ItemID.InfernoPotion], 3)
                    .Upgrade("ReversedEntropy", Assets.Upgrades.SpecialStar)
                        .Behavior<ProjectileOnHitNPC>((Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => {
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
                        .Ingredient([ItemID.ChlorophyteBar], 8)
                        .Ingredient([ItemID.FrostCore], 1)
                    .Overclock("Snowball", Assets.Upgrades.Cryo, Overclock.OverclockType.Clean)
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount *= 1.33f;
                        })
                    .Overclock("IceSpear", Assets.Upgrades.BigArrow, Overclock.OverclockType.Unstable)
                        .Behavior<ItemAltFunctionUse>((Item item, Player player) => {
                            return true;
                        })
                        .Behavior<ItemOnShoot>((Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => {
                            if (player.altFunctionUse != 2) return true;
                            Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<IceSpear>(), 250, knockback, Owner: player.whoAmI);
                            (item.ModItem as CryoCannon).AddCooldownOnShoot(player, 999f);
                            return false;
                        })
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount *= 0.8f;
                        })
                    .Overclock("IceStorm", Assets.Upgrades.Damage, Overclock.OverclockType.Unstable)
                        .Behavior<ItemStatChange>((Item item) => {
                            item.damage *= 4;
                        })
                        .Behavior<ProjectileOnSpawn>((Projectile projectile, IEntitySource source) => {
                            if (projectile.ModProjectile is not CryoProjectile cryo) return;
                            cryo.CoolingAmount *= 0.25f;
                        })
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
