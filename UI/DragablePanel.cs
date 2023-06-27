using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace deeprockitems.UI
{
    public class DragablePanel : UIPanel
    {
        private Vector2 offset;
        public bool dragging;
        public override void OnInitialize()
        {
            
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            DragStart(evt);
        }
        public void DragStart(UIMouseEvent evt)
        {
            offset = new(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            DragEnd(evt);
        }
        public void DragEnd(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // This stays for some reason

            // Make sure that clicking on the UI doesn't interact with the world
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // If the mouse panel manages to get outside the bounds of the screen, put it back inside the screen bounds
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }
    }
}
