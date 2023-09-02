using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.Overclocks
{
    public class PiercingPlasma : UpgradeTemplate
    {
        public override string ItemName { get => "Digger Rounds"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Focus shots fire through tiles"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
