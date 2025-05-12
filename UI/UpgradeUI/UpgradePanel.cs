using Terraria;
using Terraria.UI;
using deeprockitems.Content.Items;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public abstract class UpgradePanel : UIPanel
    {
        #region UI Elements
        public FakeItemSlot ParentSlot { get; set; }
        public UIButton<string> ForgeButton { get; set; }
        #endregion
        public override sealed void OnInitialize()
        {
            float MARGIN = 6;
            float PADDING = 6;
            SetPadding(PADDING);

            // Initialize the "craft" button
            ForgeButton = new UIButton<string>("Forge") {
                HAlign = 1f,
                Height = { Pixels = 52f, Percent = 0f },
                Width = { Pixels = 1.8f * 52, Percent = 0f },
                TextScaleMax = 1.5f,
            };
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
            ParentSlot.OnItemSwap += OnClickParentSlot;
            ParentSlot.GetItemToTrackInstead = () => (Parent as UpgradeState).ItemInSlot;
            ParentSlot.SetItemToTrackInstead = (ref Item item) => (Parent as UpgradeState).ItemInSlot = item;
            Append(ParentSlot);

            // Here goes the other initialization logic
            PostInitialize();
        }
        protected virtual void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {

        }
        /// <summary>
        /// Functions as <see cref="UIElement.OnInitialize"></see>
        /// </summary>
        public virtual void PostInitialize() {

        }
        protected virtual void OnClickParentSlot(Item itemNowInSlot, Item itemThatLeftSlot)
        {

        }
    }
}
