using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.Overclocks.JuryShotgun
{
    public class PelletAlignmentOC : UpgradeTemplate
    {
        public override string ItemName { get => "Magnetic Pellet Alignment"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Tighter spread"; set => base.ItemTooltip = value; }
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
