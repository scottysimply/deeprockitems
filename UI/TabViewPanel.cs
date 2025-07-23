using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace deeprockitems.UI
{
    public class TabViewPanel<TPanel> : UIElement where TPanel : UIPanel {
        private string _selectedText = "";
        private Dictionary<string, TPanel> _views;
        private bool _needsRevalidate = false;
        public TabViewPanel() {
            _views = [];
        }
#nullable enable
        public TPanel? SelectedPanel
        {
            get
            {
                if (_views.TryGetValue(_selectedText, out TPanel? panel))
                {
                    return panel;
                }
                return null;
            }
        }
#nullable disable
        public SoundStyle SwitchTabSound { get; set; } = SoundID.MenuTick;
        public Color BorderColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = new Color(63, 82, 151) * 0.7f;
        public float LabelHeight { get; private set; }

        public Asset<DynamicSpriteFont> Font { get; private set; } = FontAssets.MouseText;
        public void SetFont(Asset<DynamicSpriteFont> font) {
            Font = font;
            _needsRevalidate = true;
        }
        public void SetLabelHeight(float labelHeight) {
            LabelHeight = labelHeight;
            _needsRevalidate = true;
        }
        public void AddPanel(LocalizedText label, TPanel panel) {
            AddPanel(label.Value, panel);
        }
        public void AddPanel(string label, TPanel panel) {
            _views.Add(label, panel);
            _needsRevalidate = true;
        }
        private void ValidateLabelsAndPanel() {
            // Remove existing labels/panels
            List<UIElement> newChildren = [..Children];
            foreach (var child in newChildren)
            {
                if (child is TabLabel label)
                {
                    label.IsSelected = false;
                    RemoveChild(child);
                    child.Deactivate();
                }
                else if (child is UIPanel)
                {
                    RemoveChild(child);
                    child.Deactivate();
                }
            }

            // Create new labels
            float tabX = 20f;
            foreach ((string text, UIPanel panel) in _views)
            {
                TabLabel label = new TabLabel(text) {
                    Left = { Pixels = tabX},
                    Height = { Pixels = LabelHeight },
                    Top = { Pixels = 2f }
                };
                label.IsSelected = false;
                label.BackgroundColor = BackgroundColor;
                label.BorderColor = BorderColor;
                // Makes sure the panel stays selected
                if ((_selectedText ?? "") == text)
                {
                    label.IsSelected = true;
                }
                Append(label);
                label.Activate();
                tabX += label.Width.Pixels + 8f;
            }

            // Check panel
            ValidatePanel();
        }
        private void ValidatePanel() {
            // Append panel directly below labels:
            SelectedPanel.BackgroundColor = BackgroundColor;
            SelectedPanel.BorderColor = BorderColor;
            SelectedPanel.Top.Pixels = LabelHeight;
            SelectedPanel.Height.Pixels = GetInnerDimensions().Height - LabelHeight;
            Append(SelectedPanel);
            SelectedPanel.Activate();
        }
        public override void OnInitialize() {
            _selectedText = _views.Keys.First();
            ValidateLabelsAndPanel();
        }
        public override void Update(GameTime gameTime) {
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (_needsRevalidate)
            {
                ValidateLabelsAndPanel();
                _needsRevalidate = false;
            }
            base.Update(gameTime);
        }
        public override void LeftMouseDown(UIMouseEvent evt) {
            if (evt.Target is TabLabel label && label.Text != _selectedText)
            {
                // Deselect all others, select this one
                foreach (var child in Children)
                {
                    if (child is TabLabel childLabel)
                    {
                        childLabel.IsSelected = false;
                    }
                }
                label.IsSelected = true;
                _selectedText = label.Text;
                _needsRevalidate = true;

                SoundEngine.PlaySound(SwitchTabSound);
            }
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Separate children into their own parts
            List<TabLabel> labels = [];
            List<UIElement> otherChildren = [];
            foreach (UIElement child in Children)
            {
                if (child is TabLabel label)
                {
                    labels.Add(label);
                    continue;
                }
                else if (child == SelectedPanel)
                {
                    continue;
                }
                otherChildren.Add(child);
            }

            TabLabel selectedLabel = null;
            foreach (TabLabel label in labels)
            {
                // save the selected label for last
                if (label.IsSelected)
                {
                    selectedLabel = label;
                    continue;
                }
                // normal label
                label.Draw(spriteBatch);
            }

            DepthStencilState panelStencil = new() {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                StencilWriteMask = 255,
                DepthBufferEnable = false,
            };
            DepthStencilState tabMaskStencil = new() {
                StencilEnable = true,
                StencilFunction = CompareFunction.NotEqual,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };
            DepthFormat oldFormat = Main.graphics.PreferredDepthStencilFormat;
            Main.graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            // brute force clearing the stencil buffer because fuck terraria
            spriteBatch.End();
            spriteBatch.BeginWithDefaultsForUI(blendState: new() {
                ColorWriteChannels = ColorWriteChannels.None
            }, stencilState: new() {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 0,
                StencilMask = 0xFF
            });
            spriteBatch.Draw(Assets.WhitePixel.Value,
                new Rectangle(0, 0, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight),
                Color.White);
            spriteBatch.End();

            // draw the mask (1994)
            spriteBatch.BeginWithDefaultsForUI(stencilState: panelStencil, blendState: new() { ColorWriteChannels = ColorWriteChannels.None });
            spriteBatch.Draw(Assets.WhitePixel.Value, selectedLabel.GetDimensions().ToRectangle(), Color.White);
            spriteBatch.End();

            // panel with stencil
            spriteBatch.BeginWithDefaultsForUI(stencilState: tabMaskStencil);
            SelectedPanel.Draw(spriteBatch);
            spriteBatch.End();

            // drawing the tab for realsies :D
            spriteBatch.BeginWithDefaultsForUI();
            selectedLabel?.Draw(spriteBatch);

            Main.graphics.PreferredDepthStencilFormat = oldFormat;
            // draw normal children after stenciling
            foreach (var child in otherChildren)
            {
                child.Draw(spriteBatch);
            }
        }
        protected class TabLabel : UIElement {
            private float _labelHeight;
            public Color BorderColor = Color.Black;
            public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
            public TabLabel(LocalizedText text) : this(text.Value) {

            }
            public TabLabel(string text) {
                Text = text;
            }
            private Color AdjustedBorderColor
            {
                get
                {
                    if (IsSelected)
                    {
                        return BorderColor;
                    }
                    float dimPercent = 0.66f;
                    return new Color((int)(BorderColor.R * dimPercent), (int)(BorderColor.G * dimPercent), (int)(BorderColor.B * dimPercent), BackgroundColor.A);
                }
            }
            private Color AdjustedBackgroundColor 
            {
                get
                {
                    if (IsSelected)
                    {
                        return BackgroundColor;
                    }
                    float dimPercent = 0.66f;
                    return new Color((int)(BackgroundColor.R * dimPercent), (int)(BackgroundColor.G * dimPercent), (int)(BackgroundColor.B * dimPercent), BackgroundColor.A);
                }
            }
            public bool IsSelected { get; set; }
            public string Text { get; private set; }
            public Asset<DynamicSpriteFont> Font { get; set; } = FontAssets.MouseText;
            public override void OnInitialize() {
                Vector2 baseSize = ChatManager.GetStringSize(Font.Value, Text, new(1f));
                float scale = GetInnerDimensions().Height / baseSize.Y;
                Width.Pixels = baseSize.X * scale + 30f;
                Recalculate();
            }
            public override void Draw(SpriteBatch spriteBatch) {
                var bounds = GetDimensions();
                DRGHelpers.DrawPanel(spriteBatch, Assets.UI.TabLabelFill.Value, 18, 10, new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, AdjustedBackgroundColor);
                DRGHelpers.DrawPanel(spriteBatch, Assets.UI.TabLabelOutline.Value, 18, 10, new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, AdjustedBorderColor);
                // Draw font centered
                Vector2 baseSize = ChatManager.GetStringSize(Font.Value, Text, new(1f));
                float scale = (GetInnerDimensions().Height - 4f) / baseSize.Y;
                Vector2 adjustedCenter = GetDimensions().Center() + new Vector2(0f, 2f);
                Utils.DrawBorderString(spriteBatch, Text, adjustedCenter - 0.5f * scale * baseSize, Color.White, scale);
            }
        }
    }
}
