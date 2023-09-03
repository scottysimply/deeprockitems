using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.M1000
{
    public class HipsterOC : UpgradeTemplate
    {
        public override string ItemName { get => "Hipster"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Heavily increased normal shot fire rate, with an inability to focus"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
