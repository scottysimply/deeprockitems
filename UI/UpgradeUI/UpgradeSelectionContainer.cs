using Terraria.UI;
using System.Linq;
using deeprockitems.Content.Upgrades;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is the entire panel that contains each tier of upgrade
    /// </summary>
    public class UpgradeSelectionContainer : UIElement
    {
        UpgradeSelectionTier[] upgradeSelectors;
        public void SetUpgrades(UpgradeList upgrades)
        {
            // Begin by removing all children
            RemoveAllChildren();
            // If the upgrades are not set, there's no upgrade list and nothing should draw.
            if (upgrades is null || upgrades.Count == 0)
            {
                upgradeSelectors = [];
                return;
            }
            upgradeSelectors = upgrades.Where(tier => tier.Key != UpgradeBuilder.OVERCLOCK_TIER).Select(tier => new UpgradeSelectionTier(tier.Value, (int)Height.Pixels)).ToArray();
            float sizeOfSelector = upgradeSelectors[0].Children.First().Height.Pixels;
            // Construct the first tier
            upgradeSelectors[0].Left.Pixels = 0;
            upgradeSelectors[0].Width.Pixels = sizeOfSelector;
            upgradeSelectors[0].Height.Pixels = Height.Pixels;
            Append(upgradeSelectors[0]);
            // if there's a last tier, construct that too!
            if (upgradeSelectors.Length > 1)
            {
                upgradeSelectors[^1].HAlign = 1f;
                upgradeSelectors[^1].Width.Pixels = sizeOfSelector;
                upgradeSelectors[^1].Height.Pixels = Height.Pixels;
                Append(upgradeSelectors[^1]);
            }
            // then, construct the middle
            float computedWidth = (Width.Pixels - sizeOfSelector) / (upgradeSelectors.Length - 1);
            for (int i = 1; i < upgradeSelectors.Length - 1; i++)
            {
                upgradeSelectors[i].Left.Pixels = i * computedWidth;
                upgradeSelectors[i].Width.Pixels = sizeOfSelector;
                upgradeSelectors[i].Height.Pixels = Height.Pixels;
                Append(upgradeSelectors[i]);
            }
        }
    }
}
