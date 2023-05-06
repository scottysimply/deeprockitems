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

namespace deeprockitems.UI
{
    /*public class SuperSlotWrapper : UIElement
    {
        private Item[] _itemArray;

        private int _itemIndex;

        private int _itemSlotContext;

        private bool IsOverclockSlot;

        public SuperSlotWrapper(Item[] itemArray, int itemIndex, int itemSlotContext)
        {
            this._itemArray = itemArray;
            this._itemIndex = itemIndex;
            this._itemSlotContext = itemSlotContext;
            base.Width = new StyleDimension(48f, 0f);
            base.Height = new StyleDimension(48f, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            Parentitem
        }

        private void HandleItemSlotLogic()
        {
            if (base.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                Item inv = this._itemArray[this._itemIndex];
                ItemSlot.OverrideHover(ref inv, this._itemSlotContext);
                ItemSlot.MouseHover(ref inv, this._itemSlotContext);
                this._itemArray[this._itemIndex] = inv;
            }
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (Main.mouseItem.ModItem is UpgradeTemplate Upgrade)
            {
                if (!(Upgrade.IsOverclock ^ IsOverclockSlot) && (ParentItem.ValidUpgrades.Contains(Upgrade.Type)))
                {
                    int[] CurrentUpgrades = UpgradeUIPanel.GetItems();
                    if (!CurrentUpgrades.Contains(Upgrade.Type))
                    {
                        if (ItemInSlot.type == 0)
                        {
                            SaveItem_InSlot(Upgrade, false);
                            SoundEngine.PlaySound(SoundID.Grab);
                            ItemInSlot = Main.mouseItem;
                            Main.mouseItem = new Item();
                        }
                    }

                }

            }
            else if (Main.mouseItem.type == 0)
            {
                if (ItemInSlot.ModItem is UpgradeTemplate Upgrade2)
                {
                    SaveItem_InSlot(Upgrade2, true);
                    SoundEngine.PlaySound(SoundID.Grab);
                    Main.mouseItem = ItemInSlot;
                    ItemInSlot = new Item();
                }

            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            this.HandleItemSlotLogic();
            Item inv = this._itemArray[this._itemIndex];
            Vector2 position = base.GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
            ItemSlot.Draw(spriteBatch, ref inv, this._itemSlotContext, position);
        }
    }*/
    public class DisplaySlot : UIElement
    {
/*        public Item[] dummy = new Item[1];*/
        public Item ItemToDisplay { get; set; } = new Item();
        protected override void DrawSelf(SpriteBatch spriteBatch) // TODO: Fix this drawing code
        {
            float _InventoryScale = Main.inventoryScale;
            Texture2D SlotTexture = TextureAssets.InventoryBack.Value;
            Rectangle SlotDimensions = GetDimensions().ToRectangle();

            Color color = new Color(45, 60, 130);

            Vector2 vector = SlotTexture.Size() * _InventoryScale;

            spriteBatch.Draw(SlotTexture, SlotDimensions, color);

            if (ItemToDisplay.type > 0 && ItemToDisplay.stack > 0)
            {
                Main.instance.LoadItem(ItemToDisplay.type);
                Texture2D ItemInSlot_Texture = TextureAssets.Item[ItemToDisplay.type].Value;
                Rectangle ItemInSlot_Dimensions = ((Main.itemAnimations[ItemToDisplay.type] == null) ? ItemInSlot_Texture.Frame() : Main.itemAnimations[ItemToDisplay.type].GetFrame(ItemInSlot_Texture));

                float unknownScale = 1f;
                float spriteScale = 1f;
                if (ItemInSlot_Dimensions.Width > 32f || ItemInSlot_Dimensions.Height > 32f)
                {
                    spriteScale = ((ItemInSlot_Dimensions.Width <= ItemInSlot_Dimensions.Height) ? (32f / ItemInSlot_Dimensions.Height) : (32f / ItemInSlot_Dimensions.Width));
                }
                spriteScale *= _InventoryScale;
                Vector2 ItemDrawPosition = SlotDimensions.TopLeft() + vector / 2f - ItemInSlot_Dimensions.Size() * spriteScale / 2f;
                Vector2 origin = ItemInSlot_Dimensions.Size() * (spriteScale / 2f - .5f);
                if (ItemLoader.PreDrawInInventory(ItemToDisplay, spriteBatch, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetAlpha(Color.White), ItemToDisplay.GetColor(Color.White), origin, unknownScale * spriteScale))
                {
                    spriteBatch.Draw(ItemInSlot_Texture, ItemDrawPosition, ItemInSlot_Dimensions, Color.White, 0f, origin, spriteScale * unknownScale, SpriteEffects.None, 0f);
                }
                ItemLoader.PostDrawInInventory(ItemToDisplay, spriteBatch, ItemDrawPosition, ItemInSlot_Dimensions, ItemToDisplay.GetAlpha(Color.White), ItemToDisplay.GetColor(Color.White), origin, unknownScale * spriteScale);
            }

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.HoverItem = ItemToDisplay.Clone();
                Main.hoverItemName= ItemToDisplay.Name;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AdditionalUpdate();



        }
        public virtual void AdditionalUpdate()
        {

        }
    }
    public class UpgradeSlot : DisplaySlot
    {
        /// <summary>
        /// The item currently in the slot
        /// </summary>
        public Item ItemInSlot { get; set; } = new();

