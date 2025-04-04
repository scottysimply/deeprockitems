using deeprockitems.Content.Upgrades;
using deeprockitems.UI.Components;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace deeprockitems.UI.UpgradeUI
{
    public class OverclockMenu : UIPanel
    {
        public UIScrollbar Scrollbar { get; set; }
        public UIText OverclockLabel { get; set; }
        public UIList OverclockList { get; set; }
        public override void OnInitialize() {
            base.OnInitialize();
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclocks", () => "Overclocks"), textScale: 0.66f) {
                Left = { Percent = 0f, Pixels = -4f }
            };
            Append(OverclockLabel);
            Scrollbar = new UIScrollbar {
                Left = { Percent = 1f, Pixels = -14f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.Height.Pixels },
                Top = { Pixels = OverclockLabel.Height.Pixels }
            };
            OverclockList = new UIList {
                Width = { Pixels = -20, Percent = 1f },
                Left = { Percent = 0f, Pixels = 20f },
                Top = { Pixels = OverclockLabel.Height.Pixels },
                Height = { Percent = 1f, Pixels = -OverclockLabel.Height.Pixels},
            };
            OverclockList.SetScrollbar(Scrollbar);
            Append(Scrollbar);
            Append(OverclockList);
        }
        public void SetOverclocks(UpgradeTier tier) {
            if (tier is null || tier.Tier != UpgradeBuilder.OVERCLOCK_TIER)
            {
                OverclockList.Clear();
                return;
            }
            // Generate overclock elements from tier
            var list_of_elements = tier.Select<Upgrade, OverclockListItem>(upgrade => new(upgrade as Overclock));
            OverclockList.AddRange(list_of_elements);
        }
        public void RemoveOverclocks() {
            OverclockList = null;
        }
    }
}
