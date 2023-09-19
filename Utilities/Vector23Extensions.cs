using Microsoft.Xna.Framework;

namespace deeprockitems.Utilities
{
    public static class Vector23Extensions
    {
        public static Vector3 RGBToVector3(this Vector3 color)
        {
            return color / 255;
        }
    }
}
