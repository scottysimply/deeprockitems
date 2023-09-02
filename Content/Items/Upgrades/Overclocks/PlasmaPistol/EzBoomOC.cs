using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.Overclocks.M1000
{
    public class EzBoomOC : UpgradeTemplate
    {
        public override string ItemName { get => "E-Z Explosion"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "The plasma will explode upon contacting an enemy, with a damage decrease"; set => base.ItemTooltip = value; }
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
