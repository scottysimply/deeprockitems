using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using deeprockitems.Content.Projectiles;

namespace deeprockitems.Content.Items.Weapons
{
    public class SludgePump : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sludge Pump");
            Tooltip.SetDefault("Fires in a slow moving arc \n" +
                               "Hold click to fire a shot that splatters \n" +
                               "Splattered shots use 50% extra mana");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.width = 100;
            Item.height = 52;
            Item.scale = .85f;
            Item.mana = 7;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.rare = ItemRarityID.Orange;
            Item.value = 300000;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SludgeHelper>();
        }
    }
}