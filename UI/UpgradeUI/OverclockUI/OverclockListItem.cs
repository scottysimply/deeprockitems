using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockListItem : UIPanel
    {
        public Overclock ThisOverclock;
        public UIText OverclockName { get; set; }
        public OverclockListItem(Overclock overclock) {
            ThisOverclock = overclock;
        }
        public override void OnInitialize() {
            float scale = 1f;
            Vector2 measuredSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, ThisOverclock.DisplayName.Value, Vector2.One);
            if (measuredSize.X > GetInnerDimensions().Width)
            {
                scale = this.GetInnerDimensions().Width / measuredSize.X;
            }
            OverclockName = new(ThisOverclock.DisplayName, textScale: scale) {
                IgnoresMouseInteraction = true
            };
            Append(OverclockName);
        }
    }
}
