using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeRecipe
    {
        public List<RecipeBinding> ItemsAndAmounts { get; set; }
        public int Length => ItemsAndAmounts.Count;
        public UpgradeRecipe() {
            ItemsAndAmounts = [];
        }
        public UpgradeRecipe AddIngredient(int type, int stack) {
            ItemsAndAmounts.Add(new RecipeBinding([type], stack));
            return this;
        }
        public UpgradeRecipe AddCandidateIngredient(int[] types, int stack) {
            ItemsAndAmounts.Add(new RecipeBinding(types, stack));
            return this;
        }
        /// <summary>
        /// Attempts unlocking a recipe given the current player. <i>Will</i> remove items from the player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryToUnlockUpgrade(Player player) {
            List<Item> matchingItems = [];

            // Search for recipes for each item.
            for (int recipeIndex = 0; recipeIndex < Length; recipeIndex++)
            {
                for (int invIndex = 0; invIndex < 50; invIndex++)
                {
                    // Filter accepted types and allow it to be used for crafting
                    if (!ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(player.inventory[invIndex].type)) continue;
                    matchingItems.Add(player.inventory[invIndex]);
                }

                // Sum each stack of items.
                int totalStack = matchingItems.Where(item => ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)).Sum(item => item.stack);
                // No items? :megamind:
                if (totalStack < ItemsAndAmounts[recipeIndex].Stack) return false;
            }

            // Take items from the player's inventory to craft
            for (int recipeIndex = 0; recipeIndex < Length; recipeIndex++)
            {
                int itemsRequired = ItemsAndAmounts[recipeIndex].Stack;
                foreach (var item in matchingItems)
                {
                    if (!ItemsAndAmounts[recipeIndex].AcceptedTypes.Contains(item.type)) continue;

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
