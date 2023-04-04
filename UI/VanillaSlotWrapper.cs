using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Formats.Asn1.AsnWriter;

namespace deeprockitems.UI
{
    // This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState. 
    // By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
    // ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn. 
    // If you want more control, you might need to write your own UIElement.
    // I've added basic functionality for validating the item attempting to be placed in the slot via the validItem Func. 
    // See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
    /*internal class CustomItemSlot : UIElement
    {
        internal Item item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public CustomItemSlot(int context = ItemSlot.Context.Count, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            item = new Item();
            item.SetDefaults(0);

            Width.Set(Main.inventoryScale * scale, 0f);
            Height.Set(Main.inventoryScale * scale, 0f);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    ItemSlot.Handle(ref item, _context);
                }
            }
            ItemSlot.Draw(spriteBatch, ref item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
    }*/
    public class CustomItemSlot : UIElement
    {
        private Item item;
        private int AppropriateItem;

        public CustomItemSlot(Item )
    }
}