using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeUIPlayer : ModPlayer
    {
        public static Point UpgradeStationLocation;
        public static Item ItemInUpgradeSlot = new(0);
        public static Item ItemInMatrixSlot = new(0);
        public override void SetStaticDefaults() {
            _upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
        }
        public override void ResetEffects() {
            if ((Main.myPlayer == Player.whoAmI) && UpgradeStationLocation != new Point(-1, -1) && (!Player.IsInTileInteractionRange(UpgradeStationLocation.X, UpgradeStationLocation.Y, TileReachCheckSettings.Simple) || Player.chest != -1 || !Main.playerInventory || Player.talkNPC != -1)) {
                UpgradeSystem.CloseUpgradeInterface();
                UpgradeStationLocation = new Point(-1, -1);
            }
        }
        public override void OnEnterWorld() {
            // Give item to player
            if (ItemInUpgradeSlot != null && ItemInUpgradeSlot.type != ItemID.None)
            {
                Player.QuickSpawnItem(ItemInUpgradeSlot.GetSource_ReleaseEntity(), ItemInUpgradeSlot);
                ItemInUpgradeSlot = new(0);
            }
            if (ItemInMatrixSlot != null && ItemInMatrixSlot.type != ItemID.None)
            {
                Player.QuickSpawnItem(ItemInMatrixSlot.GetSource_ReleaseEntity(), ItemInMatrixSlot);
                ItemInMatrixSlot = new(0);
            }
        }
        private static UpgradeSystem _upgradeSystem;
        public Item ItemInSlot { 
            get => _upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.ItemInSlot;
            set
            {
                if (_upgradeSystem.UpgradeUIState.Panel.SelectedPanel is not null)
                {
                    _upgradeSystem.UpgradeUIState.Panel.SelectedPanel.ParentSlot.ItemInSlot = value;
                }
            }
        }
    }
}
