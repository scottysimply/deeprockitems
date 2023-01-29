using deeprockitems.Content.Items.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Humanizer.In;

namespace deeprockitems.Common.Players
{
    public class HeldItemDrawing : ModPlayer
    {
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Player.HeldItem.type == ModContent.ItemType<LaserPointer>() && Player == Main.player[Main.myPlayer])
            {
                Texture2D texture = ModContent.Request<Texture2D>("deeprockitems/Assets/Images/LaserPointerBeam").Value;
                Vector2 drawOrigin = new(texture.Width * .5f, texture.Height * .5f);
                Vector2 start = Player.Center;
                Vector2 mouse = Main.MouseWorld - start;
                Color c = Player.team switch
                {
                    1 => new(.9f, .3f, .3f, .85f), // Red
                    2 => new(.3f, .9f, .3f, .85f), // Green
                    3 => new(.4f, .8f, .8f, .8f), // Blue
                    4 => new(.8f, .8f, .25f, .8f), // Yellow
                    5 => new(.8f, .25f, .75f, .8f), // Pink
                    _ => new(.95f, .95f, .95f, .85f) // No team
                };
                for (int i = 0; i < 1596; i += 14)
                {
                    Vector2 position = start + i * Vector2.Normalize(mouse);
                    int TileX = (int)position.X / 16;
                    int TileY = (int)position.Y / 16;
                    int TileAt = Main.tile[TileX, TileY].TileType;
                    if (Main.tile[TileX, TileY].HasTile && Main.tileSolid[TileAt] && !Main.tileSolidTop[TileAt])
                    {
                        break;
                    }
                    Main.EntitySpriteDraw(texture, position - Main.screenPosition, new Rectangle(0, 0, 15, 2), c, mouse.ToRotation(), drawOrigin, 1f, SpriteEffects.None, 0);
                }
            }
        }
    }
}