using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace deeprockitems.UI
{
    public class TabViewPanel : UIElement {
        private string _selectedText = "";
        private Dictionary<string, UIPanel> _views;
        private bool _needsRevalidate = false;
        public TabViewPanel(float labelHeight) {
            LabelHeight = labelHeight;
            _views = [];
        }
        private UIPanel SelectedPanel
        {
            get
            {
                if (_views.TryGetValue(_selectedText, out UIPanel panel))
                {
                    return panel;
                }
                return new UIPanel();
            }
        }
        public float LabelHeight { get; private set; }
        public void AddPanel(LocalizedText label, UIPanel panel) {
            AddPanel(label.Value, panel);
        }
        public void AddPanel(string label, UIPanel panel) {
            _views.Add(label, panel);
            _needsRevalidate = true;
        }
        private void ValidateLabelsAndPanel() {
            // Remove existing labels/panels
            foreach (var child in Children)
            {
                if (child is TabLabel label)
                {
                    label.IsSelected = false;
                    RemoveChild(child);
                }
                else if (child is UIPanel)
                {
                    RemoveChild(child);
                }
            }

            // Create new labels
            float tabX = 0f;
            foreach ((string text, UIPanel panel) in _views)
            {
                TabLabel label = new TabLabel(text) {
                    Left = { Pixels = tabX},
                    Height = { Pixels = LabelHeight }
                };
                Append(label);
                label.IsSelected = true;
                tabX += label.Width.Pixels;
            }

            // Check panel
            ValidatePanel();
        }
        private void ValidatePanel() {
            // Append panel directly below labels:
            SelectedPanel.Top.Pixels = LabelHeight;
            SelectedPanel.Height.Pixels = GetInnerDimensions().Height - LabelHeight;
            Append(SelectedPanel);
        }
        public override void OnInitialize() {
            ValidateLabelsAndPanel();
        }
        public override void Update(GameTime gameTime) {
            if (_needsRevalidate)
            {
                ValidateLabelsAndPanel();
                _needsRevalidate = false;
            }
        }
        public override void LeftMouseDown(UIMouseEvent evt) {
            if (evt.Target is TabLabel label)
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
            }
        }
        public override void Draw(SpriteBatch spriteBatch) {
            // Draw panels first
            List<TabLabel> labels = [];
            foreach (UIElement child in Children)
            {
                // Exclude labels for now
                if (child is TabLabel label)
                {
                    labels.Add(label);
                    continue;
                }
                child.Draw(spriteBatch);
            }
            // draw selected label last
            TabLabel selectedLabel = null;
            foreach (TabLabel label in labels)
            {
                if (label.IsSelected)
                {
                    selectedLabel = label;
                    continue;
                }
                label.Draw(spriteBatch);
            }
            selectedLabel?.Draw(spriteBatch);
        }
        protected class TabLabel : UIElement {
            private float _labelHeight;
            private string _text;
            public Color BorderColor = Color.Black;
            public TabLabel(LocalizedText text) : this(text.Value) {

            }
            public TabLabel(string text) {
                _text = text;
            }
            private Color BackgroundColor 
            {
                get
                {
                    if (IsSelected)
                    {
                        return new Color(63, 82, 151) * 0.7f;
                    }
                    return new Color((int)(63 * 0.75f), (int)(82 * 0.75f), (int)(151 * 0.75f)) * 0.7f;
                }
            }
            public bool IsSelected { get; set; }
            public UIText LabelText { get; set; }
            public override void OnInitialize() {
                Vector2 baseSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, _text, new(1f));
                float scale = GetInnerDimensions().Height / baseSize.Y;
                LabelText = new(_text, scale);
                Width.Pixels = baseSize.X * scale;
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
                Rectangle dimensions = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
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
