using deeprockitems.Content.Upgrades;
using deeprockitems.Localization;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using static AssGen.Assets.Upgrades;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockDetails : UIPanel {
        public class InnerElement : UIElement {

        }
        private Item _item;
        public UIText OverclockName { get; set; }
        public UIText OverclockDescription { get; set; }
        public UIText Positives { get; set; }
        public UIText Negatives { get; set; }
        public UIButton<LocalizedText> EquipButtton { get; set; }
        public UIButton<LocalizedText> ViewUpgradesButton { get; set; }
        private InnerElement Container { get; set; }
        public UIScrollbar ScrollBar { get; set; }
        public override void OnInitialize() {
            (Parent as OverclockPanel).SelectedOverclock.OnValueChanged += SelectedOverclock_OnValueChanged;
        }
        private static float SmallTextScale { get => 0.66f; }
        private void SelectedOverclock_OnValueChanged(Overclock newValue, Overclock oldValue) {
            RemoveAllChildren();
            float cutoutForScroll = 24f;
            float innerWidth = OverclockPanel.DesiredSelectedWidth - PaddingLeft - PaddingRight - MarginLeft - MarginRight - cutoutForScroll;
            // Create container
            Container = new() {
                Width = { Pixels = innerWidth },
                Height = { Pixels = GetInnerDimensions().Height },
                Left = { Pixels = 0 },
                Top = { Pixels = 0 },
                OverflowHidden = false
            };
            Append(Container);
            newValue.DisplayName.ScaleToFit(Container.Width.Pixels, out var nameScale, out var nameSize);
            OverclockName = new(newValue.DisplayName, nameScale) {
                Left = { Pixels = 0.5f * Container.Width.Pixels - 0.5f * nameSize.X * nameScale },
                Top = { Pixels = 3f },
                Height = { Pixels = nameSize.Y }
            };
            Container.Append(OverclockName);
            string adjustedText = newValue.HoverText.ScaleThenSplit(SmallTextScale, Container.Width.Pixels, out float smallScale, out Vector2 descSize);
            OverclockDescription = new(adjustedText, smallScale) {
                Top = { Pixels = OverclockName.Height.Pixels },
                Height = { Pixels = descSize.Y }
            };
            Container.Append(OverclockDescription);
            // Create positives and negatives
            bool hasNegatives = newValue.Negatives.Key != newValue.Negatives.Value;
            int numSections = hasNegatives ? 2 : 1;
            float middlePadding = hasNegatives ? 8f : 0f;
            float sectionWidth = (Container.Width.Pixels - middlePadding * (numSections - 1)) / numSections;
            string testedPositives = "";
            // Prepare positives text
            foreach (var line in newValue.Positives.Value.Split('\n'))
            {
                testedPositives += "\n";
                testedPositives += $"▲ {line}";
            }
            testedPositives = testedPositives.Trim().SplitToFit(sectionWidth, SmallTextScale, out Vector2 positiveSize, replaceWith: "\n    ");
            // Color string
            string fixedPositives = "";
            foreach (var line in testedPositives.Split('\n'))
            {
                fixedPositives += '\n';
                fixedPositives += line.TextColor(DRGText.PositiveText);
            }
            Positives = new(fixedPositives.Trim(), SmallTextScale) {
                Top = { Pixels = OverclockDescription.GetDimensions().Height + 24f },
                Height = { Pixels = positiveSize.Y }
            };
            Container.Append(Positives);
            if (hasNegatives)
            {
                string testedNegatives = "";
                // Prepare negatives
                foreach (var line in newValue.Negatives.Value.Split('\n'))
                {
                    testedNegatives += "\n";
                    testedNegatives += $"▼ {line}";
                }
                testedNegatives = testedNegatives.Trim().SplitToFit(sectionWidth, SmallTextScale, out Vector2 negativeSize, replaceWith: "\n    ");
                string fixedNegatives = "";
                foreach (var line in testedNegatives.Split('\n'))
                {
                    fixedNegatives += '\n';
                    fixedNegatives += line.TextColor(DRGText.NegativeText);
                }
                Negatives = new(fixedNegatives.Trim(), SmallTextScale) {
                    Left = { Pixels = positiveSize.X + middlePadding },
                    Top = { Pixels = OverclockDescription.GetDimensions().Height + 24f },
                    Height = { Pixels = negativeSize.Y }
                };
                Container.Append(Negatives);
            }
            // Add scrollbar
            _scrollBarHeight = Container.Height.Pixels;
            foreach (var element in Container.Children)
            {
                if (element.Top.Pixels + element.Height.Pixels > _scrollBarHeight)
                {
                    _scrollBarHeight = element.Top.Pixels + element.Height.Pixels;
                }
            }
            ScrollBar = new UIScrollbar {
                Width = { Pixels = 20f },
                Left = { Pixels = innerWidth + 6f },
                Height = { Percent = 1f, Pixels = -MarginTop * 2 },
                Top = { Pixels = MarginTop },
            };
            UpdateScrollBar();
            Append(ScrollBar);
        }
        private float _scrollBarHeight;
        public override void Recalculate() {
            UpdateScrollBar();
            base.Recalculate();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (IsMouseHovering && ScrollBar is not null)
            {
                PlayerInput.LockVanillaMouseScroll("deeprockitems/OverclockDetails");
            }
        }
        private void UpdateScrollBar() {
            if (ScrollBar is not null)
            {
                ScrollBar.SetView(Container.GetInnerDimensions().Height, _scrollBarHeight - 12f);
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (ScrollBar is not null)
            {
                Container.Top.Set(0f - ScrollBar.GetValue(), 0f);
                Recalculate();
            }
            base.DrawSelf(spriteBatch);
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
                if (element is not InnerElement container)
                {
                    element.Draw(spriteBatch);
                    continue;
                }
                // End current spritebatch; begin with new one
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, oldClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
                spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(spriteBatch), oldRect);
                container.Draw(spriteBatch);
                // Reset spriteBatch
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, oldClamp, DepthStencilState.None, oldRasterizer, null, Main.UIScaleMatrix);
                spriteBatch.GraphicsDevice.ScissorRectangle = oldRect;
            }
        }
        public override void ScrollWheel(UIScrollWheelEvent evt) {
            base.ScrollWheel(evt);
            if (ScrollBar is not null)
            {
                ScrollBar.ViewPosition -= evt.ScrollWheelValue;
            }
        }
    }
}
