using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Overclocks
{
    public class SpecialPowderOC : UpgradeTemplate
    {
        public override string ItemName { get => "Special Powder"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Each shot propels you in the opposite direction you aim"; set => base.ItemTooltip = value; }
        public override int ItemToUpgrade { get => ModContent.ItemType<Weapons.JuryShotgun>(); set => base.ItemToUpgrade = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
