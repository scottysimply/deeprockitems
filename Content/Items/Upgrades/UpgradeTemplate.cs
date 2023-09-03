using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual string ItemName { get; set; }
        public virtual string ItemTooltip { get; set; }
        public virtual bool IsOverclock { get; set; }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = Item.height = 30;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
    }
}