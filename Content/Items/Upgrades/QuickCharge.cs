using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class QuickCharge : UpgradeTemplate
    {
        public override string ItemName { get => "Quick Charge"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "25% faster charge speed"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
    }
}
