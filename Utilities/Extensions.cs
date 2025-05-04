using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace deeprockitems.Utilities
{
    public static class Extensions
    {
        public static Vector3 RGBToVector3(this Vector3 color)
        {
            return color / 255;
        }
    }
}
