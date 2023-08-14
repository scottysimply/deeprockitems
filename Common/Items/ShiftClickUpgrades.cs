using Terraria;
using Terraria.ModLoader;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.UI.UpgradeItem;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Humanizer;

namespace deeprockitems.Common.Items
{
    public class ShiftClickUpgrades : ModPlayer
    {
        private UpgradeUIState panel = UpgradeUISystem.UpgradeUIPanel;
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            UpgradeableItemTemplate Weapon = panel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate;
            if (inventory[slot].ModItem is UpgradeTemplate upgrade)
            {
                if (CanShiftClick(upgrade, Weapon))
                {
                    if (ItemSlot.ShiftInUse)
                    {
                        Main.cursorOverride = 30;
                        return true;
                    }
                }
            }
            return false;
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            UpgradeableItemTemplate Weapon = panel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate;
            if (inventory[slot].ModItem is UpgradeTemplate upgrade)
            {
                if (CanShiftClick(upgrade, Weapon))
                {
                    ShiftClickInventory(upgrade, Weapon);
                    inventory[slot] = new();
                    return true;
                }
            }
            return false;
        }
        private bool CanShiftClick(UpgradeTemplate item, UpgradeableItemTemplate parentItem)
        {
            if (parentItem is null) { return false; }
            // UI obviously has to be open.
            if (UpgradeUISystem.Interface.CurrentState is null)
            {
                return false;
            }
            // If item isn't a valid upgrade, it shouldn't be moved into the slot.
            if (!parentItem.ValidUpgrades.Contains(item.Type))
            {
                return false;
            }
            // Run overclock logic.
            if (item.IsOverclock)
            {
                // if Overclock == 0, then we can shift click into the slot :)
                if (parentItem.Overclock == 0)
                {
                    return true;
                }
                // Otherwise, return false. 
                return false;
            }

            bool duplicates = false;
            bool flag = false;
            // Item wasn't an overclock so that means we can run this code and it will work, guaranteed!
            for (int i = 0; i < parentItem.Upgrades2.Length; i++)
            {
                // First of all, check for duplicates
                if (parentItem.Upgrades2[i] == item.Type)
                {
                    duplicates = true;
                }
                if (parentItem.Upgrades2[i] == 0)
                {
                    flag = true;
                }
                
            }
            if (!duplicates && flag)
            {
                return true;
            }
            return false;

        }
        private void ShiftClickInventory(UpgradeTemplate item, UpgradeableItemTemplate parentItem)
        {
            if (item.IsOverclock)
            {
                if (parentItem.Overclock == 0)
                {
                    parentItem.Overclock = item.Type;
                }
            }
            else
            {
                for (int i = 0; i < parentItem.Upgrades2.Length; i++)
                {
                    if (parentItem.Upgrades2[i] == 0)
                    {
                        parentItem.Upgrades2[i] = item.Type;
                        break;
                    }
                }
            }

            parentItem.UpdateUpgrades();
            // We have no way to know which slot was which :(
            panel.upgradeSlot1.ItemToDisplay = new(parentItem.Upgrades2[0]);
            panel.upgradeSlot2.ItemToDisplay = new(parentItem.Upgrades2[1]);
            panel.upgradeSlot3.ItemToDisplay = new(parentItem.Upgrades2[2]);
            panel.overclockSlot.ItemToDisplay = new(parentItem.Overclock);
            SoundEngine.PlaySound(SoundID.Grab);
        }
    }
    public class CursorDetour : ModSystem
    {
        public override void Load()
        {
            On_Main.DrawInterface_36_Cursor += On_Main_DrawInterface_36_Cursor;
        }
        private void On_Main_DrawInterface_36_Cursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            if (Main.cursorOverride != 30)
            {
                orig(); // Call original method
            }
            else
            {
                Main.spriteBatch.End();
                Texture2D cursor = ModContent.Request<Texture2D>("deeprockitems/UI/UpgradeCursor").Value;

                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                Main.spriteBatch.Draw(cursor, Main.MouseScreen, null, Color.White, 0, new Vector2(.1f) * cursor.Size(), Main.cursorScale * 1.1f, SpriteEffects.None, 0f);
            }

        }
    }
}