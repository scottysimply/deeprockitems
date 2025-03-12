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
        Func<T, UIElement> _creationDelegate;
        UIElement[] _childElements;
        float _verticalScrollPosition = 0;
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
            _scrollBar.Left.Percent = 0.98f;
            _scrollBar.OnScrollWheel += onScrolling;
            Append(_scrollBar);
            for (int i = 0; i < _data.Length; i++)
            {
                var element = _creationDelegate(_data[i]);
                _childElements[i] = element;
                Append(element);
            }
        }

        private void onScrolling(UIScrollWheelEvent evt, UIElement listeningElement) {
            if (IsMouseHovering)
            {
                
            }
        }

        public override void Update(GameTime gameTime) {
            if (IsMouseHovering)
            {
                PlayerInput.LockVanillaMouseScroll("deeprockitems/ListView");
            }

        }
    }
}
