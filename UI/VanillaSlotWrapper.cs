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


                /*float MaxSize = SlotDimensions.Width;
                float unknownScale = 1f;
                Color currentColor = color;
                ItemSlot.GetItemLight(ref currentColor, ref unknownScale, ItemToDisplay.type);
                float spriteScale = 1f;
                if (ItemInSlot_Dimensions.Width > 32f || ItemInSlot_Dimensions.Height > 32f)
                {
                    spriteScale = ((ItemInSlot_Dimensions.Width <= ItemInSlot_Dimensions.Height) ? (32f / ItemInSlot_Dimensions.Height) : (32f / ItemInSlot_Dimensions.Width));
                }
                Vector2 ItemDrawPosition = SlotDimensions.TopLeft() + vector / 2f - ItemInSlot_Dimensions.Size() * (spriteScale * _InventoryScale) / 2f;
                Vector2 origin = new(0, 0);
                if (ItemLoader.PreDrawInInventory(ItemToDisplay, spriteBatch, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetAlpha(currentColor), ItemToDisplay.GetColor(color), origin, unknownScale * spriteScale))
                {
                    spriteBatch.Draw(ItemInSlot_Texture, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetAlpha(currentColor), 0f, origin, spriteScale * unknownScale, SpriteEffects.None, 0f);
                    if (ItemToDisplay.color != Color.Transparent)
                    {
                        spriteBatch.Draw(ItemInSlot_Texture, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetColor(color), 0f, origin, unknownScale * spriteScale, SpriteEffects.None, 0f);
                    }
                }
                ItemLoader.PostDrawInInventory(ItemToDisplay, spriteBatch, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetAlpha(currentColor), ItemToDisplay.GetColor(color), origin, unknownScale * spriteScale);*/


            }
            else
            {
                DrawIconHook(spriteBatch);
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
        public virtual void DrawIconHook(SpriteBatch spriteBatch) { }
    }
    public class UpgradeSlot : DisplaySlot
    {

        /// <summary>
        /// The item in the ParentSlot in the UI.
        /// </summary>
        public static UpgradeableItemTemplate ParentItem { get; set; }

        /// <summary>
        /// Tracking which slot goes to which item.
        /// </summary>
        private int tracker;
        public UpgradeSlot(int slotnumber = 1)
        {
            tracker = slotnumber;
        }
        public override void LeftMouseDown(UIMouseEvent evt)
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
        }
        public void SaveItem_InSlot(UpgradeTemplate Upgrade, bool removed)
        {
            UpgradeableItemTemplate Weapon = ParentItem;
            // If item was removed from the slot
            if (!removed)
            {
                if (Weapon is not null)
                {
                    /*BitsByte Helper = Weapon.Upgrades;
                    for (int u = 0; u < 8; u++)
                    {
                        if (Weapon.ValidUpgrades[u] == Upgrade.Type)
                        {
                            Helper[u] = true;
                        }
                    }
                    Weapon.Upgrades = Helper;
                    Weapon.UpdateUpgrades();*/
                    if (Weapon.Upgrades2[tracker] == 0)
                    {
                        Weapon.Upgrades2[tracker] = Upgrade.Item.type;
                    }
                    Weapon.UpdateUpgrades();
                }
            }
            // Save removing the item from the slot
            else
            {
                if (Weapon is not null)
                {
                    if (Weapon.Upgrades2[tracker] == Upgrade.Item.type)
                    {
                        Weapon.Upgrades2[tracker] = 0;
                    }
                    Weapon.UpdateUpgrades();
                }
            }
        }
        public override void DrawIconHook(SpriteBatch spriteBatch)
        {
            string background = tracker switch
            {
                0 => "UpgradeI",
                1 => "UpgradeII",
                2 => "UpgradeIII",
                _ => "UpgradeIV"
            };
            Texture2D texture = ModContent.Request<Texture2D>("deeprockitems/UI/UpgradeItem/" + background).Value;
            Rectangle rect = GetDimensions().ToRectangle();
            rect.Inflate(-12, -12);
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}