/*using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.UI;
using Terraria.UI;
using Terraria.Audio;
using Terraria.ID;
using System.Linq;

namespace deeprockitems.Common.Overides
{
    public class ShiftClickUpgrades : ModPlayer
    {
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            // If item is upgradetemplate and is in inventory
            if (inventory[slot].ModItem is UpgradeTemplate && context == ItemSlot.Context.InventoryItem)
            {
                if (ShiftClickIntoSlot(inventory[slot]))
                {
                    // Delete item in inventory
                    inventory[slot].TurnToAir();
                    inventory[slot].maxStack = 0;
                    SoundEngine.PlaySound(SoundID.Grab);
                    return true;
                }
            }
            Main.NewText(UpgradeUIState.overclockSlot.ItemInSlot);
            return base.ShiftClickSlot(inventory, context, slot);
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            if (inventory[slot].ModItem is UpgradeTemplate && context == ItemSlot.Context.InventoryItem)
            {
                if (ItemSlot.ShiftInUse)
                {
                    Main.cursorOverride = CursorOverrideID.FavoriteStar;
                    return true;
                }
            }
            return base.HoverSlot(inventory, context, slot);
        }
        private bool ShiftClickIntoSlot(Item item)
        {
            if (item.ModItem is UpgradeTemplate upgrade && UpgradeSlot.ParentItem.ValidUpgrades.Contains(upgrade.Type))
            {
                if (upgrade.IsOverclock)
                {
                    if (UpgradeUIState.overclockSlot.ItemInSlot.IsAir)
                    {
                        UpgradeUIState.overclockSlot.SaveItem_InSlot(upgrade, false);
                        UpgradeUIState.overclockSlot.ItemToDisplay = upgrade.Item;
                        return true;
                    }
                    return false;
                }
                if (UpgradeUIState.upgradeSlot1.ItemInSlot.IsAir)
                {
                    UpgradeUIState.upgradeSlot1.SaveItem_InSlot(upgrade, false);
                    UpgradeUIState.upgradeSlot1.ItemInSlot = upgrade.Item;
                    return true;
                }
                else if (UpgradeUIState.upgradeSlot2.ItemInSlot.IsAir)
                {
                    UpgradeUIState.upgradeSlot2.SaveItem_InSlot(upgrade, false);
                    UpgradeUIState.upgradeSlot2.ItemInSlot = upgrade.Item;
                    return true;
                }
                else if (UpgradeUIState.upgradeSlot3.ItemInSlot.IsAir)
                {
                    UpgradeUIState.upgradeSlot3.SaveItem_InSlot(upgrade, false);
                    UpgradeUIState.upgradeSlot3.ItemInSlot = upgrade.Item;
                    return true;
                }
            }
            Main.NewText("Item is not valid for slot");
            Main.NewText(item.ModItem is UpgradeTemplate);
            return false;

        }
    }
}
*/