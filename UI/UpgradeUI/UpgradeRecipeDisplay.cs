using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using ReLogic.Content;
using deeprockitems.Utilities;
using Terraria.GameContent.UI.Elements;
using Terraria.UI.Chat;
using System;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeRecipeDisplay : UIElement
    {
        public UpgradeSelectOption Option;
        private UpgradeRecipeOption[] optionItems;
        private Texture2D _backgroundTexture;
        private Texture2D _borderTexture;
        private bool _needsLoad = true;
        public UIText RecipeTextDisplay { get; set; }
        public Color BackgroundColor { get; set; } = new Color(82, 70, 50);
        public Color BorderColor { get; set; } = new(160, 90, 15);
        LocalizedText recipeText => Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Recipe", () => "Recipe:");
        public UpgradeRecipeDisplay() {
            _ = recipeText;
        }
        private void LoadTextures() {
            _needsLoad = false;
            if (_borderTexture == null)
                _borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder").Value;

            if (_backgroundTexture == null)
                _backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground").Value;
        }
        public void SetState(UpgradeSelectOption option) {
            Option = option;
            // Ensure that no children exist
            RemoveAllChildren();
            optionItems = null;

            if (option is null || option.Upgrade.Recipe.Length == 0)
            {
                return;
            }
            Vector2 textSize = ChatManager.GetStringSize(FontAssets.DeathText.Value, recipeText.Value, new(1));
            float scale = (GetDimensions().Height - 24) / (textSize.Y - 24);
            RecipeTextDisplay = new(recipeText.Value, scale, true) {
                Left = { Pixels = 12f },
                Top = { Pixels = 0.5f * (GetDimensions().Height - 0.5f * scale * textSize.Y) },
                Width = { Pixels = scale * textSize.X },
                Height = { Pixels = scale * textSize.Y}
            };
            Append(RecipeTextDisplay);

            optionItems = new UpgradeRecipeOption[option.Upgrade.Recipe.Length];
            // Place the leftmost element first
            const float GAP = 14f;
            float size = 0.8f * GetDimensions().Height;
            float textDisplayRight = RecipeTextDisplay.Left.Pixels + RecipeTextDisplay.Width.Pixels;
            Vector2 center = new(textDisplayRight + 0.5f * (Width.Pixels - textDisplayRight), GetDimensions().Height / 2f);
            float gapToFirstElement = option.Upgrade.Recipe.Length % 2 == 0 ? (option.Upgrade.Recipe.Length / 2f - 0.5f) * GAP : MathF.Floor(option.Upgrade.Recipe.Length / 2f) * GAP;
            float sizeToFirstElement = MathF.Floor(option.Upgrade.Recipe.Length / 2f) * size;
            optionItems[0] = new UpgradeRecipeOption(option.Upgrade.Recipe.ItemsAndAmounts[0]) {
                Left = { Pixels = center.X - gapToFirstElement - sizeToFirstElement },
                Top = { Pixels = center.Y - 0.5f * size },
                Width = { Pixels = size },
                Height = { Pixels = size }
            };
            Append(optionItems[0]);

            // Auto place remaining elements
            for (int i = 1; i < optionItems.Length; i++)
            {
                optionItems[i] = new UpgradeRecipeOption(option.Upgrade.Recipe.ItemsAndAmounts[i]) {
                    Left = { Pixels = optionItems[0].Left.Pixels + i * (GAP + size) },
                    Top = { Pixels = center.Y - 0.5f * size },
                    Width = { Pixels = size },
                    Height = { Pixels = size }
                };
                Append(optionItems[i]);
            }
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (_needsLoad) LoadTextures();
            // Early return to ensure that a recipe is selected.
            if (Option is null) return;

            // Drawing logic
            var dimensions = GetDimensions().ToRectangle();
            // Draw background panel
            DRGHelpers.DrawPanel(spriteBatch, _borderTexture, 8, 8, new Vector2(dimensions.X, dimensions.Y), dimensions.Width, dimensions.Height, BorderColor);
            DRGHelpers.DrawPanel(spriteBatch, _backgroundTexture, 8, 8, new Vector2(dimensions.X, dimensions.Y), dimensions.Width, dimensions.Height, BackgroundColor);
            base.Draw(spriteBatch);
        }
    }
}
