using Microsoft.Xna.Framework;
using System;

namespace deeprockitems.Types
{
    public struct RectangleF
    {
        public RectangleF(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public RectangleF(int x, int y, int width, int height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Left { get => X; set => X = value; }
        public float Top { get => Y; set => Y = value; }
        public float Right { get => X + Width; set => X = value - Width; }
        public float Bottom { get => Y + Height; set => Y = value - Height; }
        public Vector2 TopLeft {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }   
        }
        public Vector2 TopRight
        {
            get => new Vector2(X + Width, Y);
            set
            {
                X = value.X - Width;
                Y = value.Y;
            }
        }
        public Vector2 BottomLeft
        {
            get => new Vector2(X, Y + Height);
            set
            {
                X = value.X;
                Y = value.Y - Height;
            }
        }
        public Vector2 BottomRight
        {
            get => new Vector2(X + Width, Y + Height);
            set
            {
                X = value.X - Width;
                Y = value.Y - Height;
            }
        }
        public Vector2 Center {
            readonly get => new(X + 0.5f * Width, Y + 0.5f * Height);
            set
            {
                X = value.X - 0.5f * Width;
                Y = value.Y - 0.5f * Height;
            }
        }
        public static explicit operator Rectangle(RectangleF rect) {
            // Rounding to prevent floating point inconsistencies
            return new((int)MathF.Round(rect.X), (int)Math.Round(rect.Y), (int)Math.Round(rect.Width), (int)Math.Round(rect.Height));
        }
        public static implicit operator RectangleF(Rectangle rect) {
            return new(rect.X, rect.Y, rect.Width, rect.Height);
        }
        /// <summary>
        /// Scales a rectangle while keeping the center constant.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public RectangleF Scale(float scale) {
            return new RectangleF(this.Center.X - 0.5f * Width * scale, this.Center.Y - 0.5f * Height * scale, Width * scale, Height * scale);
        }
        /// <summary>
        /// Returns true if the vector is inside the rectangle.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool Contains(Vector2 vector) {
            return X < vector.X && Y < vector.Y &&
                   vector.X < X + Width && vector.Y < Y + Height;
        }
        /// <summary>
        /// Returns true if the point is inside the rectangle.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point) {
            return X < point.X && Y < point.Y &&
                   point.X < X + Width && point.Y < Y + Height;
        }
        public override string ToString() {
            return $"RectangleF {{X: {X}, Y: {Y}, Width: {Width}, Height: {Height}}}";
        }
    }
}
