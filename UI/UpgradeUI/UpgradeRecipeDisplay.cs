using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using ReLogic.Content;
using deeprockitems.Utilities;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeRecipeDisplay : UIElement
    {
        public UpgradeSelectOption Option;
        private UpgradeRecipeOption[] optionItems;
        LocalizedText recipeText => Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Recipe", () => "Recipe:");
        public UpgradeRecipeDisplay() {
            _ = recipeText;
        }
        public void SetState(UpgradeSelectOption option) {
            SetPadding(6f);
            Option = option;
            // Ensure that no children exist
            RemoveAllChildren();
            optionItems = null;

            if (option is null)
            {
                return;
            }

            optionItems = new UpgradeRecipeOption[option.Upgrade.Recipe.Length];
            // Calculate available positions for children. Recipe items get aligned to the right
            Rectangle dimensions = GetInnerDimensions().ToRectangle();
            for (int i = optionItems.Length - 1; 0 <= i; i--)
            {
                int reverseIndex = optionItems.Length - 1 - i;
                // Init
                optionItems[reverseIndex] = new UpgradeRecipeOption((int)((Parent as UpgradeSelectionPanel).ForgeButton.Height.Pixels * 0.8f), option.Upgrade.Recipe.ItemsAndAmounts[i]);
                // Set positions
                optionItems[reverseIndex].Left.Pixels = Width.Pixels - (reverseIndex + 1) * (optionItems[reverseIndex].Width.Pixels + 14f);
                optionItems[reverseIndex].VAlign = 0.5f;
                Append(optionItems[reverseIndex]);
            }
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Early return to ensure that a recipe is selected.
            if (Option is null) return;

            // Drawing logic
            var dimensions = GetDimensions().ToRectangle();
            var inner = GetDimensions().ToRectangle();
            // Draw background panel
            DRGHelpers.DrawPanel(spriteBatch, TextureAssets.InventoryBack9.Value, 8, 8, new Vector2(inner.X, inner.Y), inner.Width, inner.Height, Color.White);
            int xOffset = 16;
            // Calculate scale:
            Asset<DynamicSpriteFont> font = FontAssets.MouseText;
            Vector2 fontSize = font.Value.MeasureString(recipeText.Value);
            float scale = dimensions.Height / fontSize.Y;
            // Restart spritebatch with nearest neighbor to remove the terrible, horrifying, blur.
            Utils.DrawBorderString(spriteBatch, recipeText.Value, new Vector2(dimensions.X + xOffset, dimensions.Y + (fontSize.Y * 0.25f)), Color.White, scale: scale);


            // Draw children
            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
        }
    }
}
