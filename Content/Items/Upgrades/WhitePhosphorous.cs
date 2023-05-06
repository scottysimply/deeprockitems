using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class WhitePhosphorous : UpgradeTemplate
    {
        public override string ItemName { get => "White Phosphorous Shells"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Ignite enemies within 10 feet when you fire!"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
