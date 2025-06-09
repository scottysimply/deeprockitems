using deeprockitems.Content.Items;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockSelectionMenu : UIPanel
    {
        public OverclockService SelectedOverclock { get; set; }
        public UIScrollbar Scrollbar { get; set; }
        public UIText OverclockLabel { get; set; }
        public UIList OverclockList { get; set; }
        public OverclockSelectionMenu(OverclockService overclock) {
            SelectedOverclock = overclock;
        }
        public override void OnInitialize() {
            base.OnInitialize();
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclocks", () => "Overclocks"), textScale: 0.66f) {
                Left = { Percent = 0f, Pixels = -4f }
            };
            Append(OverclockLabel);
            Scrollbar = new UIScrollbar {
                Width = { Pixels = 20f },
                Left = { Percent = 1f, Pixels = -14f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.Height.Pixels },
                Top = { Pixels = OverclockLabel.Height.Pixels }
            };
            OverclockList = new UIList {
                Width = { Pixels = -20f, Percent = 1f },
                Top = { Pixels = OverclockLabel.GetDimensions().Height + 4f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.GetDimensions().Height },
            };
            OverclockList.OnLeftClick += OverclockList_OnLeftClick;
            OverclockList.SetScrollbar(Scrollbar);
            Append(Scrollbar);
            Append(OverclockList);
        }

        private void OverclockList_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            if (evt.Target is OverclockListItem target)
            {
                Main.NewText($"Selected {target.ThisOverclock.DisplayName}");
                SelectedOverclock.ThisOverclock = target.ThisOverclock;
                SelectedOverclock.ThisOverclock.Tier.SelectUpgrade(target.ThisOverclock.UpgradeName);
                (ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.ItemInSlot.ModItem as IUpgradable).ApplyStatUpgrades();
            }
        }

        public void SetOverclocks(UpgradeTier tier) {
            if (tier is null || tier.Tier != UpgradeBuilder.OVERCLOCK_TIER)
            {
                OverclockList.Clear();
                return;
            }
            // Generate overclock elements from tier
            var list_of_elements = tier.Select<Upgrade, OverclockListItem>(upgrade => new(upgrade as Overclock) {
                Width = { Percent = 1f, Pixels = -10f },
                Height = { Pixels = 40f },
                Left = { Pixels = 0f },
            });
            OverclockList.AddRange(list_of_elements);
            OverclockList.Activate();
        }
        public void RemoveOverclocks() {
            OverclockList = null;
        }
    }
}
