using Terraria;
using Terraria.ModLoader;
using System;
using MonoMod.Utils;

namespace deeprockitems.UI.UpgradeUI
{
    public class BlockChestDrawing : ModSystem
    {
        public override void Load() {
            On_Main.DrawTrashItemSlot += On_Main_DrawTrashItemSlot;
        }
        private void OffsetUI(Action<int, int> orig) {
            int offX = 0;
            int offY = 0;
            // If my ui is open, move icon
            if (UpgradeSystem.IsUIOpen)
            {
                offY = (int)UpgradeState.MenuSize.Y;
            }
            //pivotTopLeftX = (int)((455 + pivotTopLeftX) - 56f * Main.inventoryScale * 2f);
            orig(offX, offY);
        }
        private void On_Main_DrawTrashItemSlot(On_Main.orig_DrawTrashItemSlot orig, int pivotTopLeftX, int pivotTopLeftY) {
            OffsetUI(orig.CastDelegate<Action<int, int>>());
        }
    }
}
