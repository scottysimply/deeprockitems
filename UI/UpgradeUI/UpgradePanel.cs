using Terraria;
using Terraria.UI;
using deeprockitems.Content.Items;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria.ModLoader.UI;
using deeprockitems.Content.Upgrades;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Steamworks;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradePanel : UIPanel
    {
        #region UI Elements
        public OverclockMenu OverclockDisplay;
        public FakeItemSlot ParentSlot;
        public UpgradeSelectionContainer UpgradeContainer;
        public UIButton<string> ForgeButton;
        #endregion
        public override void OnInitialize()
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
            ForgeButton.OnLeftClick += ForgeButton_OnLeftClick;
            Append(ForgeButton);

            // Set size and position of parent slot
            ParentSlot = new FakeItemSlot((mouseItem, slotItem) => {
                if (mouseItem.ModItem is IUpgradable)
                {
                    return true;
                }
                else if (slotItem.type != 0 && (mouseItem.type == 0 || mouseItem.ModItem is IUpgradable))
                {
                    return true;
                }
                return false;
            }) {
                HAlign = 0f,
                Width = ForgeButton.Height,
                Height = ForgeButton.Height
            };
            ParentSlot.OnItemSwap += ParentSlot_OnItemSwap;
            Append(ParentSlot);

            // Set recipe display position
            RecipeDisplay = new UpgradeRecipeDisplay {
                Top = { Pixels = ParentSlot.Top.Pixels },
                Width = { Pixels = this.Width.Pixels - ForgeButton.Width.Pixels - ParentSlot.Width.Pixels - 4 * PADDING },
                Left = { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels + PADDING },
                Height = ParentSlot.Height,
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
            };
            Append(OverclockDisplay);
        }
        /// <summary>
        /// Recipe display for whether an upgrade can be "bought" or not.
        /// </summary>
        public UpgradeRecipeDisplay RecipeDisplay { get; set; }
        /// <summary>
        /// Selects the locked upgrade for crafting. Will not attempt to unlock the upgrade; refer to <see cref="UpgradePanel.UnlockUpgrade()"/>
        /// </summary>
        private void SelectThisLockedUpgrade(UpgradeSelectOption option) {
            RecipeDisplay.SetState(option);
        }

        private void UpgradeContainer_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            // Determine which upgrade was clicked:
            if (evt.Target is not UpgradeSelectOption option) return;
            SoundEngine.PlaySound(SoundID.MenuTick);

            // DEBUG: allow any upgrade to be equipped
            if (deeprockitems.DebugMode)
            {
                SelectThisLockedUpgrade(option);
                option.SelectThisUpgrade();
                return;
            }

            if (!option.Upgrade.UpgradeState.IsUnlocked)
            {
                SelectThisLockedUpgrade(option);
                return;
            }

            option.SelectThisUpgrade();
        }
        private void ParentSlot_OnItemSwap(Item itemNowInSlot, Item itemThatLeftSlot)
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

                    OverclockDisplay.SetOverclocks(value);
                }
            }
            else
            {
                UpgradeContainer.SetUpgrades(null);
                OverclockDisplay.SetOverclocks(null);
            }
        }

        private void ForgeButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            // Check if a current recipe is put in
            if (RecipeDisplay.Option is null || RecipeDisplay.Option.Upgrade is null) return;

            // Check if the ingredients of the recipe are in the player's inventory
            if (!TryToCraftItem(Main.LocalPlayer, RecipeDisplay.Option.Upgrade.Recipe))
            {
                // cannot craft sound
                SoundEngine.PlaySound(SoundID.Tink);
                return;
            }

            // Unlock the upgrade, equip it, and disable the recipe
            RecipeDisplay.Option.Upgrade.UpgradeState.IsUnlocked = true;

            // Select this upgrade through the recipe
            RecipeDisplay.Option.SelectThisUpgrade();
            // Set state to null
            RecipeDisplay.SetState(null);
            // Play sound to let the player know items were taken
            SoundEngine.PlaySound(SoundID.Unlock);

        }
        /// <summary>
        /// Attempt crafting the upgrade. Returns true if the item was successfully crafted.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private bool TryToCraftItem(Player player, UpgradeRecipe recipe) {
            List<Item> matchingItems = [];

            // Search for recipes for each item.
            for (int recipeIndex = 0; recipeIndex < recipe.Length; recipeIndex++)
            {
                for (int invIndex = 0; invIndex < 50; invIndex++)
                {
                    // Filter accepted types and allow it to be used for crafting
                    if (!recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(player.inventory[invIndex].type)) continue;
                    matchingItems.Add(player.inventory[invIndex]);
                }

                // Sum each stack of items.
                int totalStack = matchingItems.Where(item => recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)).Sum(item => item.stack);
                // No items? :megamind:
                if (totalStack < recipe.ItemsAndAmounts[recipeIndex].Stack) return false;
            }

            // Take items from the player's inventory to craft
            for (int recipeIndex = 0; recipeIndex < recipe.Length; recipeIndex++)
            {
                int itemsRequired = recipe.ItemsAndAmounts[recipeIndex].Stack;
                foreach (var item in matchingItems)
                {
                    if (!recipe.ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)) continue;

                    // Awful code but i'm not sure how to do this better
                    int currentStack = 0;
                    while (currentStack < itemsRequired)
                    {
                        item.stack--;
                        currentStack++;
                        if (item.stack == 0)
                        {
                            item.TurnToAir();
                            item.maxStack = 0; // still not fixed in the year of our lord 2025
                        }
                    }
                }
            }
            return true;
        }
    }
}
