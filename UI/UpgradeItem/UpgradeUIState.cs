using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using System.Linq;

namespace deeprockitems.UI.UpgradeItem
{
    public class UpgradeUIState : UIState
    {
        public DragablePanel dragPanel;
        public DisplaySlot ParentSlot;
        public UpgradeSlot upgradeSlot1;
        public UpgradeSlot upgradeSlot2;
        public UpgradeSlot upgradeSlot3;
        public UpgradeSlot overclockSlot;
        private const int OUTER_PADDING = 8;
        private const int INNER_PADDING = 4;
        private const int SLOT_SIZE = 40;

        public override void OnInitialize()
        {
            int total_width = 2 * OUTER_PADDING + INNER_PADDING + 2 * SLOT_SIZE;
            int total_height = 2 * OUTER_PADDING + 2 * INNER_PADDING + 3 * SLOT_SIZE;
            dragPanel = new();
            dragPanel.SetPadding(0);

            dragPanel.Left.Set(800, 0);
            dragPanel.Top.Set(100, 0);
            dragPanel.Width.Set(total_width, 0);
            dragPanel.Height.Set(total_height, 0);
            dragPanel.BackgroundColor = new Color(73, 94, 171);

            ParentSlot = new DisplaySlot();
            ParentSlot.SetPadding(0);
            upgradeSlot1 = new(0);
            upgradeSlot1.SetPadding(0);
            upgradeSlot2 = new(1);
            upgradeSlot2.SetPadding(0);
            upgradeSlot3 = new(2);
            upgradeSlot3.SetPadding(0);
            overclockSlot = new(3);
            overclockSlot.SetPadding(0);

            ParentSlot.Left.Set((total_width - SLOT_SIZE) / 2, 0);
            ParentSlot.Top.Set(OUTER_PADDING, 0);
            ParentSlot.Width.Set(SLOT_SIZE, 0);
            ParentSlot.Height.Set(SLOT_SIZE, 0);

            upgradeSlot1.Left.Set(OUTER_PADDING, 0);
            upgradeSlot1.Top.Set(OUTER_PADDING + SLOT_SIZE + INNER_PADDING, 0);
            upgradeSlot1.Width.Set(SLOT_SIZE, 0);
            upgradeSlot1.Height.Set(SLOT_SIZE, 0);

            upgradeSlot2.Left.Set(total_width - OUTER_PADDING - SLOT_SIZE, 0);
            upgradeSlot2.Top.Set(OUTER_PADDING + SLOT_SIZE + INNER_PADDING, 0);
            upgradeSlot2.Width.Set(SLOT_SIZE, 0);
            upgradeSlot2.Height.Set(SLOT_SIZE, 0);

            upgradeSlot3.Left.Set(OUTER_PADDING, 0);
            upgradeSlot3.Top.Set(total_height - OUTER_PADDING - SLOT_SIZE, 0);
            upgradeSlot3.Width.Set(SLOT_SIZE, 0);
            upgradeSlot3.Height.Set(SLOT_SIZE, 0);

            overclockSlot.Left.Set(total_width - OUTER_PADDING - SLOT_SIZE, 0);
            overclockSlot.Top.Set(total_height - OUTER_PADDING - SLOT_SIZE, 0);
            overclockSlot.Width.Set(SLOT_SIZE, 0);
            overclockSlot.Height.Set(SLOT_SIZE, 0);

            dragPanel.Append(ParentSlot);
            dragPanel.Append(upgradeSlot1);
            dragPanel.Append(upgradeSlot2);
            dragPanel.Append(upgradeSlot3);
            dragPanel.Append(overclockSlot);

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
        public void ClearItems()
        {
            ParentSlot.ItemToDisplay = new();
            upgradeSlot1.ItemToDisplay = new();
            upgradeSlot2.ItemToDisplay = new();
            upgradeSlot3.ItemToDisplay = new();
            overclockSlot.ItemToDisplay = new();
        }
        public int[] GetItems()
        {
            int[] result = new int[4];
            result[0] = upgradeSlot1.ItemToDisplay.type;
            result[1] = upgradeSlot2.ItemToDisplay.type;
            result[2] = upgradeSlot3.ItemToDisplay.type;
            result[3] = overclockSlot.ItemToDisplay.type;
            return result;
        }
        public void LoadItem_InSlot()
        {
            UpgradeableItemTemplate Weapon = (UpgradeableItemTemplate)ParentSlot.ItemToDisplay.ModItem;
            if (Weapon is not null)
            {

                upgradeSlot1.ItemToDisplay = new(Weapon.Upgrades2[0]);
                upgradeSlot2.ItemToDisplay = new(Weapon.Upgrades2[1]);
                upgradeSlot3.ItemToDisplay = new(Weapon.Upgrades2[2]);
                overclockSlot.ItemToDisplay = new(Weapon.Overclock);
                /*for (int u = 0; u < 8; u++)
                {
                    if (0 <= u && u <= 2)
                    {
                        if (Helper[u])
                        {
                            overclockSlot.ItemInSlot = new(Weapon.ValidUpgrades[u]);
                            u = 2;
                        }
                    }
                    else
                    {
                        if (Helper[u])
                        {
                            if (upgradeSlot1.ItemInSlot.type == 0)
                            {
                                upgradeSlot1.ItemInSlot = new(Weapon.ValidUpgrades[u]);
                            }
                            else if (upgradeSlot2.ItemInSlot.type == 0)
                            {
                                upgradeSlot2.ItemInSlot = new(Weapon.ValidUpgrades[u]);
                            }
                            else if (upgradeSlot3.ItemInSlot.type == 0)
                            {
                                upgradeSlot3.ItemInSlot = new(Weapon.ValidUpgrades[u]);
                            }
                            else
                            {
                                Player player = Main.player[Main.myPlayer];
                                Item.NewItem(player.GetSource_Misc("PlayerDropItemCheck"), new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Weapon.ValidUpgrades[u]);
                            }
                        }

                    }
                }*/
            }
        }

    }
}