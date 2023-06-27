using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual string ItemName { get; set; }
        public virtual string ItemTooltip { get; set; }
        public virtual bool IsOverclock { get; set; }
        public virtual string Positives { get; set; }
        public virtual string Negatives { get; set; }
        public override void SetStaticDefaults()
        {
            string name = IsOverclock ? "Overclock" : "Upgrade"; // If IsOverclock, item name will prefix with Overclock. If !IsOverclock, item name will prefix with Upgrade
        }
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