using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace deeprockitems.UI.CooldownUI
{
    public class CooldownMeter : UIElement, ILoadable
    {
        #region Initialize/dispose of basicEffect
        // BasicEffect is a shader that is required to draw primitives
        static BasicEffect basicEffect;
        void ILoadable.Load(Mod mod) {
            // Initialize the basic effect
            Main.RunOnMainThread(() => {
                basicEffect = new(Main.instance.GraphicsDevice);
                basicEffect.VertexColorEnabled = true;
                basicEffect.TextureEnabled = true;
            });
        }
        void ILoadable.Unload() {
            // Dispose of the basicEffect
            Main.RunOnMainThread(() => {
                basicEffect?.Dispose();
                basicEffect = null;
            });
        }
        #endregion
#nullable enable
        private static UpgradableWeapon? PlayerWeapon {
            get
            {
                try
                {
                    return Main.LocalPlayer.HeldItem.ModItem as UpgradableWeapon;
                }
                catch
                {
                    return null;
                }
            }
        }
#nullable disable
        public CooldownMeter() {
            // Set width and height
            Width.Pixels = 32f;
            Height.Pixels = 32f;
        }
        public float Angle = 0f;
        public override void Draw(SpriteBatch spriteBatch) {            
            // Only draw if the player weapon is not null, and the cooldown is greater than 0
            if (PlayerWeapon is null || PlayerWeapon.OverheatCooldown <= 0) {
                return;
            }
            // We only have the simple meter right now
            Rectangle dimensions = GetDimensions().ToRectangle();
            DrawSimpleMeter(spriteBatch, new Vector2(dimensions.Center.X, dimensions.Center.Y + 60f), PlayerWeapon.OverheatCooldown / UpgradableWeapon.COOLDOWN_THRESHOLD, CooldownDrawColor);

        }
        public Color CooldownDrawColor => PlayerWeapon.IsWeaponEnabledByCooldown ? Color.White : Color.IndianRed;
        private void DrawSimpleMeter(SpriteBatch spriteBatch, Vector2 center, float percentageFilled, Color drawColor) {
            // Adjust draw center
            center.Y -= 20;

            // Get real drawing position
            Vector2 adjustedDrawPos = new Vector2(center.X, center.Y) - 0.5f * TextureAssets.Hb1.Size();
            int filledMeterWidth = (int)(percentageFilled * 36f);
            if (filledMeterWidth < 3)
            {
                filledMeterWidth = 3;
            }

            // If the meter is not full
            if (filledMeterWidth < 34)
            {
                // Draw background of meter
                if (filledMeterWidth < 36)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth, adjustedDrawPos.Y), (Rectangle?)new Rectangle(2, 0, 2, TextureAssets.Hb2.Height()), drawColor);
                }
                // Draw outline of right edge of meter
                if (filledMeterWidth < 34)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth + 2, adjustedDrawPos.Y), (Rectangle?)new Rectangle(filledMeterWidth + 2, 0, 36 - filledMeterWidth - 2, TextureAssets.Hb2.Height()), drawColor);
                }
                // Draw left edge of meter
                if (filledMeterWidth > 2)
                {
                    spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X, adjustedDrawPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth - 2, TextureAssets.Hb1.Height()), drawColor);
                }
                // The actual filled meter
                spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth - 2, adjustedDrawPos.Y), (Rectangle?)new Rectangle(32, 0, 2, TextureAssets.Hb1.Height()), drawColor);
            }
            else // Close to maximum temperature
            {
                // Draw meter background
                if (filledMeterWidth < 36)
                {
                    Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb2.Value, new Vector2(adjustedDrawPos.X + filledMeterWidth, adjustedDrawPos.Y), (Rectangle?)new Rectangle(filledMeterWidth, 0, 36 - filledMeterWidth, TextureAssets.Hb2.Height()), drawColor));
                }
                // Draw filled meter
                Main.EntitySpriteDraw(new DrawData(TextureAssets.Hb1.Value, new Vector2(adjustedDrawPos.X, adjustedDrawPos.Y), (Rectangle?)new Rectangle(0, 0, filledMeterWidth, TextureAssets.Hb1.Height()), drawColor));
            }
        }
        private Vector3[] GetVerticesOfPolygon(Vector2 center, int numberOfSides, float radius, float rotationOffset = 0f) {
            if (numberOfSides < 3)
            {
                throw new Exception("Polygons cannot have less than 3 sides!");
            }
            // First get the internal angle in radians
            //float internalAngle = (numberOfSides - 2) * MathHelper.Pi / numberOfSides;
            float angleToRotateBy = 2 * MathHelper.Pi / numberOfSides;
            // Calculate side length
            float sideLength = (float)Math.Sqrt(2 * radius * radius * (1 - Math.Cos(2 * MathHelper.Pi / numberOfSides)));
            Vector3 originalPoint = new Vector3(center.X, center.Y - radius, 0f);
            // Now we need to dynamically create the vertices
            // Add the center as the first index
            List<Vector3> vertexPositions = [new Vector3(center.X, center.Y, 0f)];
            // Dynamically create vertices based on rotation and iteration
            for (int i = 0; i < numberOfSides; i++)
            {
                // Rotated X coordinate
                float rotatedX = (int)(center.X + (originalPoint.X - center.X) * Math.Cos(i * angleToRotateBy + rotationOffset) - (originalPoint.Y - center.Y) * Math.Sin(i * angleToRotateBy + rotationOffset));
                // Rotated Y coordinate
                float rotatedY = (int)(center.Y + (originalPoint.X - center.X) * Math.Sin(i * angleToRotateBy + rotationOffset) - (originalPoint.Y - center.Y) * Math.Cos(i * angleToRotateBy + rotationOffset));
                // Add the vector to the vertex positions list
                vertexPositions.Add(new Vector3(rotatedX, rotatedY, 0f));
            }
            // Return the vertices
            return [.. vertexPositions];
        }
    }
}
