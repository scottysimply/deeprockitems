using deeprockitems.Common.EntitySources;
using deeprockitems.Common.NPCs;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.ZhukovProjectiles;
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
                .WithOverclock("StaticBlast", Assets.Upgrades.Electricity, Overclock.OverclockType.Balanced)
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
