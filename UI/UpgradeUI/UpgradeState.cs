using deeprockitems.UI.UpgradeUI.OverclockUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeState : UIState
    {
        public TabViewWithColors Panel { get; set; }
        public static Vector2 MenuSize { get => new(420, 220); }
        public override void OnInitialize()
        {
            Panel = new();
            Panel.Left.Pixels = 73f;
            Panel.Top.Pixels = Main.instance.invBottom;
            Panel.Height.Pixels = MenuSize.Y;
            Panel.Width.Pixels = MenuSize.X;
            // Apply colors
            Panel.PrimaryBorderColor = new(255, 156, 0);
            Panel.PrimaryBackgroundColor = new Color(94, 90, 74, 230);
            Panel.SecondaryBorderColor = new Color(160, 90, 15);
            Panel.SecondaryBackgroundColor = new Color(94 / 255f * 0.84f, 90 / 255f * 0.84f, 74 / 255f * 0.84f);
            Panel.TertiaryBorderColor = new Color(70, 35, 8);
            Panel.TertiaryBackgroundColor = new Color(170, 105, 14);
            Panel.AddPanel("Upgrade", new UpgradeSelectionPanel() {
                Height = { Pixels = MenuSize.Y },
                Width = { Pixels = MenuSize.X }
            });
            Panel.AddPanel("Overclock", new OverclockPanel() {
                Height = { Pixels = MenuSize.Y },
                Width = { Pixels = MenuSize.X }
            });
            Panel.SetLabelHeight(24f);
            Panel.SetFont(FontAssets.DeathText);
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
