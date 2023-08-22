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
        public virtual void MoveItems(ref Item item1, ref Item item2) { }
    }
    public class UpgradeSlot : DisplaySlot
    {
        public UpgradeSlot(int ID) : base(ID)
        {
            _slotID = ID;
        }
        /// <summary>
        /// Swaps item1 and item2. 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public override void MoveItems(ref Item item1, ref Item item2)
        {
            Item placeholder = item1.Clone();

            SoundEngine.PlaySound(SoundID.Grab);
            item1 = item2.Clone();
            item2 = placeholder;
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
            UpgradeUISystem.UpgradeUIPanel.ShowItems();
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            if (UpgradeUISystem.UpgradeUIPanel.UpgradeSlots[_slotID] is UpgradeSlot { ItemToDisplay: Item upgrade})
            {
                SwapItemWithUI(ref Main.mouseItem, ref upgrade);
                
            }
        }

        /*        public override void LeftMouseDown(UIMouseEvent evt)
                {
                    Item mouseItem = Main.mouseItem;

                    // If item is being taken from the slot
                    if (mouseItem.IsAir)
                    {
                        if (!ItemToDisplay.IsAir)
                        {

                            Main.mouseItem = ItemToDisplay.Clone();
                            ItemToDisplay = new Item();
                            SoundEngine.PlaySound(SoundID.Grab);
                            if (tracker == 3)
                            {
                                ParentItem.Overclock = 0;
                            }
                            else
                            {
                                ParentItem.Upgrades2[tracker] = 0;
                            }
                            ParentItem.UpdateUpgrades();

                        }
                    }
                    else if (mouseItem.ModItem is UpgradeTemplate Upgrade) // If the item being put into the slot is an upgrade
                    {
                        if (ItemToDisplay.IsAir) // If slot is empty
                        {
                            if (ParentItem.ValidUpgrades.Contains(Upgrade.Type)) // If Upgrade is a valid upgrade
                            {
                                // Code will only execute IF item is Overclock being put in an OC slot, or item is normal upgrade in a normal slot.
                                if (!(Upgrade.IsOverclock ^ tracker == 3))
                                {
                                    foreach (int i in ParentItem.Upgrades2)
                                    {
                                        if (i == Upgrade.Type) // Stop executing if the item is already a part of the UI. No duplicates!
                                        {
                                            return;
                                        }
                                    }

                                    if (tracker == 3)
                                    {
                                        ParentItem.Overclock = Upgrade.Item.type;
                                    }
                                    else
                                    {
                                        ParentItem.Upgrades2[tracker] = Upgrade.Item.type;

                                    }

                                    ParentItem.UpdateUpgrades();
                                    ItemToDisplay = Upgrade.Item;
                                    SoundEngine.PlaySound(SoundID.Grab);
                                    Main.mouseItem = new Item();
                                }
                            }

                        }
                    }
                }*/
    }
}