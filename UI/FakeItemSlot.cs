using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.Audio;
using Terraria.ID;
using System;

namespace deeprockitems.UI
{
    public class FakeItemSlot : UIElement
    {
        public Func<Item> GetItemToTrackInstead { get; set; }
        public delegate void ItemSetter(ref Item item);
        public ItemSetter SetItemToTrackInstead { get; set; }
        private Item _itemInSlot;
        internal Item ItemInSlot
        {
            get
            {
                if (GetItemToTrackInstead != null && SetItemToTrackInstead != null)
                {
                    return GetItemToTrackInstead();
                }
                return _itemInSlot;
            }
            set
            {
                if (GetItemToTrackInstead != null && SetItemToTrackInstead != null)
                {
                    SetItemToTrackInstead(ref value);
                    return;
                }
                _itemInSlot = value;
            }
        }
        public ItemPredicate PredicateToPutItemIn;
        private float _drawScale = 1f;
        public delegate bool ItemPredicate(Item mouseItem, Item inSlot);
        public FakeItemSlot(ItemPredicate canItemBePutInSlot)
        {
            PredicateToPutItemIn = canItemBePutInSlot;
            ItemInSlot = new(0);
            OnLeftClick += FakeItemSlot_OnLeftClick;
        }
        public event ItemSwapHandler OnItemSwap;
        public delegate void ItemSwapHandler(Item itemNowInSlot, Item itemThatLeftSlot);
        private void FakeItemSlot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (ItemSlot.ShiftInUse && !ItemSlot.ShiftForcedOn && ItemInSlot.type != 0)
            {
                // Find first empty slot in inventory
                for (int i = 49; i >= 0; i--)
                {
                    // Put in empty slot if it can be put in
                    if (Main.LocalPlayer.inventory[i].type == 0)
                    {
                        Item tempItem = ItemInSlot;
                        SwapItems(ref Main.LocalPlayer.inventory[i], ref tempItem);
                        ItemInSlot = tempItem;
                        break;
                    }
                }
                return;
            }
            if (PredicateToPutItemIn(Main.mouseItem, ItemInSlot))
            {
                Item tempItem = ItemInSlot;
                SwapItems(ref Main.mouseItem, ref tempItem);
                ItemInSlot = tempItem;
            }
        }
        public void SwapItems(ref Item itemGoingToSlot, ref Item itemLeavingSlot)
        {
            // Send event
            OnItemSwap?.Invoke(itemGoingToSlot, itemLeavingSlot);
            // Play sound
            SoundEngine.PlaySound(SoundID.Grab);
            // Swap the items.
            (itemGoingToSlot, itemLeavingSlot) = (itemLeavingSlot, itemGoingToSlot);
        }
        public override void OnInitialize()
        {
            _drawScale = Width.Pixels / 52f;
            base.OnInitialize();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dimensions = GetDimensions().ToRectangle();
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _drawScale;

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                // Set hovering information
                Main.LocalPlayer.mouseInterface = true;
                Main.HoverItem = ItemInSlot.Clone();
                Main.hoverItemName = ItemInSlot.HoverName;
                if (ItemSlot.ShiftInUse && !ItemSlot.ShiftForcedOn && ItemInSlot.type != 0)
                {
                    Main.cursorOverride = 8;
                }
            }
            Item tempItem = ItemInSlot;
            ItemSlot.Draw(spriteBatch, ref tempItem, 1, dimensions.TopLeft());

            // Reset scale
            Main.inventoryScale = oldScale;
        }
    }
}
