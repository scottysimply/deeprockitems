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
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclock", () => "Overclock"), textScale: 0.66f) {
                Left = { Percent = 0f, Pixels = -4f }
            };
            OverclockLabel.OnLeftClick += (UIMouseEvent evt, UIElement sender) => {
                ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.SetState<OverclockPanel>();
            };
            Append(OverclockLabel);
            OverclockStateButton = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.ViewOverclocks", () => "View Overclocks")) {
                Left = { Percent = 0f },
                Top = { Pixels = OverclockLabel.Height.Pixels },
                Width = { Percent = 1f },
                Height = { Pixels = 30f}
            };
            OverclockStateButton.OnLeftClick += (UIMouseEvent evt, UIElement sender) => {
                ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.SetState<OverclockPanel>();
            };
            Append(OverclockStateButton);
            OverclockIcon = new() {
                Left = { Percent = 0.5f },
                Top = { Pixels = OverclockStateButton.Top.Pixels + OverclockStateButton.Height.Pixels }
            };
            Append(OverclockIcon);
        }
        public void SetOverclock(Overclock overclock) {
            OverclockIcon.SetOverclock(overclock);
        }
    }
}
