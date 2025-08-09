using Terraria;
using Terraria.UI;
using deeprockitems.Content.Items;
using System;
using deeprockitems.Content.Upgrades;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Audio;
using deeprockitems.UI.UpgradeUI.OverclockUI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeSelectionPanel : UpgradePanel
    {
        #region UI Elements

        /// <summary>
        /// Recipe display for whether an upgrade can be "bought" or not.
        /// </summary>
        public UpgradeRecipeDisplay RecipeDisplay { get; set; }
        /// <summary>
        /// The tiny menu that brings you to the overclock menu.
        /// </summary>
        public OverclockSummary OverclockDisplay { get; set; }
        /// <summary>
        /// The selection field for all of the upgrades
        /// </summary>
        public UpgradeSelectionContainer UpgradeContainer { get; set; }
        #endregion
        public override void PostInitialize()
        {
            float PADDING = 6;

            // Set recipe display position
            RecipeDisplay = new UpgradeRecipeDisplay {
                Top = { Pixels = ParentSlot.Top.Pixels },
                Width = { Pixels = this.Width.Pixels - ForgeButton.Width.Pixels - ParentSlot.Width.Pixels - 4 * PADDING },
                Left = { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING },
                Height = ParentSlot.Height,
                BackgroundColor = SecondaryBackgroundColor,
                BorderColor = SecondaryBorderColor
            };
            RecipeDisplay.SetState(null);
            Append(RecipeDisplay);

            // Upgrade container definition
            UpgradeContainer = new UpgradeSelectionContainer {
                // width and height are both 0
                Width = { Pixels = this.Width.Pixels-ForgeButton.Width.Pixels - 2*PADDING },
                Height = { Pixels = this.Height.Pixels-ForgeButton.Height.Pixels - 2*PADDING },
                Top = { Pixels = ParentSlot.Height.Pixels }
            };
            UpgradeContainer.OnLeftClick += UpgradeContainer_OnLeftClick;
            Append(UpgradeContainer);
            // Set overclock display
            OverclockDisplay = new() {
                HAlign = 1f,
                Top = { Pixels = ForgeButton.Top.Pixels + ForgeButton.Height.Pixels + PADDING },
                Width = ForgeButton.Width,
                // Height is the (parent's height - some offset)
                Height = { Pixels = this.Height.Pixels-(ForgeButton.Height.Pixels + 3 * PADDING) },
                BackgroundColor = SecondaryBackgroundColor,
                BorderColor = SecondaryBorderColor
            };
            Append(OverclockDisplay);
        }
        /// <summary>
        /// Selects the locked upgrade for crafting. Will not attempt to unlock the upgrade; refer to <see cref="UpgradeSelectionPanel.UnlockUpgrade()"/>
        /// </summary>
        private void SelectThisLockedUpgrade(Upgrade upgrade) {
            RecipeDisplay.SetState(upgrade);
        }

        protected void UpgradeContainer_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            // Determine which upgrade was clicked:
            if (evt.Target is not UpgradeSelectOption option) return;
            SoundEngine.PlaySound(SoundID.MenuTick);

            // DEBUG: allow any upgrade to be equipped
            if (deeprockitems.DebugMode)
            {
                SelectThisLockedUpgrade(option.Upgrade);
                option.SelectThisUpgrade();
                return;
            }

            if (!option.Upgrade.UpgradeState.IsUnlocked)
            {
                SelectThisLockedUpgrade(option.Upgrade);
                return;
            }

            option.SelectThisUpgrade();
        }
        protected override void ParentItemSlotChanged(Item itemNowInSlot, Item itemThatLeftSlot)
        {
            // Remove currently selected locked upgrade
            RecipeDisplay.SetState(null);

            // Set upgrade state
            if (itemNowInSlot.ModItem is IUpgradable modItem)
            {
                // Set upgrades
                UpgradeContainer.SetUpgrades(modItem.UpgradeMasterList);
                if (modItem.UpgradeMasterList.TryGetValue(UpgradeBuilder.OVERCLOCK_TIER, out UpgradeTier value))
                {
                    OverclockDisplay.SetOverclock(value.Where(ov => ov.UpgradeState.IsEquipped).FirstOrDefault() as Overclock ?? null);
                }
            }
            else
            {
                UpgradeContainer.SetUpgrades(null);
                OverclockDisplay.SetOverclock(null);
            }
        }

        protected override void OnClickForgeButton(UIMouseEvent evt, UIElement listeningElement)
        {
            // Check if the recipe could be unlocked or not
            if (RecipeDisplay.CurrentUpgrade is null || !RecipeDisplay.CurrentUpgrade.Recipe.TryToUnlockUpgrade(Main.LocalPlayer))
            {
                // funne sound
                SoundEngine.PlaySound(SoundID.Tink);
                return;
            }

            // Upgrade could be crafted
            RecipeDisplay.CurrentUpgrade.UpgradeState.IsUnlocked = true;
            RecipeDisplay.SetState(null);
            SoundEngine.PlaySound(SoundID.Unlock);
        }
    }
}