        /// <summary>
        /// The item that this slot will have effect on
        /// </summary>
        public static UpgradeableItemTemplate ParentItem { get; set; }

        /// <summary>
        /// If this slot is for overclocks or not
        /// </summary>
        public bool IsOverclockSlot;
        public UpgradeSlot(bool isOverclockSlot = false)
        {
            IsOverclockSlot = isOverclockSlot;
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (Main.mouseItem.ModItem is UpgradeTemplate Upgrade)
            {
                if (!(Upgrade.IsOverclock ^ IsOverclockSlot) && (ParentItem.ValidUpgrades.Contains(Upgrade.Type)))
                {
                    int[] CurrentUpgrades = UpgradeUIState.GetItems();
                    if (!CurrentUpgrades.Contains(Upgrade.Type))
                    {
                        if (ItemInSlot.type == 0)
                        {
                            SaveItem_InSlot(Upgrade, false);
                            SoundEngine.PlaySound(SoundID.Grab);
                            ItemInSlot = Main.mouseItem;
                            Main.mouseItem = new Item();
                        }
                    }

                }
                
            }
            else if (Main.mouseItem.type == 0)
            {
                if (ItemInSlot.ModItem is UpgradeTemplate Upgrade2)
                {
                    SaveItem_InSlot(Upgrade2, true);
                    SoundEngine.PlaySound(SoundID.Grab);
                    Main.mouseItem = ItemInSlot;
                    ItemInSlot = new Item();
                }

            }
        }
        public override void AdditionalUpdate()
        {
            ItemToDisplay = ItemInSlot;

        }
        public void SaveItem_InSlot(UpgradeTemplate Upgrade, bool removed)
        {
            UpgradeableItemTemplate Weapon = ParentItem;
            if (!removed)
            {
                if (Weapon is not null)
                {
                    BitsByte Helper = Weapon.Upgrades;
                    for (int u = 0; u < 8; u++)
                    {
                        if (Weapon.ValidUpgrades[u] == Upgrade.Type)
                        {
                            Helper[u] = true;
                        }
                    }
                    Weapon.Upgrades = Helper;
                    Weapon.UpdateUpgrades();
                }
            }
            else
            {
                if (Weapon is not null)
                {
                    BitsByte Helper = Weapon.Upgrades;
                    for (int u = 0; u < 8; u++)
                    {
                        if (Weapon.ValidUpgrades[u] == Upgrade.Type)
                        {
                            Helper[u] = false;
                        }
                    }
                    Weapon.Upgrades = Helper;
                    Weapon.UpdateUpgrades();
                }
            }
        }
        
    }
}