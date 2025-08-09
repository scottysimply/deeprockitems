using deeprockitems.UI.UpgradeUI;
using deeprockitems.UI.UpgradeUI.OverclockUI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI
{
    public class ShiftClickIntoFakeSlot : ModPlayer
    {
        UpgradeSystem upgradeSystem;
        FakeItemSlot parentSlot;
        FakeItemSlot overclockSlot;
        bool shiftToParent = false;
        bool shiftToOverclock = false;
        int shiftableSlot = -1;
        public override void Initialize()
        {
            // Used to get the UI instance
            upgradeSystem = ModContent.GetInstance<UpgradeSystem>();
        }
        public override void PreUpdate()
        {
            shiftToParent = false;
            shiftToOverclock = false;
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            if (inventory[slot].type == 0) return false;
            if (ItemSlot.ShiftInUse && !ItemSlot.ShiftForcedOn && upgradeSystem.Interface.CurrentState != null)
            {
                var selectedPanel = upgradeSystem.UpgradeUIState.Panel.SelectedPanel;
                if (selectedPanel is null) return false;
                // Check parent slot
                if (selectedPanel.ParentSlot.PredicateToPutItemIn(inventory[slot], upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.ItemInSlot))
                {
                    Main.cursorOverride = 9;
                    shiftToParent = true;
                    return true;
                }
                // check overclock slot
                if (selectedPanel is OverclockPanel ocPanel && ocPanel.MatrixCoreSlot.PredicateToPutItemIn(inventory[slot], upgradeSystem.UpgradeUIState.Panel.SelectedPanel?.ParentSlot.ItemInSlot))
                {
                    Main.cursorOverride = 9;
                    shiftToOverclock = true;
                    return true;
                }
            }
            return false;
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (shiftToParent && upgradeSystem.UpgradeUIState.Panel.SelectedPanel is not null)
            {
                Item tempItem = upgradeSystem.UpgradeUIState.Panel.SelectedPanel.ParentSlot.ItemInSlot;
                upgradeSystem.UpgradeUIState.Panel.SelectedPanel.ParentSlot.SwapItems(ref inventory[slot], ref tempItem);
                upgradeSystem.UpgradeUIState.Panel.SelectedPanel.ParentSlot.ItemInSlot = tempItem;
                return true;
            }
            else if (shiftToOverclock && upgradeSystem.UpgradeUIState.Panel.SelectedPanel is OverclockPanel panel)
            {
                Item tempItem = panel.MatrixCoreSlot.ItemInSlot;
                panel.MatrixCoreSlot.SwapItems(ref inventory[slot], ref tempItem);
                panel.MatrixCoreSlot.ItemInSlot = tempItem;
                return true;
            }
            return false;
        }
    }
}
