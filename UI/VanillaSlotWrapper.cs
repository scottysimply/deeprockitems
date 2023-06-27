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
    public class DisplaySlot : UIElement
    {
        public Item ItemToDisplay { get; set; } = new Item();
        protected override void DrawSelf(SpriteBatch spriteBatch) // TODO: Fix this drawing code
        {
            float _InventoryScale = Main.inventoryScale;
            Texture2D SlotTexture = ModContent.Request<Texture2D>("deeprockitems/UI/BlankSlot").Value;
            Rectangle SlotDimensions = GetDimensions().ToRectangle();

            // Color color = new Color(45, 60, 130);
            Color color = Color.White;

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
            else
            {
                DrawHook(spriteBatch);
            }

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.HoverItem = ItemToDisplay.Clone();
                Main.hoverItemName = ItemToDisplay.Name;
            }
        }
        public virtual void DrawHook(SpriteBatch spriteBatch) { }
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
        /// Which slot is this? Used for rendering. 0 is overclock slot
        /// </summary>
        public int tracker;
        public UpgradeSlot(int slotnumber = 1)
        {
            tracker = slotnumber;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (Main.mouseItem.ModItem is UpgradeTemplate Upgrade)
            {
                if (!(Upgrade.IsOverclock ^ (tracker == 0)) && (ParentItem.ValidUpgrades.Contains(Upgrade.Type)))
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
        public override void DrawHook(SpriteBatch spriteBatch)
        {
            string background = tracker switch
            {
                1 => "UpgradeI",
                2 => "UpgradeII",
                3 => "UpgradeIII",
                _ => "UpgradeIV"
            };
            Texture2D texture = ModContent.Request<Texture2D>("deeprockitems/UI/" + background).Value;
            Rectangle rect = GetDimensions().ToRectangle();
            rect.Inflate(-12, -12);
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}