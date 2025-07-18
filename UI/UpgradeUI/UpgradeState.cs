using deeprockitems.UI.UpgradeUI.OverclockUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeState : UIState
    {
        public TabViewPanel<UpgradePanel> Panel { get; set; }
        public static Vector2 MenuSize { get => new(420, 220); }
        public override void OnInitialize()
        {
            Panel = new();
            Panel.Left.Pixels = 73f;
            Panel.Top.Pixels = Main.instance.invBottom;
            Panel.Height.Pixels = MenuSize.Y;
            Panel.Width.Pixels = MenuSize.X;
            Panel.AddPanel("Upgrade", new UpgradeSelectionPanel() {
                Height = { Pixels = MenuSize.Y },
                Width = { Pixels = MenuSize.X }
            });
            Panel.AddPanel("Overclock", new OverclockPanel() {
                Height = { Pixels = MenuSize.Y },
                Width = { Pixels = MenuSize.X }
            });
            Panel.SetLabelHeight(20f);
            Append(Panel);
            Panel.Activate();
        }
        internal Item ItemInSlot
        {
            get => UpgradeUIPlayer.ItemInUpgradeSlot;
            set => UpgradeUIPlayer.ItemInUpgradeSlot = value;
        }
    }
}
