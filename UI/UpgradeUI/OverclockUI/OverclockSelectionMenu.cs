using deeprockitems.Content.Items;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
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
                Width = { Pixels = 220f },
                MinWidth = { Pixels = 220f},
                Top = { Pixels = OverclockLabel.GetDimensions().Height + 4f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.GetDimensions().Height },
            };
            OverclockList.OnLeftClick += OverclockList_OnLeftClick;
            OverclockList.SetScrollbar(Scrollbar);
            Append(OverclockList);
            Append(Scrollbar);
        }
        private void OverclockList_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            if (evt.Target is OverclockListItem target)
            {
                SelectedOverclock.ThisOverclock = target.ThisOverclock;
                (Parent as OverclockPanel).SelectedOverclock.ThisOverclock = target.ThisOverclock;
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
            OverclockList.OverflowHidden = true;
        }
        public void RemoveOverclocks() {
            OverclockList = null;
        }
        private static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState {
            CullMode = CullMode.None,
            ScissorTestEnable = true
        };
        protected override void DrawChildren(SpriteBatch spriteBatch) {
            var oldRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            var oldRasterizer = spriteBatch.GraphicsDevice.RasterizerState;
            var oldClamp = spriteBatch.GraphicsDevice.SamplerStates[0];
            foreach (var element in Children)
            {
                if (element is not UIList list)
                {
                    element.Draw(spriteBatch);
                    continue;
                }
                // End current spritebatch; begin with new one
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, oldClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
                spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(spriteBatch), oldRect);
                list.Draw(spriteBatch);
                // Reset spriteBatch
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, oldClamp, DepthStencilState.None, oldRasterizer, null, Main.UIScaleMatrix);
                spriteBatch.GraphicsDevice.ScissorRectangle = oldRect;
            }
        }
    }
}
