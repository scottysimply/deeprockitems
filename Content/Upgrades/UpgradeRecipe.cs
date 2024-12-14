using System.Collections.Generic;

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
    }
}
