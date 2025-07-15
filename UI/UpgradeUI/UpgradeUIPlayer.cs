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
            if (ItemToSpawnOnWorldLoad != null && ItemToSpawnOnWorldLoad.type != ItemID.None)
            {
                Player.QuickSpawnItem(ItemToSpawnOnWorldLoad.GetSource_ReleaseEntity(), ItemToSpawnOnWorldLoad);
            }
        }
        private static UpgradeSystem _upgradeSystem;
        public Item ItemToSpawnOnWorldLoad = new(0);
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
