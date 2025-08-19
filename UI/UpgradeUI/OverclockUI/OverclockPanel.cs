using deeprockitems.Content.Items;
using deeprockitems.Content.Items.Misc;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockPanel : UpgradePanel
    {
        public OverclockService CurrentlyViewedOverclock { get; set; } = new();
        public FakeItemSlot MatrixCoreSlot { get; set; }
        public bool SelectedDetails = false;
        public bool SelectedMenu = false;
        public OverclockDetails Details { get; set; }
        public OverclockSelectionMenu SelectionMenu { get; set; }
        public UpgradeRecipeDisplay OverclockRecipe { get; set; }
        public override void PostInitialize() {
            int PADDING = 6;
            float slotSize = ParentSlot.GetDimensions().Width;
            MatrixCoreSlot = new FakeItemSlot((mouseItem, slotItem) => {
                // Removing from slot
                if (ParentSlot.ItemInSlot is null || ParentSlot.ItemInSlot.type == 0) return false;
                if (slotItem.type > 0 && mouseItem.type == 0) return true;
                // Adding or swapping to the slot (both are the same logic)
                if (mouseItem.ModItem is BlankMatrixCore core && core.InfusedOverclock != null)
                {
                    // Only allow matrix cores that match the parent slot's slot
                    if (ParentSlot.ItemInSlot.ModItem.Name == core.InfusedOverclock.WeaponName)
                    {
                        return true;
                    }
                }
                return false;
            }) {
                Left = { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING },
                Top = ParentSlot.Top,
                Width = { Pixels = slotSize },
                Height = { Pixels = slotSize },
                BackgroundColor = ParentSlot.BackgroundColor,
                BackgroundTexture = ParentSlot.BackgroundTexture,
                BorderColor = ParentSlot.BorderColor,
                BorderTexture = ParentSlot.BorderTexture
            };
            // upgrade recipe display
            OverclockRecipe = new() {
                Top = { Pixels = ParentSlot.Top.Pixels },
                Width = { Pixels = this.Width.Pixels - ForgeButton.Width.Pixels - MatrixCoreSlot.Width.Pixels - 4 * PADDING },
                Left = { Pixels = MatrixCoreSlot.Left.Pixels + MatrixCoreSlot.Width.Pixels + PADDING },
                Height = ParentSlot.Height,
                BackgroundColor = SecondaryBackgroundColor,
                BorderColor = SecondaryBorderColor
            };
            // setting position manually because of jank
            OverclockRecipe.Left.Pixels = MatrixCoreSlot.Left.Pixels + MatrixCoreSlot.Width.Pixels + PADDING;
            OverclockRecipe.SetState(null);
            Append(MatrixCoreSlot);
            // Left-side menu
            SelectionMenu = new OverclockSelectionMenu(CurrentlyViewedOverclock) {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING},
                BackgroundColor = SecondaryBackgroundColor,
                BorderColor = SecondaryBorderColor,
                ChildBorderColor = new Color(SecondaryBorderColor.ToVector3() * 0.8f),
                ChildBackgroundColor = new Color(SecondaryBackgroundColor.ToVector3() * 0.8f),
            };
            Append(SelectionMenu);
            SelectionMenu.Activate();
            // Right-side menu
            Details = new OverclockDetails() {
                Width = { Percent = 0.5f, Pixels = -PADDING },
                Height = { Percent = 1f, Pixels = -MatrixCoreSlot.Height.Pixels - PADDING },
                Left = { Percent = 0.5f },
                Top = { Pixels = MatrixCoreSlot.Height.Pixels + PADDING },
                BackgroundColor = SecondaryBackgroundColor,
                BorderColor = SecondaryBorderColor,
                ChildBorderColor = new Color(SecondaryBorderColor.ToVector3() * 0.8f),
                ChildBackgroundColor = new Color(SecondaryBackgroundColor.ToVector3() * 0.8f),
            };
            Append(Details);
            // Add delegates
            OnUpdate += OverclockPanel_OnUpdate;
            CurrentlyViewedOverclock.OnValueChanged += SelectedOverclock_OnValueChanged;
            MatrixCoreSlot.OnItemSwap += MatrixCoreChanged;

        }
        private void SelectedOverclock_OnValueChanged(Overclock newValue, Overclock oldValue) {
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
            // If the upgradable item was removed, spawn the matrix core
            if (itemNowInSlot.type != itemThatLeftSlot.type && itemNowInSlot.ModItem is IUpgradable && MatrixCoreSlot.ItemInSlot.type != 0)
            {
                Item tempItem = MatrixCoreSlot.ItemInSlot;
                Item air = new(0);
                MatrixCoreSlot.SwapItems(ref tempItem, ref air);
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_ReleaseEntity(), tempItem);
            }
            if ((itemNowInSlot.ModItem as IUpgradable)?.UpgradeMasterList.TryGetValue(UpgradeBuilder.OVERCLOCK_TIER, out UpgradeTier overclocks) ?? false)
            {
                SelectionMenu.SetOverclocks(overclocks);
                return;
            }
            CurrentlyViewedOverclock.ThisOverclock = null;
            SelectionMenu.SetOverclocks(null);
        }
        protected void MatrixCoreChanged(Item itemNowInSlot, Item itemThatLeftSlot) {
            if (itemNowInSlot.ModItem is BlankMatrixCore { InfusedOverclock: Overclock overclock })
            {
                OverclockRecipe.SetState(overclock);
                return;
            }
            OverclockRecipe.SetState(null);
        }
        protected override void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {
            if ((OverclockRecipe.CurrentUpgrade is null || OverclockRecipe.CurrentUpgrade.UpgradeState.IsUnlocked || !OverclockRecipe.CurrentUpgrade.Recipe.TryToUnlockUpgrade(Main.LocalPlayer)))
            {
                SoundEngine.PlaySound(SoundID.Tink);
                return;
            }

            // Slight difference from the way upgrades are handled: Dependencies are broken from matrix cores. We need to search for this overclock on the player
            var upgrades = (ParentSlot.ItemInSlot.ModItem as IUpgradable).UpgradeMasterList;
            foreach (var upgrade in upgrades[UpgradeBuilder.OVERCLOCK_TIER])
            {
                if (upgrade.UpgradeName == OverclockRecipe.CurrentUpgrade.UpgradeName)
                {
                    upgrade.UpgradeState.IsUnlocked = true;
                    SelectionMenu.RefreshMenu();
                    OverclockRecipe.SetState(null);
                    SoundEngine.PlaySound(SoundID.Unlock);
                    return;
                }
            }
            SoundEngine.PlaySound(SoundID.Tink);
        }
    }
}
