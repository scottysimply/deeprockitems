using Terraria;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeState : UIState
    {
        public UpgradePanel Panel { get; set; }
        public override void OnInitialize()
        {
            // default state is UpgradeSelectionPanel
            SetState<UpgradeSelectionPanel>();
        }
        internal Item ItemInSlot { get; set; } = new(0);    
        public void SetState<TPanel>() where TPanel : UpgradePanel, new() {
            RemoveAllChildren();
            Panel = new TPanel();
            // Set size and location
            Panel.Left.Pixels = 73f;
            Panel.Top.Pixels = Main.instance.invBottom;
            Panel.Height.Pixels = 200;
            Panel.Width.Pixels = 420;
            Append(Panel);
            Panel.Activate();
        }
    }
}
