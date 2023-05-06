using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Weapons;
using System.Linq;

namespace deeprockitems.UI
{
    public class UpgradeUIState : UIState
    {
        public DragablePanel dragPanel;
        public static DisplaySlot ParentSlot;
        public static UpgradeSlot upgradeSlot1;
        public static UpgradeSlot upgradeSlot2;
        public static UpgradeSlot upgradeSlot3;
        public static UpgradeSlot overclockSlot;

        public bool IsVisible = false;
        public override void OnInitialize()
        {
            dragPanel = new();
            dragPanel.SetPadding(0);

            dragPanel.Left.Set(800, 0);
            dragPanel.Top.Set(100, 0);
            dragPanel.Width.Set(108, 0);
            dragPanel.Height.Set(154, 0);
            dragPanel.BackgroundColor = new Color(73, 94, 171);

            ParentSlot = new DisplaySlot();
            ParentSlot.SetPadding(0);
            upgradeSlot1 = new UpgradeSlot();
            upgradeSlot1.SetPadding(0);
            upgradeSlot2 = new();
            upgradeSlot2.SetPadding(0);
            upgradeSlot3 = new();
            upgradeSlot3.SetPadding(0);
            overclockSlot = new();
            overclockSlot.SetPadding(0);

            ParentSlot.Left.Set(33, 0);
            ParentSlot.Top.Set(10, 0);
            ParentSlot.Width.Set(42, 0);
            ParentSlot.Height.Set(42, 0);

            upgradeSlot1.Left.Set(10, 0);
            upgradeSlot1.Top.Set(56, 0);
            upgradeSlot1.Width.Set(42, 0);
            upgradeSlot1.Height.Set(42, 0);

            upgradeSlot2.Left.Set(56, 0);
            upgradeSlot2.Top.Set(56, 0);
            upgradeSlot2.Width.Set(42, 0);
            upgradeSlot2.Height.Set(42, 0);

            upgradeSlot3.Left.Set(10, 0);
            upgradeSlot3.Top.Set(102, 0);
            upgradeSlot3.Width.Set(42, 0);
            upgradeSlot3.Height.Set(42, 0);

            overclockSlot.Left.Set(56, 0);
            overclockSlot.Top.Set(102, 0);
            overclockSlot.Width.Set(42, 0);
            overclockSlot.Height.Set(42, 0);
            overclockSlot.IsOverclockSlot = true;

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
            upgradeSlot1.ItemInSlot = new();
            upgradeSlot2.ItemInSlot = new();
            upgradeSlot3.ItemInSlot = new();
            overclockSlot.ItemInSlot = new();
        }
        public static int[] GetItems()
        {
            int[] result = new int[4];
            result[0] = upgradeSlot1.ItemInSlot.type;
            result[1] = upgradeSlot2.ItemInSlot.type;
            result[2] = upgradeSlot3.ItemInSlot.type;
            result[3] = overclockSlot.ItemInSlot.type;
            return result;
        }
        public void LoadItem_InSlot()
        {
            UpgradeableItemTemplate Weapon = (UpgradeableItemTemplate)ParentSlot.ItemToDisplay.ModItem;
            if (Weapon is not null)
            {
                BitsByte Helper = Weapon.Upgrades;
                for (int u = 0; u < 8; u++)
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
                }
            }
        }

    }
}