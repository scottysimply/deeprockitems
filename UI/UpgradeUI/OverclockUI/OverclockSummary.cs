using deeprockitems.Content.Upgrades;
using deeprockitems.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockSummary : UIPanel
    {
        public UIText OverclockLabel { get; set; }
        public OverclockIcon OverclockIcon { get; set; }
        public Asset<DynamicSpriteFont> Font { get; set; } = FontAssets.DeathText;
        public override void OnInitialize() {
            base.OnInitialize();
            string text = Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclock", () => "Overclock").Value;
            Vector2 size = ChatManager.GetStringSize(Font.Value, text, new(1f));
            OverclockLabel = new(text, textScale: (GetDimensions().Width - 16f) / size.X, large: true) {
                Left = { Percent = 0f, Pixels = -4f },
                Height = { Pixels = 20f },
            };
            Append(OverclockLabel);
            OverclockIcon = new() {
                Left = { Pixels = 10f },
                Top = { Pixels = OverclockLabel.Top.Pixels + OverclockLabel.Height.Pixels + 16f }
            };
            float remainingHeight = this.Height.Pixels - OverclockIcon.Top.Pixels;
            OverclockIcon.Width.Pixels = OverclockIcon.Height.Pixels = 52f;
            Append(OverclockIcon);
        }
        public void SetOverclock(Overclock overclock) {
            OverclockIcon?.SetOverclock(overclock);
        }
    }
}
