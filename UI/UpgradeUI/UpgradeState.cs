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
        private Item _oldItem;
        public void SetState<TPanel>() where TPanel : UpgradePanel, new() {
            if (Panel is not null)
            {
                _oldItem = Panel.ParentSlot.ItemInSlot;
            }
            RemoveAllChildren();
            Panel = new TPanel();
            // Set size and location
            Panel.Left.Pixels = 73f;
            Panel.Top.Pixels = Main.instance.invBottom;
            Panel.Height.Pixels = 200;
            Panel.Width.Pixels = 420;
            Append(Panel);
            Panel.Activate();
            if (_oldItem is not null)
            {
                Panel.ParentSlot.ItemInSlot = _oldItem;
                _oldItem = null;
            }
        }
    }
}
