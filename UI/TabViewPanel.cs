using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
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
        public float LabelHeight { get; private set; }
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
                otherChildren.Add(child);
            }
            // Order will go labels -> other children -> the selected label
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
            // draw normal children
            foreach (var child in otherChildren)
            {
                child.Draw(spriteBatch);
            }
            // draw selected last (over everything else)
            selectedLabel?.Draw(spriteBatch);
        }
        protected class TabLabel : UIElement {
            private float _labelHeight;
            public Color BorderColor = Color.Black;
            public TabLabel(LocalizedText text) : this(text.Value) {

            }
            public TabLabel(string text) {
                Text = text;
            }
            private Color BackgroundColor 
            {
                get
                {
                    if (IsSelected)
                    {
                        return new Color(63, 82, 151) * 0.7f;
                    }
                    float dimPercent = 0.66f;
                    return new Color((int)(63 * dimPercent), (int)(82 * dimPercent), (int)(151 * dimPercent)) * 0.8f;
                }
            }
            public bool IsSelected { get; set; }
            public string Text { get; private set; }
            public UIText LabelText { get; set; }
            public override void OnInitialize() {
                Vector2 baseSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, Text, new(1f));
                float scale = GetInnerDimensions().Height / baseSize.Y;
                LabelText = new(Text, scale) {
                    PaddingLeft = 18,
                    PaddingRight = 18,
                    Top = { Pixels = 6}
                };
                LabelText.IgnoresMouseInteraction = true;
                Width.Pixels = baseSize.X * scale + 36;
                Append(LabelText);
            }
            protected override void DrawSelf(SpriteBatch spriteBatch) {
                var bounds = GetDimensions();
                DrawTab(spriteBatch, Assets.UI.TabLabelFill.Value, 20, 12, new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, BackgroundColor);
                DrawTab(spriteBatch, Assets.UI.TabLabelOutline.Value, 20, 12, new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, BorderColor);
            }
            /// <summary>
            /// Adapted from DRGHelpers.DrawPanel.
            /// </summary>
            public void DrawTab(SpriteBatch spriteBatch, Texture2D texture, int insetWidth, int insetHeight, Vector2 position, float width, float height, Color color) {
                // Draw each edge
                var dimensions = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
                // Left
                spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + insetHeight, insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(0, insetHeight, insetWidth, texture.Height - 2 * insetHeight), color);
                // Right
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y + insetHeight, insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(texture.Width - insetWidth, insetHeight, insetWidth, texture.Height - 2 * insetHeight), color);
                // Top
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y, dimensions.Width - 2 * insetWidth, insetHeight), new Rectangle(insetWidth, 0, texture.Width - 2 * insetWidth, insetHeight), color);
                // bottom is omitted for tab sake
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y + dimensions.Height - insetHeight, dimensions.Width - 2 * insetWidth, insetHeight), new Rectangle(insetWidth, texture.Height - insetHeight, texture.Width - 2 * insetWidth, insetHeight), color);
                // Draw corners, top left
                spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, insetWidth, insetHeight), new Rectangle(0, 0, insetWidth, insetHeight), color);
                // top right
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y, insetWidth, insetHeight), new Rectangle(texture.Width - insetWidth, 0, insetWidth, insetHeight), color);
                // bottom left
                spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height - insetHeight, insetWidth, insetHeight), new Rectangle(0, texture.Height - insetHeight, insetWidth, insetHeight), color);
                // bottom right
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width - insetWidth, dimensions.Y + dimensions.Height - insetHeight, insetWidth, insetHeight), new Rectangle(texture.Width - insetWidth, texture.Height - insetHeight, insetWidth, insetHeight), color);

                // Draw center
                spriteBatch.Draw(texture, new Rectangle(dimensions.X + insetWidth, dimensions.Y + insetHeight, dimensions.Width - 2 * insetWidth, dimensions.Height - 2 * insetHeight), new Rectangle(insetWidth, insetHeight, texture.Width - 2 * insetWidth, texture.Height - 2 * insetHeight), color);
            }
        }
    }
}
