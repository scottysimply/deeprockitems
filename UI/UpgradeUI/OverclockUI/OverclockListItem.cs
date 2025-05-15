using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

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
            OverclockName = new(ThisOverclock.DisplayName) {
                IgnoresMouseInteraction = true
            };
            Append(OverclockName);
        }
    }
}
