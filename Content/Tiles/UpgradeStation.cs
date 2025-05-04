using deeprockitems.UI.UpgradeUI;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace deeprockitems.Content.Tiles
{
    public class UpgradeStation : ModTile
    {
        const int HEIGHT = 3;
        const int WIDTH = 3;
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
        }
        public override void MouseOver(int i, int j) {
            Main.LocalPlayer.cursorItemIconEnabled = true;
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<UpgradeStationItem>();
            Main.LocalPlayer.noThrow = 2;
        }
        public override bool RightClick(int i, int j)
        {
            // Open UI
            UpgradeSystem system = ModContent.GetInstance<UpgradeSystem>();
            if (system.UpgradeUIState == null)
            {
                Main.LocalPlayer.chest = -1;
                Main.LocalPlayer.sign = -1;
                UpgradeUIPlayer.UpgradeStationLocation = new(i, j);
                Main.playerInventory = true;
                UpgradeSystem.OpenUpgradeInterface();
            }
            else // Set state
            {
                UpgradeSystem.CloseUpgradeInterface();
            }
            return true;
        }
    }
}
