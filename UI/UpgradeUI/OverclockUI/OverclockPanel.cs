using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Misc;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockPanel : UpgradePanel
    {
        public OverclockService SelectedOverclock { get; set; } = new();
        public FakeItemSlot MatrixCoreSlot { get; set; }
        public bool SelectedDetails = false;
        public bool SelectedMenu = false;
        public OverclockDetails Details { get; set; }
        public OverclockSelectionMenu SelectionMenu { get; set; }
        public override void PostInitialize() {
            int PADDING = 6;
            float slotSize = ParentSlot.GetDimensions().Width;
            MatrixCoreSlot = new FakeItemSlot((mouseItem, slotItem) => {
                if (mouseItem.ModItem is BlankMatrixCore) return true;
                if (slotItem.type != 0 && (mouseItem.type == 0 || mouseItem.ModItem is BlankMatrixCore)) return true;
                return false;
            }) {
                Left = { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING },
                Top = ParentSlot.Top,
                Width = { Pixels = slotSize },
                Height = { Pixels = slotSize }
            };
            Append(MatrixCoreSlot);
            SelectionMenu = new OverclockSelectionMenu(SelectedOverclock) {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING},
            };
            Append(SelectionMenu);
            SelectionMenu.Activate();
            Details = new OverclockDetails() {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0.5f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING },
            };
            Append(Details);
            OnUpdate += OverclockPanel_OnUpdate;
            SelectedOverclock.OnValueChanged += SelectedOverclock_OnValueChanged;
        }

        private void SelectedOverclock_OnValueChanged(Overclock newValue, Overclock oldValue) {
            Main.NewText($"oldclock: {oldValue.DisplayName.Value ?? "none"}, newclock: {newValue.DisplayName.Value ?? "none"}");
        }
        public static float DesiredSelectedWidth => 260f;
        private void OverclockPanel_OnUpdate(UIElement affectedElement) {
            // Determine selected panel
            if (Details.IsMouseHovering)
            {
                SelectedDetails = true;
                SelectedMenu = false;
            }
            else if (SelectionMenu.IsMouseHovering)
            {
                SelectedMenu = true;
                SelectedDetails = false;
            }

            // Set selected width
            const float BaseSpeed = 6f;
            Func<float, float> getMultiplier = panelWidth => {
                return (2.5f * DesiredSelectedWidth / panelWidth) - 2.4f;
            };
            var detailsDims = Details.GetDimensions();
            var menuDims = SelectionMenu.GetDimensions();
            if (SelectedDetails)
            {
                if (detailsDims.Width < DesiredSelectedWidth)
                {
                    float multiplier = getMultiplier(detailsDims.Width);
                    Details.Width.Pixels += BaseSpeed * multiplier;
                    Details.Left.Pixels -= BaseSpeed * multiplier;
                    SelectionMenu.Width.Pixels -= BaseSpeed * multiplier;
                }
                if (detailsDims.Width > DesiredSelectedWidth)
                {
                    float difference = detailsDims.Width - DesiredSelectedWidth;
                    Details.Width.Pixels -= difference;
                    Details.Left.Pixels += difference;
                    SelectionMenu.Width.Pixels += difference;
                }
            }
            if (SelectedMenu)
            {
                if (menuDims.Width < DesiredSelectedWidth)
                {
                    float multiplier = getMultiplier(menuDims.Width);
                    SelectionMenu.Width.Pixels += BaseSpeed * multiplier;
                    Details.Width.Pixels -= BaseSpeed * multiplier;
                    Details.Left.Pixels += BaseSpeed * multiplier;
                }
                if (menuDims.Width > DesiredSelectedWidth)
                {
                    float difference = menuDims.Width - DesiredSelectedWidth;
                    Details.Width.Pixels += difference;
                    Details.Left.Pixels -= difference;
                    SelectionMenu.Width.Pixels -= difference;
                }
            }
        }

        protected override void ParentItemSlotChanged(Item itemNowInSlot, Item itemThatLeftSlot) {
            if ((itemNowInSlot.ModItem as IUpgradable)?.UpgradeMasterList.TryGetValue(UpgradeBuilder.OVERCLOCK_TIER, out UpgradeTier overclocks) ?? false)
            {
                SelectionMenu.SetOverclocks(overclocks);
                return;
            }
            SelectionMenu.SetOverclocks(null);
        }
        protected override void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {
            if (SelectedOverclock?.ThisOverclock is not null)
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
