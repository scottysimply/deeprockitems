using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    // Token: 0x0200052B RID: 1323
    public class ColorableScrollbar : UIScrollbar
    {
        public Color ScrollbarColor { get; set; }
        public Color InnerColor { get; set; }

        // Token: 0x06003F24 RID: 16164 RVA: 0x005D798C File Offset: 0x005D5B8C
        public ColorableScrollbar() : base() {
            _texture = Assets.UI.Scrollbar;
            _innerTexture = Assets.UI.ScrollbarInner;
        }
        // Token: 0x06003F27 RID: 16167 RVA: 0x005D7A60 File Offset: 0x005D5C60
        private Rectangle GetHandleRectangle() {
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            ref float viewPosition = ref _ViewPosition(this);
            ref float viewSize = ref _ViewSize(this);
            ref float maxViewSize = ref _MaxViewSize(this);
            ref bool isDragging = ref _IsDragging(this);
            ref bool isHovering = ref _IsHoveringOverHandle(this);
            ref float yOffset = ref _DragYOffset(this);
            if (maxViewSize == 0f && viewSize == 0f)
            {
                viewSize = 1f;
                viewSize = 1f;
            }
            return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (viewPosition / maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (viewSize / maxViewSize)) + 7);
        }

        // Token: 0x06003F28 RID: 16168 RVA: 0x005D7AE8 File Offset: 0x005D5CE8
        internal void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color) {
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
        }

        // Token: 0x06003F29 RID: 16169 RVA: 0x005D7BA8 File Offset: 0x005D5DA8
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            CalculatedStyle dimensions = base.GetDimensions();
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            ref float viewPosition = ref _ViewPosition(this);
            ref float viewSize = ref _ViewSize(this);
            ref float maxViewSize = ref _MaxViewSize(this);
            ref bool isDragging = ref _IsDragging(this);
            ref bool isHovering = ref _IsHoveringOverHandle(this);
            ref float yOffset = ref _DragYOffset(this);
            if (isDragging)
            {
                float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - yOffset;
                viewPosition = MathHelper.Clamp(num / innerDimensions.Height * maxViewSize, 0f, maxViewSize - viewSize);
            }
            Rectangle handleRectangle = this.GetHandleRectangle();
            Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
            bool oldHovering = isHovering;
            isHovering = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
            if (!oldHovering && isHovering && Main.hasFocus)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            this.DrawBar(spriteBatch, this._texture.Value, dimensions.ToRectangle(), ScrollbarColor);
            this.DrawBar(spriteBatch, this._innerTexture.Value, handleRectangle, InnerColor * ((isDragging || isHovering) ? 1f : 0.85f));
        }
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_viewPosition")]
        protected extern static ref float _ViewPosition(UIScrollbar instance);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_viewSize")]
        protected extern static ref float _ViewSize(UIScrollbar instance);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_maxViewSize")]
        protected extern static ref float _MaxViewSize(UIScrollbar instance);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isDragging")]
        protected extern static ref bool _IsDragging(UIScrollbar instance);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isHoveringOverHandle")]
        protected extern static ref bool _IsHoveringOverHandle(UIScrollbar instance);
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_dragYOffset")]
        protected extern static ref float _DragYOffset(UIScrollbar instance);
        private Asset<Texture2D> _texture;
        private Asset<Texture2D> _innerTexture;
    }
}
