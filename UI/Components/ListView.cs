using deeprockitems.UI.UpgradeUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace deeprockitems.UI.Components
{
    /// <summary>
    /// Defines a ListView object that displays an object.
    /// </summary>
    public class ListView<T> : UIElement
    {
        T[] _data;
        float _maxHeight = 0;
        Func<T, UIElement> _creationDelegate;
        UIElement[] _childElements;
        float _verticalScrollPosition = 0;
        const float PADDING = 4f;
        const int SCROLL_THRESHOLD = 16;
        int _oldScrollValue;
        UIScrollbar _scrollBar;
        public ListView(T[] data, Func<T, UIElement> creationDelegate) {
            _data = data;
            _creationDelegate = creationDelegate;
            _childElements = new UIElement[data.Length];
        }
        public override void OnInitialize() {
            _scrollBar = new();
            _scrollBar.Width.Pixels = 10f;
            _scrollBar.Left.Percent = 1f;
            _scrollBar.OnScrollWheel += onScrolling;
            OverflowHidden = true;
            for (int i = 0; i < _data.Length; i++)
            {
                var element = _creationDelegate(_data[i]);
                _maxHeight += element.Height.Pixels + element.PaddingBottom;
                element.Top.Pixels = i * (element.Height.Pixels + 4f);
                _childElements[i] = element;
                Append(element);
            }
            _scrollBar.SetView(Height.Pixels, _maxHeight);
            Append(_scrollBar);
        }

        private void onScrolling(UIScrollWheelEvent evt, UIElement listeningElement) {
            if (IsMouseHovering)
            {
                Main.NewText(_maxHeight);
            }
        }

        public override void Update(GameTime gameTime) {
            //if (IsMouseHovering)
            //{
            //    PlayerInput.LockVanillaMouseScroll("deeprockitems/ListView");
            //}
            base.Update(gameTime);
        }
    }
}
