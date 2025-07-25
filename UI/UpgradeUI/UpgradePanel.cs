using Terraria;
using Terraria.UI;
using deeprockitems.Content.Items;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.UI.UpgradeUI
{
    public abstract class UpgradePanel : UIPanel
    {
        public UpgradePanel() {

        }
        #region UI Elements
        public FakeItemSlot ParentSlot { get; set; }
        public UIButton<string> ForgeButton { get; set; }
        #endregion
        #region UI Colors
        public Color PrimaryBorderColor { get; set; } = Color.Black;
        public Color PrimaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        public Color SecondaryBorderColor { get; set; } = Color.Black;
        public Color SecondaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        public Color TertiaryBorderColor { get; set; } = Color.Black;
        public Color TertiaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        #endregion
        public override sealed void OnInitialize()
        {
            float PADDING = 6;
            SetPadding(PADDING);
            const float SLOT_SIZE = 46f;

            // Initialize the "craft" button
            ForgeButton = new UIButton<string>("Forge") {
                HAlign = 1f,
                Height = { Pixels = SLOT_SIZE, Percent = 0f },
                Width = { Pixels = 1.8f * SLOT_SIZE, Percent = 0f },
                TextScaleMax = 1.5f,
            };
            ForgeButton.BorderColor = SecondaryBorderColor;
            ForgeButton.BackgroundColor = SecondaryBackgroundColor;
            ForgeButton.TextOriginY -= 0.3f;
            ForgeButton.OnLeftClick += OnClickForgeButton;
            Append(ForgeButton);

            // Set size and position of parent slot
            ParentSlot = new FakeItemSlot((mouseItem, slotItem) => {
                if (mouseItem.ModItem is IUpgradable) return true;
                if (slotItem.type != 0 && (mouseItem.type == 0 || mouseItem.ModItem is IUpgradable)) return true;
                return false;
            }) {
                HAlign = 0f,
                Width = ForgeButton.Height,
                Height = ForgeButton.Height
            };
            ParentSlot.BorderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder");
            ParentSlot.BackgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
            ParentSlot.BorderColor = new(160, 90, 15);
            ParentSlot.BackgroundColor = new Color(82, 70, 50);
            ParentSlot.OnItemSwap += ParentItemSlotChanged;
            ParentSlot.GetItemToTrackInstead = () => UpgradeUIPlayer.ItemInUpgradeSlot;
            ParentSlot.SetItemToTrackInstead = (ref Item item) => UpgradeUIPlayer.ItemInUpgradeSlot = item;
            Append(ParentSlot);

            // Here goes the other initialization logic
            PostInitialize();
        }
        public override void OnActivate() {
            // When tabs are switched, make sure the upgrades or overclocks get set
            ParentItemSlotChanged(ParentSlot.ItemInSlot, new(0));
        }
        protected virtual void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {

        }
        /// <summary>
        /// Functions as <see cref="UIElement.OnInitialize"></see>
        /// </summary>
        public virtual void PostInitialize() {

        }
        protected virtual void ParentItemSlotChanged(Item itemNowInSlot, Item itemThatLeftSlot)
        {

        }
    }
}
