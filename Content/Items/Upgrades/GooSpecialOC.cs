using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class GooSpecialOC : UpgradeTemplate
    {
        public override string ItemName { get => "Goo Bomber Special"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Charge shots drop goo fragments as they travel"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
