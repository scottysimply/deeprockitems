using deeprockitems.Content.Upgrades;
using deeprockitems.Types;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockSummary : UIPanel
    {
        public UIText OverclockLabel { get; set; }
        public OverclockIcon OverclockIcon { get; set; }
        public UIButton<LocalizedText> OverclockStateButton { get; set; }
        public override void OnInitialize() {
            base.OnInitialize();
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclock", () => "Overclock"), textScale: 1f) {
                Left = { Percent = 0f, Pixels = -4f },
                Height = { Pixels = 20f },
            };
            OverclockLabel.OnLeftClick += (UIMouseEvent evt, UIElement sender) => {
                ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.SetState<OverclockPanel>();
            };
            Append(OverclockLabel);
            OverclockStateButton = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.ViewOverclocks", () => "View Overclocks")) {
                Left = { Percent = 0f },
                Top = { Pixels = OverclockLabel.Height.Pixels + 5f },
                Width = { Pixels = GetDimensions().Width + 20f },
                Height = { Pixels = 30f}
            };
            OverclockStateButton.OnLeftClick += (UIMouseEvent evt, UIElement sender) => {
                ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.SetState<OverclockPanel>();
            };
            Append(OverclockStateButton);
            OverclockIcon = new() {
                Left = { Pixels = 10f },
                Top = { Pixels = OverclockStateButton.Top.Pixels + OverclockStateButton.Height.Pixels + 5f }
            };
            float remainingHeight = this.Height.Pixels - OverclockIcon.Top.Pixels;
            OverclockIcon.Width.Pixels = OverclockIcon.Height.Pixels = 52f;
            Append(OverclockIcon);
        }
        public void SetOverclock(Overclock overclock) {
            OverclockIcon.SetOverclock(overclock);
        }
    }
}
