using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class SludgeExplosionOC : UpgradeTemplate
    {
        public override string ItemName { get => "Waste Ordnance"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Focus shots explode instead of fragmenting"; set => base.ItemTooltip = value; }
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
