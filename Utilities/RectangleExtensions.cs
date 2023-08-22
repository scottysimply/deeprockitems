using Microsoft.Xna.Framework;

namespace deeprockitems.Utilities
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Returns true if any part of rect1 is outside the parent rectangle
        /// </summary>
        /// <param name="parentRect"></param>
        /// <param name="rect1"></param>
        /// <returns></returns>
        public static bool Outside(this Rectangle parentRect, Rectangle rect1)
        {
            if (parentRect.Left > rect1.Left)
            {
                return true;
            }
            if (parentRect.Right < rect1.Right)
            {
                return true;
            }
            if (parentRect.Top > rect1.Left)
            {
                return true;
            }
            if (parentRect.Bottom < rect1.Bottom)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true only if rect1 is fully enclosed by the parent rectangle
        /// </summary>
        /// <param name="parentRect"></param>
        /// <param name="rect1"></param>
        /// <returns></returns>
        public static bool Inside(this Rectangle parentRect, Rectangle rect1)
        {
            if (parentRect.Left > rect1.Left)
            {
                return false;
            }
            if (parentRect.Right < rect1.Right)
            {
                return false;
            }
            if (parentRect.Top > rect1.Left)
            {
                return false;
            }
            if (parentRect.Bottom < rect1.Bottom)
            {
                return false;
            }
            return true;
        }
    }
}
