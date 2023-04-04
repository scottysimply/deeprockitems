using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Overclocks
{
    public class PelletAlignmentOC : UpgradeTemplate
    {
        public override string ItemName { get => "Magnetic Pellet Alignment"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Tighter spread, but with lower base damage"; set => base.ItemTooltip = value; }
        public override int ItemToUpgrade { get => ModContent.ItemType<Weapons.JuryShotgun>(); set => base.ItemToUpgrade = value; }
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
