using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class OverclockListItem : UIPanel
    {
        public UIText Label { get; set; }
        public UIImage Icon { get; set; }
        private Overclock _internalOverclock;
        public OverclockListItem(Overclock overclock) {
            _internalOverclock = overclock;
        }
        public override void OnInitialize() {
            Label = new UIText(_internalOverclock.DisplayName, textScale: 0.75f);
            Icon = new UIImage(_internalOverclock.Texture.Value);
            Icon.Left.Pixels = 2f;
            Icon.Height.Pixels = 32f;
            Icon.Width.Pixels = 32f;
            Label.Left.Pixels = Icon.Left.Pixels + 6f;
            Icon.Activate();
            Label.Activate();
            Height.Pixels = 40f;
            Width.Pixels = Width.Pixels - 20f;
            Append(Icon);
            Append(Label);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            if (ContainsPoint(Main.MouseScreen)) {
                UICommon.TooltipMouseText($"[c/E3B465:{_internalOverclock.DisplayName}]\n" +
                                          $"{_internalOverclock.HoverText}");
            }
        }
    }
}
