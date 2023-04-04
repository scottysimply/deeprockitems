using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Overclocks
{
    public class SupercoolOC : UpgradeTemplate
    {
        public override string ItemName { get => "Supercooling Chamber"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "2x focus shot damage, with slower focus speed and lower normal shot fire rate"; set => base.ItemTooltip = value; }
        public override int ItemToUpgrade { get => ModContent.ItemType<Weapons.M1000>(); set => base.ItemToUpgrade = value; }
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
