using deeprockitems.Content.Items.Misc;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockPanel : UpgradePanel
    {
        public FakeItemSlot MatrixCoreSlot { get; set; }
        public override void PostInitialize() {
            MatrixCoreSlot = new FakeItemSlot((mouseItem, slotItem) => {
                if (mouseItem.ModItem is MatrixCore) return true;
                if (mouseItem.type == 0 & slotItem.type > 0) return true;
                return false;
            });
            MatrixCoreSlot.Left = new StyleDimension { Pixels = ParentSlot.Left.Pixels + ParentSlot.Width.Pixels };
            MatrixCoreSlot.Top = ParentSlot.Top;
            MatrixCoreSlot.Width = MatrixCoreSlot.Height = new StyleDimension { Pixels = 52 };
            Append(MatrixCoreSlot);
        }
        protected override void OnClickForgeButton(UIMouseEvent evt, UIElement sender) {
            if (MatrixCoreSlot.ItemInSlot?.type != 0)
            {
                MatrixCoreSlot.ItemInSlot.stack--;
                if (MatrixCoreSlot.ItemInSlot.stack == 0)
                {
                    MatrixCoreSlot.ItemInSlot.TurnToAir();
                }
                // Consume item in slot
                Main.NewText("Explode");
            }
        }
    }
}
