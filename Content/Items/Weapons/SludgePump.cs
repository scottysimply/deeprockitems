using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Utilities;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Items.Weapons
{
    public class SludgePump : UpgradeableItemTemplate
    {
        public int TIMER = 0;
        public static int MAX_TIMER = 30;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SafeDefaults()
        {
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
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.rare = ItemRarityID.Orange;
            Item.value = 200000;
            ValidUpgrades.Add(ModContent.ItemType<AntiGravOC>());
            ValidUpgrades.Add(ModContent.ItemType<SludgeExplosionOC>());
            ValidUpgrades.Add(ModContent.ItemType<GooSpecialOC>());

            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Add(ModContent.ItemType<TracerRounds>());

        }
        public override void HoldItem(Player player)
        {
            if (player == Main.LocalPlayer && player.HeldItem.type == ModContent.ItemType<TracerRounds>())
            {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed, ModContent.ProjectileType<ProjectileTracer>(), 0, 0, ai0: TIMER, ai1: Overclock);
            }
            TIMER++;
            if (TIMER > MAX_TIMER)
            {
                TIMER = 0;
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SludgeHelper>();
        }
        public override void IndividualUpgrades()
        {
            if (Upgrades.Contains(ModContent.ItemType<AntiGravOC>()))
            {
                CurrentOverclock = "AG Mixture";
                OverclockPositives = "▶Shots are no longer affected by gravity";
                OverclockNegatives = "";
            }
            else if (Upgrades.Contains(ModContent.ItemType<SludgeExplosionOC>()))
            {
                CurrentOverclock = "Waste Ordnance";
                OverclockPositives = "▶Charge shots explode with a large range";
                OverclockNegatives = "▶Charge shots don't fragment when destroyed";
            }
            else if (Upgrades.Contains(ModContent.ItemType<GooSpecialOC>()))
            {
                CurrentOverclock = "Goo Bomber Special";
                OverclockPositives = "▶Charge shots leave trails behind";
                OverclockNegatives = "▶Charge shots don't fragment when destroyed\n" +
                                     "▶Normal shots deal less damage";
            }
        }
    }
}