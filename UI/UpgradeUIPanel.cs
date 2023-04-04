using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.UI
{
    public class UpgradeUIPanel : UIState
    {
        public DragablePanel dragPanel;
        CustomItemSlot UpgradeSlot1;
        CustomItemSlot UpgradeSlot2;
        CustomItemSlot UpgradeSlot3;
        CustomItemSlot OverclockSlot;

        public bool IsVisible = false;
        public override void OnInitialize()
        {
            dragPanel = new();
            dragPanel.SetPadding(0);

            dragPanel.Left.Set(400, 0);
            dragPanel.Top.Set(100, 0);
            dragPanel.Width.Set(120, 0);
            dragPanel.Height.Set(240, 0);
            dragPanel.BackgroundColor = new Color(73, 94, 171);

            UpgradeSlot1 = new();
            UpgradeSlot1.SetPadding(0);
            UpgradeSlot1.Left.Set(5, 0);
            UpgradeSlot1.Top.Set(80, 0);
            UpgradeSlot1.Width.Set(20, 0);
            UpgradeSlot1.Height.Set(20, 0);

            UpgradeSlot2 = new();
            UpgradeSlot2.SetPadding(0);
            UpgradeSlot2.Left.Set(63, 0);
            UpgradeSlot2.Top.Set(80, 0);
            UpgradeSlot2.Width.Set(20, 0);
            UpgradeSlot2.Height.Set(20, 0);

            UpgradeSlot3 = new();
            UpgradeSlot3.SetPadding(0);
            UpgradeSlot3.Left.Set(5, 0);
            UpgradeSlot3.Top.Set(140, 0);
            UpgradeSlot3.Width.Set(20, 0);
            UpgradeSlot3.Height.Set(20, 0);

            OverclockSlot = new();
            OverclockSlot.SetPadding(0);
            OverclockSlot.Left.Set(63, 0);
            OverclockSlot.Top.Set(140, 0);
            OverclockSlot.Width.Set(20, 0);
            OverclockSlot.Height.Set(20, 0);

            dragPanel.Append(UpgradeSlot1);
            dragPanel.Append(UpgradeSlot2);
            dragPanel.Append(UpgradeSlot3);
            dragPanel.Append(OverclockSlot);

            Append(dragPanel);
        }
    }
}