using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace deeprockitems.UI.UpgradeUI
{
    public class ScrollbarDetour : ModSystem
    {
        public override void Load() {
            On_UIScrollbar.SetView += On_UIScrollbar_SetView;
        }

        private void On_UIScrollbar_SetView(On_UIScrollbar.orig_SetView orig, UIScrollbar self, float viewSize, float maxViewSize) {
            if (maxViewSize == 0)
            {
                maxViewSize = 1f;
            }
            orig(self, viewSize, maxViewSize);
        }
    }
}
