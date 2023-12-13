using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using System;
using deeprockitems.Common.PlayerLayers;

namespace deeprockitems.Common.Weapons
{
    public class ZhukovsRearLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.OffhandAcc, PlayerDrawLayers.Torso);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (!drawInfo.drawPlayer.ItemAnimationActive) return;
            if (drawInfo.drawPlayer.JustDroppedAnItem) return;
            if (drawInfo.shadow != 0f) return;

            // Set Draw Info defaults
            drawInfo.heldItem = drawInfo.drawPlayer.HeldItem;
            drawInfo.itemColor = Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawInfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawInfo.drawPlayer.height * 0.5) / 16.0));
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
                Vector2 textureCenter = new Vector2((int)(zhukovsSprite.Width / 2f), (int)(zhukovsSprite.Height / 2f));
                Vector2 drawOffset = new(2f, (int)(zhukovsSprite.Height / 2f) + (int)(drawInfo.drawPlayer.gravDir * -4f));

                int drawOffX = (int)drawOffset.X;
                textureCenter.Y = drawOffset.Y;
                Vector2 drawOrigin = new Vector2((float)(-(float)drawOffX), zhukovsSprite.Height / 2f);
                if (drawInfo.drawPlayer.direction == -1)
                {
                    drawOrigin = new Vector2((float)(zhukovsSprite.Width + drawOffX), zhukovsSprite.Height / 2f);
                }

                DualWieldPlayer modPlayer = drawInfo.drawPlayer.GetModPlayer<DualWieldPlayer>();

                float offhandScale = 0.8f;
                // Offhand first since it's in the back.
                item = new DrawData(zhukovsSprite, new Vector2((int)(modPlayer.OffHandItemLocation.X - Main.screenPosition.X + textureCenter.X), (int)(modPlayer.OffHandItemLocation.Y - Main.screenPosition.Y + textureCenter.Y)), zhukovsSprite.Bounds, drawInfo.heldItem.GetAlpha(new(drawInfo.itemColor.ToVector3() * 0.75f)), drawInfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale * offhandScale, drawInfo.itemEffect, 0f);
                drawInfo.DrawDataCache.Add(item);
                return;
            }
        }
    }
}
