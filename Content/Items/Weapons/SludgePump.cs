using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
                               "Hold click to fire a shot that splatters");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.width = 100;
            Item.height = 52;
            Item.scale = 1f;
            Item.mana = 7;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;
            Item.rare = ItemRarityID.Orange;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SludgeBall>();
        }
    }
}