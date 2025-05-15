using deeprockitems.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace deeprockitems.UI
{
    /// <summary>
    /// Defines an element that grows or shrinks when hovered over.
    /// </summary>
    public abstract class TweenableElement : UIElement
    {
        protected float currentScale = 1f;
        public TweenableElement() {
            currentScale = UnhoveredScale;
        }
        public virtual float UnhoveredScale { get => 0.8f; }
        public virtual float HoveredScale { get => 1.0f; }
        public bool AllowedToTween { get; set; } = true;
        private Rectangle _dimensions => GetDimensions().ToRectangle();
        public RectangleF ScaledDimensions => new RectangleF(_dimensions.Center.X - 0.5f * currentScale * _dimensions.Width, _dimensions.Center.Y - 0.5f * currentScale * _dimensions.Height, currentScale * _dimensions.Width, currentScale * _dimensions.Height);
        public new bool IsMouseHovering { get => ScaledDimensions.Contains(Main.MouseScreen); }
        public sealed override void Update(GameTime gameTime) {
            UpdateHook(gameTime);
        }
        public virtual void UpdateHook(GameTime gameTime) {

        }
        public sealed override void Draw(SpriteBatch spriteBatch) {
            HandleTweening();
            DrawHook(spriteBatch);
        }
        /// <summary>
        /// When drawing anything, use ScaledDimensions rather than GetDimensions().ToRectangle().
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void DrawHook(SpriteBatch spriteBatch) {

        }
        protected void HandleTweening() {
            // Handle blocked tweens
            if (!AllowedToTween)
            {
                AllowedToTween = true;
                return;
            }
            if (IsMouseHovering)
            {
                if (currentScale < HoveredScale)
                {
                    currentScale += 0.03f;
                }
                else if (HoveredScale > HoveredScale)
                {
                    currentScale = HoveredScale;
                }
            }
            else
            {
                if (UnhoveredScale < currentScale)
                {
                    currentScale -= 0.05f;
                }
                else if (UnhoveredScale > currentScale)
                {
                    currentScale = UnhoveredScale;
                }
            }
        }
    }
}
