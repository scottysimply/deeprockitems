using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    /// <summary>
    /// This is the indivudal tier of each upgrade.
    /// </summary>
    public class UpgradeSelectionTier : UIElement
    {
        int _tier;
        public UpgradeSelectionTier(UpgradeTier upgrades, int heightToWorkWith)
        {
            Options = upgrades.Select(upgrade => new UpgradeSelectOption(upgrades, upgrade)).ToArray();
            // Space out elements to fit in the height allowed.
            const int PADDING = 2;
            int computedHeight = (int)((heightToWorkWith - upgrades.Length * PADDING) / 3.5f);
            for (int i = 0; i < upgrades.Length; i++)
            {
                Options[i].Top.Pixels = 20f + (i + 1) * PADDING + i * computedHeight;
                // fixes centering
                Options[i].Left.Pixels = 1f;
                Options[i].Width.Pixels = Options[i].Height.Pixels = computedHeight;
                Append(Options[i]);
            }
            _tier = upgrades.Tier;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {

            var dimensions = GetDimensions().ToRectangle();
            base.DrawSelf(spriteBatch);
            // We want to ensure this draws _below_ the last upgrade
            var texture = Assets.UI.UpgradeTierNumbers.Value;
            var src = new Rectangle(0, (_tier - 1) * texture.Height / 5, texture.Width, texture.Height / 5);
            spriteBatch.Draw(texture, new Rectangle((int)(dimensions.Center.X - 0.5f * texture.Width), (int)((dimensions.Top + 12f) - 0.5f * src.Height), src.Width, src.Height), src, Color.White);
        }
        public UpgradeSelectOption[] Options;
    }
}
