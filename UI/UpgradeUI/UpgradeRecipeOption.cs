using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using deeprockitems.Content.Upgrades;
using System.Linq;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeRecipeOption : UIElement
    {
        RecipeBinding _recipeIngredient;
        public UpgradeRecipeOption(RecipeBinding ingredient) {
            _recipeIngredient = ingredient;
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Get dimensions
            Rectangle dimensions = GetDimensions().ToRectangle();
            // Get draw scale
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = Width.Pixels / 52f;

            int currentID = _recipeIngredient.AcceptedTypes[(int)Main.timeForVisualEffects / 120 % _recipeIngredient.AcceptedTypes.Length];
            // Draw itemslot and utilize mousehover
            Item dummy = new Item(currentID, stack: _recipeIngredient.Stack);
            // Pass in context depending on if this recipe option can be fulfilled
            int context = 17;
            int count = _recipeIngredient.AcceptedTypes.Sum(itemID => Main.LocalPlayer.CountItem(itemID));
            if (count < _recipeIngredient.Stack)
            {
                // Give red slot
                context = 3;
            }
            ItemSlot.Draw(spriteBatch, ref dummy, context, new Vector2(dimensions.X, dimensions.Y));
            if (IsMouseHovering)
            {
                // Hover text
                dummy = new(_recipeIngredient.AcceptedTypes[0], stack: _recipeIngredient.Stack);
                ItemSlot.MouseHover(ref dummy, 22);
                // Set override name if the item is a multiple, and append any for multiple accepted types
                if (_recipeIngredient.AcceptedTypes.Length > 1)
                {
                    Main.HoverItem.SetNameOverride(Language.GetTextValue("LegacyMisc.37") + " " + dummy.Name);
                }
                // Add stack size
                if (dummy.stack > 1)
                {
                    Main.hoverItemName += $" ({dummy.stack})";
                }
            }
            // Reset scale
            Main.inventoryScale = oldScale;
        }
    }
}
