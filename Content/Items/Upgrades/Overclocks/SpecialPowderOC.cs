using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.Overclocks
{
    public class SpecialPowderOC : UpgradeTemplate
    {
        public override string ItemName { get => "Special Powder"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shots propel you backward, but reduced damage"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
