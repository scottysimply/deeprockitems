using deeprockitems.Content.Upgrades;
using deeprockitems.Localization;
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
using static AssGen.Assets.Upgrades;

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
        private static float SmallTextScale { get => 0.66f; }
        private void SelectedOverclock_OnValueChanged(Overclock newValue, Overclock oldValue) {
            RemoveAllChildren();
            float innerWidth = OverclockPanel.DesiredSelectedWidth - PaddingLeft - PaddingRight - MarginLeft - MarginRight - 2f;
            newValue.DisplayName.ScaleToFit(innerWidth, out var nameScale, out var nameSize);
            OverclockName = new(newValue.DisplayName, nameScale) {
                Left = { Pixels = 0.5f * innerWidth - 0.5f * nameSize.X * nameScale },
                Top = { Pixels = 3f },
                Height = { Pixels = nameSize.Y }
            };
            Append(OverclockName);
            string adjustedText = newValue.HoverText.ScaleThenSplit(SmallTextScale, innerWidth, out float smallScale, out Vector2 descSize);
            OverclockDescription = new(adjustedText, smallScale) {
                Top = { Pixels = OverclockName.GetDimensions().Height },
                Height = { Pixels = descSize.Y }
            };
            Append(OverclockDescription);
            // Create positives and negatives
            bool hasNegatives = newValue.Negatives.Key != newValue.Negatives.Value;
            int numSections = hasNegatives ? 2 : 1;
            float middlePadding = hasNegatives ? 8f : 0f;
            float cutoutForScroll = 24f;
            float sectionWidth = (OverclockPanel.DesiredSelectedWidth - middlePadding * (numSections - 1) - cutoutForScroll) / numSections;
            string testedPositives = "";
            // Prepare positives text
            foreach (var line in newValue.Positives.Value.Split('\n'))
            {
                testedPositives += "\n";
                testedPositives += $"▲ {line}";
            }
            testedPositives = testedPositives.Trim().SplitToFit(sectionWidth, SmallTextScale, out Vector2 positiveSize);
            // Color string
            string fixedPositives = "";
            foreach (var line in testedPositives.Split('\n'))
            {
                fixedPositives += '\n';
                fixedPositives += line.TextColor(DRGText.PositiveText);
            }
            Positives = new(fixedPositives.Trim(), SmallTextScale) {
                Top = { Pixels = OverclockDescription.GetDimensions().Height + 24f },
            };
            Append(Positives);
            if (hasNegatives)
            {
                string testedNegatives = "";
                // Prepare negatives
                foreach (var line in newValue.Negatives.Value.Split('\n'))
                {
                    testedNegatives += "\n";
                    testedNegatives += $"▼ {line}";
                }
                testedNegatives = testedNegatives.Trim().SplitToFit(sectionWidth, SmallTextScale, out _);
                string fixedNegatives = "";
                foreach (var line in testedNegatives.Split('\n'))
                {
                    fixedNegatives += '\n';
                    fixedNegatives += line.TextColor(DRGText.NegativeText);
                }
                Negatives = new(fixedNegatives.Trim(), SmallTextScale) {
                    Left = { Pixels = positiveSize.X + middlePadding },
                    Top = { Pixels = OverclockDescription.GetDimensions().Height + 24f }
                };
                Append(Negatives);
            }
        }
    }
}
