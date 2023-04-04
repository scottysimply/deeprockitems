using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Overclocks
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual string ItemName { get; set; }
        public virtual string ItemTooltip { get; set; }
        public virtual int ItemToUpgrade { get; set; }
        public virtual bool IsOverclock { get; set; }
        public override void SetStaticDefaults()
        {
            string name = IsOverclock ? "Overclock" : "Upgrade"; // If IsOverclock, item name will prefix with Overclock. If !IsOverclock, item name will prefix with Upgrade
            DisplayName.SetDefault(string.Format("{0}: {1}", name, ItemName)); // Full item name will be Upgrade/Overclock: Name
            Tooltip.SetDefault(ItemTooltip);
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = 30;
            Item.height = IsOverclock ? 30 : 15;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
    }
}