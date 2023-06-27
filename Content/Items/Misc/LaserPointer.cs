using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using static System.Math;
using Terraria.DataStructures;
using static Humanizer.In;
using deeprockitems.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Content.Items.Misc
{
    public class LaserPointer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.scale = 1.4f;
            Item.holdStyle = 1;
            Item.useStyle = 5;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.shootSpeed = 2;
            Item.value = 60000;
            Item.shoot = ProjectileID.PurificationPowder;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<LaserPointerPing>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.type == ModContent.ProjectileType<LaserPointerPing>() && Main.player[proj.owner] == player)
                {
                    proj.Kill();
                }
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (Main.MouseWorld.X < player.Center.X) // cursor is to the left of the player
            {
                player.direction = -1; // make player face left
            }
            else // cursor is to the right of the player
            {
                player.direction = 1; // make player face right
            }
        }
        public override void HoldItemFrame(Player player)
        {
            // Make the held item aim toward the mouse
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
    }
}