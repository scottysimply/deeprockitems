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
        public UpgradeSelectionContainer(float width, float height)
        {
            Width.Pixels = width;
            Height.Pixels = height;
        }
        public void SetUpgrades(UpgradeList upgrades)
        {
            // Begin by removing all children
            RemoveAllChildren();
            // If the upgrades are not set, the parent item was removed. Undo everything.
            if (upgrades is null)
            {
                upgradeSelectors = [];
                return;
            }

            upgradeSelectors = upgrades.Select(tier => new UpgradeSelectionTier(tier.Value, (int)Height.Pixels)).ToArray();
            const int GAP = 4;
            int computedWidth = (int)((Width.Pixels - upgradeSelectors.Length * GAP) / (float)upgradeSelectors.Length);
            for (int i = 0; i < upgradeSelectors.Length; i++)
            {
                upgradeSelectors[i].Left.Pixels = i * (GAP + computedWidth);
                upgradeSelectors[i].Width.Pixels = computedWidth;
                upgradeSelectors[i].Height.Pixels = Height.Pixels;

                // Append
                Append(upgradeSelectors[i]);
            }
        }
    }
}
