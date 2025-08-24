using deeprockitems.Content.Items;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockSelectionMenu : UIPanel
    {
        private UpgradeTier _tier;
        public OverclockService SelectedOverclock { get; set; }
        public UIScrollbar Scrollbar { get; set; }
        public UIText OverclockLabel { get; set; }
        public UIList OverclockList { get; set; }
        public Color ChildBackgroundColor { get; set; } = Color.Transparent;
        public Color ChildBorderColor { get; set; } = Color.Transparent;
        public OverclockSelectionMenu(OverclockService overclock) {
            SelectedOverclock = overclock;
        }
        public override void OnInitialize() {
            base.OnInitialize();
            OverclockLabel = new(Language.GetOrRegister("Mods.deeprockitems.Misc.UsefulWords.Overclocks", () => "Overclocks"), textScale: 0.66f) {
                Left = { Percent = 0f, Pixels = -4f }
            };
            Append(OverclockLabel);
            Scrollbar = new ColorableScrollbar {
                Width = { Pixels = 20f },
                Left = { Percent = 1f, Pixels = -14f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.Height.Pixels },
                Top = { Pixels = OverclockLabel.Height.Pixels },
                ScrollbarColor = ChildBackgroundColor,
                InnerColor = ChildBorderColor,
            };
            OverclockList = new UIList {
                Width = { Pixels = 220f },
                MinWidth = { Pixels = 220f},
                Top = { Pixels = OverclockLabel.GetDimensions().Height + 4f },
                Height = { Percent = 1f, Pixels = -OverclockLabel.GetDimensions().Height },
            };
            OverclockList.OnLeftClick += OverclockList_OnLeftClick;
            OverclockList.OnLeftDoubleClick += OverclockList_OnLeftDoubleClick;
            OverclockList.SetScrollbar(Scrollbar);
            Append(OverclockList);
            Append(Scrollbar);
        }

        private void OverclockList_OnLeftDoubleClick(UIMouseEvent evt, UIElement listeningElement) {
            if (evt.Target is OverclockListItem target)
            {
                Overclock oc = target.ThisOverclock;
                oc.Tier.SelectUpgrade(oc.UpgradeName);
                ((Parent as OverclockPanel).ParentSlot.ItemInSlot.ModItem as IUpgradable).ApplyStatUpgrades();
                RefreshMenu();
                (Parent as OverclockPanel).Details.UpdateButtonText(oc);
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        private void OverclockList_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
            if (evt.Target is OverclockListItem target)
            {
                (Parent as OverclockPanel).CurrentlyViewedOverclock.ThisOverclock = target.ThisOverclock;
            }
        }
        public void RefreshMenu() {
            OverclockList.Clear();
            if (_tier is null || _tier.Tier != UpgradeBuilder.OVERCLOCK_TIER)
            {
                return;
            }
            // Generate overclock elements from tier
            var list_of_elements = _tier.Where(u => u is Overclock oc && oc.UpgradeState.IsUnlocked).Select((Upgrade upgrade) => {
                var element = new OverclockListItem(upgrade as Overclock) {
                    Width = { Percent = 1f, Pixels = -10f },
                    Height = { Pixels = 40f },
                    Left = { Pixels = 0f },
                };
                if (ChildBackgroundColor != Color.Transparent)
                {
                    element.BackgroundColor = ChildBackgroundColor;
                }
                if (ChildBorderColor != Color.Transparent)
                {
                    element.BorderColor = ChildBorderColor;
                }
                if (upgrade.UpgradeState.IsEquipped)
                {
                    element.BorderColor = UpgradePanel.SelectedContentBorderColor;
                }
                return element;
            });
            OverclockList.AddRange(list_of_elements);
            OverclockList.Activate();
            OverclockList.OverflowHidden = true;
        }
        public void SetOverclocks(UpgradeTier tier) {
            _tier = tier;
            RefreshMenu();
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
            // Allows for scissor rectangle
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
