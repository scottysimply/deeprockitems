using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.UI.UpgradeUI
{
    public class TabViewWithColors : TabViewPanel<UpgradePanel>
    {
        #region UI Colors
        public Color PrimaryBorderColor { get; set; } = Color.Black;
        public Color PrimaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        public Color SecondaryBorderColor { get; set; } = Color.Black;
        public Color SecondaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        public Color TertiaryBorderColor { get; set; } = Color.Black;
        public Color TertiaryBackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        #endregion
        public override void PostAddPanel(UpgradePanel panel) {
            panel.PrimaryBorderColor = PrimaryBorderColor;
            panel.PrimaryBackgroundColor = PrimaryBackgroundColor;
            panel.SecondaryBorderColor = SecondaryBorderColor;
            panel.SecondaryBackgroundColor = SecondaryBackgroundColor;
            panel.TertiaryBorderColor = TertiaryBorderColor;
            panel.TertiaryBackgroundColor = TertiaryBackgroundColor;
        }
    }
}
