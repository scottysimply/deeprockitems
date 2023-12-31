using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace deeprockitems.Utilities
{
    public static class DRGExtensions
    {
        public static int VelocityClamp_GetExtraUpdates(this Vector2 velocity, float max_speed = 16f)
        {
            // Velocity is clamped to 16f.
            if (velocity.X * velocity.X + velocity.Y * velocity.Y <= 256f)
            {
                return 0;
            }
            Main.NewText("Velocity is too great! Clamping");

            int resultX = Math.Abs((int)Math.Round(velocity.X / 16f, MidpointRounding.AwayFromZero));
            Main.NewText($"Min needed extra updates in X direction is {resultX}");
            int resultY = Math.Abs((int)Math.Round(velocity.Y / 16f, MidpointRounding.AwayFromZero));
            Main.NewText($"Min needed extra updates in Y direction is {resultY}");
            return resultX >= resultY ? resultX : resultY;
        }
        public static Vector3 RGBToVector3(this Vector3 color)
        {
            return color / 255;
        }
    }
}
