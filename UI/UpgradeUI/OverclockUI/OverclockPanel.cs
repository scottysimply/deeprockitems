using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Misc;
using deeprockitems.Content.Upgrades;
using Terraria;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockPanel : UpgradePanel
    {
        public OverclockService SelectedOverclock { get; set; } = new();
        public FakeItemSlot MatrixCoreSlot { get; set; }
        public OverclockDetails Details { get; set; }
        public OverclockSelectionMenu SelectionMenu { get; set; }
        public override void PostInitialize() {
            int PADDING = 6;
            MatrixCoreSlot = new FakeItemSlot((mouseItem, slotItem) => {
                if (mouseItem.ModItem is MatrixCore) return true;
                if (slotItem.type != 0 && (mouseItem.type == 0 || mouseItem.ModItem is MatrixCore)) return true;
                return false;
            }) {
                Left = { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING },
                Top = ParentSlot.Top,
                Width = { Pixels = 52 },
                Height = { Pixels = 52 }
            };
            Append(MatrixCoreSlot);
            SelectionMenu = new OverclockSelectionMenu(SelectedOverclock) {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING}
            };
            Append(SelectionMenu);
            SelectionMenu.Activate();
            Details = new OverclockDetails() {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0.5f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING }
            };
            Append(Details);
            SelectionMenu.SetOverclocks((ParentSlot.ItemInSlot.ModItem as IUpgradable)?.UpgradeMasterList[UpgradeBuilder.OVERCLOCK_TIER] ?? null);
        }
        protected override void OnClickParentSlot(Item itemNowInSlot, Item itemThatLeftSlot) {
            if (itemNowInSlot.ModItem is IUpgradable modItem)
            {
                SelectionMenu.SetOverclocks(modItem.UpgradeMasterList[UpgradeBuilder.OVERCLOCK_TIER] ?? null);
                return;
            }
            SelectionMenu.SetOverclocks(null);
        }
        protected override void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {
            if (SelectedOverclock != null)
            {
                Main.NewText($"Verified OC: {SelectedOverclock.ThisOverclock.DisplayName}");
            }
            if (MatrixCoreSlot.ItemInSlot?.type != 0)
            {
                MatrixCoreSlot.ItemInSlot.stack--;
                if (MatrixCoreSlot.ItemInSlot.stack == 0)
                {
                    MatrixCoreSlot.ItemInSlot.TurnToAir();
                }
                // Consume item in slot
                Main.NewText("Explode");
            }
        }
    }
}
