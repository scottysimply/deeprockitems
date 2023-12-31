using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Content.Items.Weapons;
using Terraria.UI;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.UI.UpgradeItem;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual bool IsOverclock { get; set; }   
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = Item.height = 30;
            Item.value = IsOverclock ? Item.buyPrice(0, 5, 0, 0) : Item.buyPrice(0, 3, 0, 0);
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
        private uint _drawTimer = 0;
        /*public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            _drawTimer++;
            float color_shifter = CycleColorOnTimer(_drawTimer, 0.5f, 1f);
            // Check if the upgrade UI is open:
            if (UpgradeUISystem.Interface != null && UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem is UpgradeableItemTemplate modItem)
            {
                // Check if this upgrade is a valid weapon
                if (modItem.ValidUpgrades.Contains(Item.type) && !modItem.Upgrades.Contains(Item.type))
                {
                    // Redraw the slot with varying color.
                    int slot = 7;
                    ItemSlot.Draw(spriteBatch, Main.LocalPlayer.inventory, ItemSlot.Context.ChestItem, slot, position);
                }
            }
            return true;
        }
        /// <summary>
        /// Acts kind of like a lerp between a min and max brightness.
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private static float CycleColorOnTimer(uint currentTime, float min, float max)
        {
            const int CHANGE_TIME = 75;
            const int HOLD_TIME = 25;

            int one_cycle = 2 * CHANGE_TIME + 2 * HOLD_TIME;
            
            // First holding time. Darker appearance
            if (currentTime % one_cycle < HOLD_TIME)
            {
                return min;
            }
            // Lighten up!
            else if (currentTime % one_cycle < HOLD_TIME + CHANGE_TIME)
            {
                return (currentTime % one_cycle) / one_cycle * max;
            }
            // Hold lighter color
            else if (currentTime % one_cycle < HOLD_TIME + CHANGE_TIME)
            {
                return max;
            }
            // Fluctuate back to darkness again
            else
            {
                return one_cycle / (currentTime % one_cycle)* min;
            }
        }*/
    }
}