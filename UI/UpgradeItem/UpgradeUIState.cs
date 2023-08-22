using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using System.Linq;
using System.Collections.Generic;

namespace deeprockitems.UI.UpgradeItem
{
    public class UpgradeUIState : UIState
    {
        public DragablePanel dragPanel;
        private const int OUTER_PADDING = 8;
        private const int INNER_PADDING = 4;
        private const int SLOT_SIZE = 40;
        public UpgradeSlot[] UpgradeSlots { get; set; }
        public DisplaySlot ParentSlot { get; set; }


        private const int SLOTS_WIDTH = 2; // How many slots should be in the width of the panel
        private const int SLOTS_HEIGHT = 2; // How many slots should be in the height of the panel

        public override void OnInitialize()
        {
            UpgradeSlots = new UpgradeSlot[SLOTS_WIDTH * SLOTS_HEIGHT];
            int total_width = 2 * OUTER_PADDING + (SLOTS_WIDTH - 1) * INNER_PADDING + SLOTS_WIDTH * SLOT_SIZE;
            int total_height = 2 * OUTER_PADDING + (SLOTS_HEIGHT) * INNER_PADDING + (SLOTS_HEIGHT + 1) * SLOT_SIZE;
            dragPanel = new();
            dragPanel.SetPadding(0);

            dragPanel.Left.Set(800, 0);
            dragPanel.Top.Set(100, 0);
            dragPanel.Width.Set(total_width, 0);
            dragPanel.Height.Set(total_height, 0);
            dragPanel.BackgroundColor = new Color(73, 94, 171);

            // Generate array and make the UI in one loop! It's like magic!
            for (int height = 0; height < SLOTS_HEIGHT; height++)
            {
                for (int width = 0; width < SLOTS_WIDTH; width++)
                {
                    int index = width + SLOTS_WIDTH * height;
                    UpgradeSlots[index] = new UpgradeSlot(index);

                    UpgradeSlots[index].Left.Set(OUTER_PADDING + width * (INNER_PADDING + SLOT_SIZE), 0);
                    UpgradeSlots[index].Top.Set(OUTER_PADDING + (height + 1) * (INNER_PADDING + SLOT_SIZE), 0);
                }
            }
            ParentSlot = new DisplaySlot(-1);
            ParentSlot.Left.Set((total_width - SLOT_SIZE) / 2, 0);
            ParentSlot.Top.Set(OUTER_PADDING, 0);
            ParentSlot.Width.Set(SLOT_SIZE, 0);
            ParentSlot.Height.Set(SLOT_SIZE, 0);
            dragPanel.Append(ParentSlot);

            foreach (var slot in UpgradeSlots)
            {
                slot.Width.Set(SLOT_SIZE, 0);
                slot.Height.Set(SLOT_SIZE, 0);
                dragPanel.Append(slot);
            }

            Append(dragPanel);
        }
        public override void Update(GameTime gameTime)
        {
            bool oldBlock = UpgradeUISystem.BlockItemSlotActionsDetour;
            if (dragPanel.IsMouseHovering)
            {
                UpgradeUISystem.BlockItemSlotActionsDetour = false;
            }
            base.Update(gameTime);
            UpgradeUISystem.BlockItemSlotActionsDetour = oldBlock;
        }
        public int[] GetUpgrades()
        {
            // if (Slots[0] is UpgradeSlot) { return new int[1]; }
            if (ParentSlot.ItemToDisplay.ModItem is UpgradeableItemTemplate parentItem)
            {
                return parentItem.Upgrades;
            }
            Main.NewText("The upgrade menu's parent item does not contain the TagCompound \"Upgrades\". Try resetting the world, or contact scotty if the issue persists.", Color.DarkOrange);
            return new int[1];
        }
        public List<int> GetValidUpgrades()
        {
            // if (Slots[0] is UpgradeSlot) { return new int[1]; }
            if (ParentSlot.ItemToDisplay.ModItem is UpgradeableItemTemplate parentItem)
            {
                return parentItem.ValidUpgrades;
            }
            Main.NewText("The upgrade menu's parent item does not contain the TagCompound \"Upgrades\". Try resetting the world, or contact scotty if the issue persists.", Color.DarkOrange);
            return new();
        }

        public void ClearItems()
        {
            foreach (DisplaySlot slot in UpgradeSlots)
            {
                slot.ItemToDisplay = new();
            }
            ParentSlot.ItemToDisplay = new();
        }
        public void ShowItems()
        {
            UpgradeableItemTemplate weapon = (UpgradeableItemTemplate)ParentSlot.ItemToDisplay.ModItem;
            for (int i = 0; i < UpgradeSlots.Length; i++)
            {
                if (weapon is not null)
                {
                    UpgradeSlots[i].ItemToDisplay = new(weapon.Upgrades[i]);
                    
                }
            }
        }
    }
}