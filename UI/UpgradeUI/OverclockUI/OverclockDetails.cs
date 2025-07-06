using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockDetails : UIPanel {
        private Item _item;
        public UIText OverclockName { get; set; }
        public UIText OverclockDescription { get; set; }
        public UIText Positives { get; set; }
        public UIText Negatives { get; set; }
        public UIButton<LocalizedText> EquipButtton { get; set; }
        public UIButton<LocalizedText> ViewUpgradesButton { get; set; }
        public override void OnInitialize() {
            (Parent as OverclockPanel).SelectedOverclock.OnValueChanged += SelectedOverclock_OnValueChanged;
            OverflowHidden = true;
        }

        private void SelectedOverclock_OnValueChanged(Overclock newValue, Overclock oldValue) {
            RemoveAllChildren();
            float innerWidth = OverclockPanel.DesiredSelectedWidth - PaddingLeft - PaddingRight - MarginLeft - MarginRight;
            newValue.DisplayName.ScaleText(innerWidth, out var nameScale, out var nameSize);
            OverclockName = new(newValue.DisplayName, nameScale) {
                Left = { Pixels = 0.5f * innerWidth - 0.5f * nameSize.X * nameScale },
                Top = { Pixels = 3f },
            };
            Append(OverclockName);
            newValue.HoverText.ScaleText(innerWidth, out var descScale, out var descSize);
            OverclockDescription = new(newValue.HoverText, descScale) {
                Top = { Pixels = OverclockName.GetDimensions().Height + 3 }
            };
            Append(OverclockDescription);
            Positives = new(newValue.Positives) {
                Top = { Pixels = OverclockDescription.GetDimensions().Height + 3 }
            };
            Append(Positives);
            if (newValue.Negatives.Value != newValue.Negatives.Key)
            {
                Negatives = new(newValue.Negatives) {
                    Top = { Pixels = OverclockDescription.GetDimensions().Height + 3 }
                };
                Append(Negatives);
            }
        }
    }
}
