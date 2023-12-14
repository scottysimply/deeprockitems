using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using System;

namespace deeprockitems.Common.PlayerLayers
{
    public class ZhukovsFrontLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.SolarShield, PlayerDrawLayers.ArmOverItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (!drawInfo.drawPlayer.ItemAnimationActive) return;
            if (drawInfo.drawPlayer.JustDroppedAnItem) return;
            if (drawInfo.shadow != 0f) return;

            // Set Draw Info defaults
            drawInfo.heldItem = drawInfo.drawPlayer.HeldItem;
            drawInfo.itemColor = Lighting.GetColor((int)(drawInfo.Position.X + drawInfo.drawPlayer.width * 0.5) / 16, (int)((drawInfo.Position.Y + drawInfo.drawPlayer.height * 0.5) / 16.0));
            float adjustedItemScale = drawInfo.drawPlayer.GetAdjustedItemScale(drawInfo.heldItem);

            // Draw.
            DrawData item;
            if (drawInfo.heldItem.type == ModContent.ItemType<Zhukovs>())
            {
                Texture2D zhukovsSprite;
                try
                {
                    zhukovsSprite = (Texture2D)ModContent.Request<Texture2D>("deeprockitems/Content/Items/Weapons/ZhukovsHeld");
                }
                catch
                {
                    Mod drg = ModContent.GetInstance<deeprockitems>();
                    drg.Logger.Warn("Unable to retrieve Zhukovs' held item texture. Zhukovs will draw like a regular item.");
                    return;
                }
                Vector2 textureCenter = new Vector2((int)(drawInfo.heldItem.width / 2f), (int)(drawInfo.heldItem.height / 2f));
                Vector2 drawOffset = new(2f, (int)(zhukovsSprite.Height / 2f) + (int)(drawInfo.drawPlayer.gravDir * -4f));

                int drawOffX = (int)drawOffset.X;
                textureCenter.Y = drawOffset.Y;
                Vector2 drawOrigin = new Vector2((float)-(float)drawOffX, zhukovsSprite.Height / 2f);
                if (drawInfo.drawPlayer.direction == -1)
                {
                    drawOrigin = new Vector2(zhukovsSprite.Width + drawOffX, zhukovsSprite.Height / 2f);
                }

                // Mainhand in the front.
                item = new DrawData(zhukovsSprite, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + textureCenter.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + textureCenter.Y)), zhukovsSprite.Bounds, drawInfo.heldItem.GetAlpha(drawInfo.itemColor), drawInfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale, drawInfo.itemEffect, 0f);
                drawInfo.DrawDataCache.Add(item);
                return;
            }
        }
    }
}
