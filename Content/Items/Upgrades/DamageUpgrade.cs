using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class DamageUpgrade : UpgradeTemplate
    {
        public override string ItemName { get => "Small Damage Upgrade"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "+5 base damage"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
    }
}
