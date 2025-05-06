using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockSelectionMenu : UIPanel
    {
        public UIScrollbar Scrollbar { get; set; }
        public UIText OverclockLabel { get; set; }
        public UIList OverclockList { get; set; }
        public override void OnInitialize() {
            base.OnInitialize();
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclocks", () => "Overclocks"), textScale: 0.66f) {
                Left = { Percent = 0f, Pixels = -4f }
            };
            OverclockLabel.OnLeftClick += (UIMouseEvent evt, UIElement sender) => {
                ModContent.GetInstance<UpgradeSystem>().UpgradeUIState.SetState<OverclockPanel>();
            };
            Append(OverclockLabel);
            Scrollbar = new UIScrollbar {
                Width = { Pixels = 20f },
                Left = { Percent = 1f, Pixels = -14f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.Height.Pixels },
                Top = { Pixels = OverclockLabel.Height.Pixels }
            };
            OverclockList = new UIList {
                Width = { Pixels = -20, Percent = 1f },
                Top = { Pixels = OverclockLabel.GetDimensions().Height + 4f },
                Height = { Pixels = this.Height.Pixels - OverclockLabel.Height.Pixels - 30f },
                OverflowHidden = true,
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
            var list_of_elements = tier.Select<Upgrade, OverclockListItem>(upgrade => new(tier, upgrade as Overclock) {
                Height = { Pixels = 40f },
                Width = { Pixels = 40f },
                Left = { Pixels = 6f }
            });
            OverclockList.AddRange(list_of_elements);
        }
        protected override void DrawChildren(SpriteBatch spriteBatch) {
            base.DrawChildren(spriteBatch);
        }
        public void RemoveOverclocks() {
            OverclockList = null;
        }
    }
}
