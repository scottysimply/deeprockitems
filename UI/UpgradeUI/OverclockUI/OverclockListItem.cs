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
        public OverclockIcon Icon { get; set; }
        public UIText OverclockName { get; set; }
        public OverclockListItem(Overclock overclock) {
            ThisOverclock = overclock;
        }
        public override void OnInitialize() {
            var innerDims = GetInnerDimensions();
            Icon = new();
            Icon.Width.Pixels = Icon.Height.Pixels = GetDimensions().ToRectangle().Height;
            Icon.Left.Pixels = -10f;
            Icon.Top.Pixels = 4f;
            Icon.SetOverclock(ThisOverclock);
            Icon.ShowTooltip = false;
            Append(Icon);


            float nameScale = 1f;
            Vector2 measuredSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, ThisOverclock.DisplayName.Value, Vector2.One);
            if (measuredSize.X > innerDims.Width - Icon.Width.Pixels + 8f)
            {
                nameScale = (innerDims.Width - Icon.Width.Pixels + 8f) / measuredSize.X;
            }
            OverclockName = new(ThisOverclock.DisplayName, textScale: nameScale) {
                IgnoresMouseInteraction = true,
                Left = { Pixels = Icon.Width.Pixels - 4f }
            };
            Append(OverclockName);
        }
    }
}
