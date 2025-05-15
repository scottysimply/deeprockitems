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
    public class UpgradeSelectOption : TweenableElement
    {
        protected Asset<Texture2D> backgroundImage { get => Upgrade.Background; }
        protected Asset<Texture2D> icon { get => Upgrade.Texture; }
        protected UpgradeTier _upgrades;
        public UpgradeSelectOption(UpgradeTier upgrades, Upgrade upgrade) : base()
        {
            Upgrade = upgrade;
            _upgrades = upgrades;
        }

        /// <summary>
        /// The upgrade that is represented by this UIElement.
        /// </summary>
        public Upgrade Upgrade;
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
            (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.ItemInSlot.ModItem as IUpgradable).ApplyStatUpgrades();

        }
        public override void DrawHook(SpriteBatch spriteBatch)
        {
            // Enable tweening blocker if this upgrade is the selected recipe

            if ((Parent.Parent.Parent as UpgradeSelectionPanel)?.RecipeDisplay.Option?.Upgrade == Upgrade)
            {
                AllowedToTween = false;
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
            // Draw the actual slot
            spriteBatch.Draw(backgroundImage.Value, (Rectangle)ScaledDimensions, drawColor);

            // Draw upgrade icon
            float scale = 0.7f * currentScale;
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
        Color BaseColor => new Color(252, 93, 38);
        Color SelectedColor => new Color(242, 227, 62);
    }
}
