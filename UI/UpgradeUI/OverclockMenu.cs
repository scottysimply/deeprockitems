using deeprockitems.Content.Upgrades;
using deeprockitems.UI.Components;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace deeprockitems.UI.UpgradeUI
{
    public class OverclockMenu : UIPanel
    {
        public UIText OverclockLabel { get; set; }
        public ListView<Upgrade> OverclockList { get; set; }
        public OverclockMenu(string text_for_label) {
            OverclockLabel = new(text_for_label);
        }
        public void SetOverclocks(UpgradeTier tier) {
            OverclockList = new([..tier], (upgrade) => {
                UIPanel display = new();
                display.Width.Percent = 0.99f;
                UIText text = new UIText(upgrade.DisplayName);
                display.Height = text.MinHeight;
                display.Append(text);
                return display;
            });
        }
        public void RemoveOverclocks() {
            OverclockList = null;
        }
        public override void OnInitialize() {
            
        }
    }
}
