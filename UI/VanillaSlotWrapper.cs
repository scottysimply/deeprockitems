using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent;
using static System.Formats.Asn1.AsnWriter;
using Terraria.Audio;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.GameContent.UI.Elements;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using System.Linq;
using deeprockitems.UI.UpgradeItem;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace deeprockitems.UI
{
    public class DisplaySlot : UIElement
    {
        protected int _slotID = -1;
        public DisplaySlot(int ID)
        {
            _slotID = ID;
        }

        public bool Clicked { get; private set; } = false;
        public Item ItemToDisplay { get; set; } = new Item();
        protected override void DrawSelf(SpriteBatch spriteBatch) // TODO: Clean up the drawing code. Items in the slots are still roughly 5% bigger than they should be. It's pretty good for now, though :)
        {
            float _InventoryScale = Main.inventoryScale;
            Texture2D SlotTexture = TextureAssets.InventoryBack.Value;
            Rectangle SlotDimensions = GetDimensions().ToRectangle();

            Color color = Color.White;

            spriteBatch.Draw(SlotTexture, SlotDimensions, color);

            if (ItemToDisplay.type > 0 && ItemToDisplay.stack > 0)
            {
                Main.instance.LoadItem(ItemToDisplay.type);
                Texture2D ItemInSlot_Texture = TextureAssets.Item[ItemToDisplay.type].Value;
                Rectangle ItemInSlot_Dimensions = ((Main.itemAnimations[ItemToDisplay.type] == null) ? ItemInSlot_Texture.Frame() : Main.itemAnimations[ItemToDisplay.type].GetFrame(ItemInSlot_Texture));

                float DrawScale = 1f;
                Vector2 DrawPosition;

                // If item is too big to fit in the slot:
                if (ItemInSlot_Dimensions.Width > 32|| ItemInSlot_Dimensions.Height > 32)
                {
                    DrawScale = ItemInSlot_Dimensions.Width <= ItemInSlot_Dimensions.Height ? 32f / ItemInSlot_Dimensions.Height : 32f / ItemInSlot_Dimensions.Width;
                }
                Vector2 slot_center = SlotDimensions.Center();
                Vector2 ScaledSize = DrawScale * ItemInSlot_Texture.Size();
                DrawPosition = slot_center - .5f * ScaledSize;
                Rectangle dest = new Rectangle((int)Math.Round(DrawPosition.X), (int)Math.Round(DrawPosition.Y), (int)Math.Round(ScaledSize.X), (int)Math.Round(ScaledSize.Y));
                // spriteBatch.Draw(TextureAssets.MagicPixel.Value, dest, Color.White);
                spriteBatch.Draw(ItemInSlot_Texture, dest, ItemInSlot_Dimensions, Color.White, 0, new(0, 0), SpriteEffects.None, 0f);
            }
            else
            {
                DrawEmpty(spriteBatch);
            }

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.HoverItem = ItemToDisplay.Clone();
                Main.hoverItemName = ItemToDisplay.Name;
                MouseHoverCursor();
            }
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            Clicked = true;
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            Clicked = false;
            base.LeftMouseUp(evt);
        }
        /// <summary>
        /// Used for drawing the empty slot
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void DrawEmpty(SpriteBatch spriteBatch) { }
        public virtual void MouseHoverCursor() { }
    }
    public class UpgradeSlot : DisplaySlot
    {
        public UpgradeSlot(int ID) : base(ID)
        {
            _slotID = ID;
        }
        public override void MouseHoverCursor()
        {
            if (ItemSlot.ShiftInUse && ItemToDisplay.type != 0)
            {
                for (int i = 49; i > 0; i--)
                {
                    if (Main.player[Main.myPlayer].inventory[i].type == 0)
                    {
                        Main.cursorOverride = 30;
                    }
                }
            }
        }
        /// <summary>
        /// Checks to see if this upgrade is already equipped. Returns true if the upgrade process *should continue*, not if the upgrade is equipped
        /// </summary>
        /// <param name="item1"></param>
        /// <returns>Whether the process of swapping the items should continue</returns>
        private bool CheckDuplicates(Item item1)
        {
            if (UpgradeUISystem.UpgradeUIPanel.GetUpgrades().Contains(item1.type))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Gets whether the upgrade item is considered a valid upgrade. Returns true if the upgrade if valid
        /// </summary>
        /// <param name="item1"></param>
        /// <returns>Whether the upgrade item is a valid upgrade</returns>
        private bool CheckValid(Item item1)
        {
            if (UpgradeUISystem.UpgradeUIPanel.GetValidUpgrades().Contains(item1.type))
            {
                // Only executes if normal upgrade going into upgrade slot, or if is overclock going into the last slot (overclock slot)
                if (!((item1.ModItem is UpgradeTemplate { IsOverclock: true}) ^ (_slotID == UpgradeUISystem.UpgradeUIPanel.UpgradeSlots.Length - 1)))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Manages swapping item from or into the item slot
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns>If the method suceeded</returns>
        public bool SwapItemWithUI(ref Item item1, ref Item item2)
        {
            if (item1.ModItem is UpgradeTemplate)
            {
                if (CheckDuplicates(item1) && CheckValid(item1))
                {
                    // Proceed with swapping the item.
                    SwapItems(ref item1, ref item2);
                    return true;
                }
            }
            if (item1.type == 0 && item2.ModItem is UpgradeTemplate)
            {
                SwapItems(ref item1, ref item2);
                return true;
            }

            return false;
        }
        private void SwapItems(ref Item item1, ref Item item2)
        {
            if (UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem is not UpgradeableItemTemplate parentItem)
            {
                return;
            }
            Item placeholder = item1.Clone();
            item1 = item2.Clone();
            item2 = placeholder;
            SoundEngine.PlaySound(SoundID.Grab);
            parentItem.Upgrades[_slotID] = item2.type;
            parentItem.UpdateUpgrades();
            UpgradeUISystem.UpgradeUIPanel.ShowItems();
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);

            if (ItemSlot.ShiftInUse)
            {
                Player player = Main.player[Main.myPlayer];
                for (int i = 49; i > 0; i--)
                {
                    if (player.inventory[i].type == 0)
                    {
                        Item itemToDisplay = ItemToDisplay;
                        SwapItems(ref player.inventory[i], ref itemToDisplay);
                        return;

                    }
                }
            }

            if (UpgradeUISystem.UpgradeUIPanel.UpgradeSlots[_slotID] is UpgradeSlot { ItemToDisplay: Item upgrade})
            {
                SwapItemWithUI(ref Main.mouseItem, ref upgrade);
                
            }
        }
    }
}