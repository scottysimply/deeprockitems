using deeprockitems.Content.Items;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.UI;
using Terraria.ModLoader;
using Terraria.UI;
using ReLogic.Content;
using deeprockitems.Types;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is each individual upgrade represented as UI.
    /// </summary>
    public class UpgradeSelectOption : UIElement
    {
        protected Asset<Texture2D> backgroundImage { get => Upgrade.Background; }
        protected Asset<Texture2D> icon { get => Upgrade.Texture; }
        protected UpgradeTier _upgrades;
        public UpgradeSelectOption(UpgradeTier upgrades, Upgrade upgrade)
        {
            Upgrade = upgrade;
            _upgrades = upgrades;
            HoverScale = _minScale;
        }
        public bool TweenBlock { get; set; } = false;

        /// <summary>
        /// The upgrade that is represented by this UIElement.
        /// </summary>
        public Upgrade Upgrade;
        public float HoverScale { get; set; }
        const float _minScale = 0.8f;
        const float _maxScale = 1f;
        private Rectangle _dimensions => GetDimensions().ToRectangle();
        public RectangleF ScaledDimensions => new RectangleF(_dimensions.Center.X - 0.5f * HoverScale * _dimensions.Width, _dimensions.Center.Y - 0.5f * HoverScale * _dimensions.Height, HoverScale * _dimensions.Width, HoverScale * _dimensions.Height);
        public new bool IsMouseHovering => ScaledDimensions.Contains(Main.MouseScreen);
        /// <summary>
        /// Deselects all other upgrades in the tier and forces this upgrade to be equipped.
        /// </summary>
        public void SelectThisUpgrade() {
            foreach (var upgrade in _upgrades)
            {
                if (upgrade == Upgrade) continue;

                // Deselect all upgrades except for this one
                upgrade.UpgradeState.IsEquipped = false;
            }

            // Invert this upgrade
            Upgrade.UpgradeState.IsEquipped = !Upgrade.UpgradeState.IsEquipped;

            // Apply stat changes
            (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.Panel.ParentSlot.ItemInSlot.ModItem as IUpgradable).ApplyStatUpgrades();

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Enable tweening blocker if this upgrade is the selected recipe
            if (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.Panel.RecipeDisplay.Option?.Upgrade == Upgrade)
            {
                TweenBlock = true;
            }
            // Get slot color
            Color drawColor = BaseColor;

            // Show hover text if the slot is being hovered
            if (IsMouseHovering)
            {
                // Mouse text to draw
                string mouseText = $"[c/E3B465:{Upgrade.DisplayName}]\n" +
                                   $"{Upgrade.HoverText}";
                // Add locked text
                if (!Upgrade.UpgradeState.IsUnlocked)
                {
                    mouseText = "[c/F13010:[Locked][c/F13010:]] " + mouseText;
                }
                // Draw
                UICommon.TooltipMouseText(mouseText);
            }
            // Tween if being hovered
            HandleTweening();

            // Draw the actual slot
            spriteBatch.Draw(backgroundImage.Value, (Rectangle)ScaledDimensions, drawColor);

            // Draw upgrade icon
            float scale = 0.7f * HoverScale;
            Rectangle destination = new((int)(ScaledDimensions.Center.X - ScaledDimensions.Width * 0.5f), (int)(ScaledDimensions.Center.Y - ScaledDimensions.Height * 0.5f), (int)ScaledDimensions.Width, (int)ScaledDimensions.Height);
            spriteBatch.Draw(icon.Value, destination, Color.White);

            // Draw outline if equipped
            if (Upgrade.UpgradeState.IsEquipped)
            {
                RectangleF outlineDimensions = new RectangleF(ScaledDimensions.Center.X - 0.5f * Assets.UI.UpgradeSlotOutline.Value.Height, ScaledDimensions.Y - 0.5f * Assets.UI.UpgradeSlotOutline.Value.Height, Assets.UI.UpgradeSlotOutline.Value.Width, Assets.UI.UpgradeSlotOutline.Value.Height);
                spriteBatch.Draw(Assets.UI.UpgradeSlotOutline.Value, (Rectangle)ScaledDimensions, SelectedColor);
            }
            // Don't draw lock if unlocked
            if (Upgrade.UpgradeState.IsUnlocked) return;
            // Draw lock with top left near center
            spriteBatch.Draw(Assets.UI.UpgradeLock.Value, new Rectangle((int)(destination.Center.X + scale * 0.2f * ScaledDimensions.Width), (int)(destination.Center.Y + scale * 0.1f * ScaledDimensions.Height), (int)(scale * Assets.UI.UpgradeLock.Width()), (int)(scale * Assets.UI.UpgradeLock.Height())), Color.White);
        }
        protected void HandleTweening() {
            // Enable size block
            if (TweenBlock)
            {
                TweenBlock = false;
                return;
            }
            // Grow slightly
            if (IsMouseHovering)
            {
                if (HoverScale < _maxScale)
                {
                    HoverScale += 0.03f;
                }
                else if (HoverScale > _maxScale)
                {
                    HoverScale = _maxScale;
                }
            }
            else
            {
                if (_minScale < HoverScale)
                {
                    HoverScale -= 0.05f;
                }
                else if (_minScale > HoverScale)
                {
                    HoverScale = _minScale;
                }
            }
        }
        Color BaseColor => new Color(252, 93, 38);
        Color SelectedColor => new Color(242, 227, 62);
    }
}
