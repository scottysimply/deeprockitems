using deeprockitems.UI.UpgradeUI;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI
{
    public class ShiftClickIntoFakeSlot : ModPlayer
    {
        UpgradeSystem upgradeSystem;
        bool canShiftIn = false;
        public override void Initialize()
        {
            upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
        }
        public override void PreUpdate()
        {
            canShiftIn = false;
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            if (inventory[slot].type == 0) return false;
            if (ItemSlot.ShiftInUse && !ItemSlot.ShiftForcedOn && upgradeSystem.Interface.CurrentState != null && (upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.PredicateToPutItemIn(inventory[slot], upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.ItemInSlot) ?? false))
            {
                Main.cursorOverride = 9;
                canShiftIn = true;
                return true;
            }
            return false;
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (canShiftIn)
            {
                Item tempItem = upgradeSystem.UpgradeUIState.ItemInSlot;
                upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.SwapItems(ref inventory[slot], ref tempItem);
                upgradeSystem.UpgradeUIState.ItemInSlot = tempItem;
                return true;
            }
            return false;
        }
    }
}
