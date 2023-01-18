using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using static deeprockitems.MyFunctions;
using static System.Math;
using Terraria.DataStructures;
using static Humanizer.In;

namespace deeprockitems.Content.Items.Misc
{
    public class LaserPointer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Pointer");
            Tooltip.SetDefault("'Did you borrow my underwear?'\n" +
                               "Puts a marker on whatever you ping");
        }
        public override void SetDefaults()
        {
            Item.scale = 1.4f;
            Item.holdStyle = 1;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.PurificationPowder;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<Projectiles.LaserPointerBeam>();
        }
        public override void HoldItem(Player player)
        {
            Vector2 mouseRelative = Main.MouseWorld - player.position;
            if (Main.MouseWorld.X < player.Center.X) // cursor is to the left of the player
            {
                player.direction = -1; // make player face left
                if (Main.MouseWorld.Y < player.Center.Y) // Mouse is above the player
                {
                    player.itemRotation = mouseRelative.ToRotation() + (float)PI;
                }
                else
                {
                    player.itemRotation = mouseRelative.ToRotation() - (float)PI;
                }
            }
            else // cursor is to the right of the player
            {
                player.direction = 1; // make player face right
                player.itemRotation = mouseRelative.ToRotation();
            }
        }
        public override void HoldItemFrame(Player player)
        {
            Vector2 mouseRelative = Main.MouseWorld - player.position;
            if (Main.MouseWorld.X < player.Center.X) // cursor is to the left of the player
            {
                if (Main.MouseWorld.Y < player.Center.Y) // Mouse is above the player
                {
                    player.itemRotation = mouseRelative.ToRotation() + (float)PI;
                }
                else
                {
                    player.itemRotation = mouseRelative.ToRotation() - (float)PI;
                }
            }
            else // cursor is to the right of the player
            {
                player.itemRotation = mouseRelative.ToRotation();
            }
        }
    }
}