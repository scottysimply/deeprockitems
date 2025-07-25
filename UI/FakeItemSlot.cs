using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.Audio;
using Terraria.ID;
using System;
using Humanizer;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using ReLogic.Content;

namespace deeprockitems.UI
{
    public class FakeItemSlot : UIElement
    {
        public Func<Item> GetItemToTrackInstead { get; set; }
        public delegate void ItemSetter(ref Item item);
        public ItemSetter SetItemToTrackInstead { get; set; }
        public Asset<Texture2D> BackgroundTexture { get; set; } = TextureAssets.InventoryBack9;
        public Asset<Texture2D>? BorderTexture { get; set; } = null;
        public Color BackgroundColor { get; set; } = Main.inventoryBack;
        public Color BorderColor { get; set; } = Color.Black;
        public int Context { get; set; } = 1;
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
            base.OnInitialize();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dimensions = GetDimensions().ToRectangle();
            float inventoryScale = Main.inventoryScale * 1.416667f;

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
            if (BorderTexture is not null)
            {
                spriteBatch.Draw(BorderTexture.Value, dimensions, BackgroundColor);
            }
            if (BackgroundTexture is not null)
            {
                spriteBatch.Draw(BackgroundTexture.Value, dimensions, BackgroundColor);
            }
            Vector2 scaledCenterOffset = dimensions.Size();

            if (ItemInSlot.type > ItemID.None && ItemInSlot.stack > 0)
            {
                int type = ItemInSlot.type;
                Color color = Color.White;
                Main.instance.LoadItem(type);
                Texture2D itemTexture = TextureAssets.Item[type].Value;
                Rectangle itemFrame = ((Main.itemAnimations[type] == null) ? itemTexture.Frame() : Main.itemAnimations[type].GetFrame(itemTexture));
                ItemSlot.DrawItem_GetColorAndScale(ItemInSlot, inventoryScale, ref color, 40f / 52f * dimensions.Width, ref itemFrame, out var itemLight, out var finalDrawScale);
                spriteBatch.Draw(itemTexture, dimensions.TopLeft() + scaledCenterOffset / 2f, itemFrame, ItemInSlot.GetAlpha(itemLight), 0f, itemFrame.Size() / 2f, finalDrawScale, SpriteEffects.None, 0f);
            }
        }
    }
}
