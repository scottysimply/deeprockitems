using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items
{
    public abstract class UpgradeTemplate : ModItem
    {
        public string ItemName = "";
        public string ItemTooltip = "";
        public ModItem ItemToUpgrade;
        public bool IsOverclock;
        public UpgradeTemplate(string name, string tooltip, ModItem itemToUpgrade, bool isOverclock)
        {
            ItemName = name;
            ItemTooltip = tooltip;
            ItemToUpgrade = itemToUpgrade;
            IsOverclock = isOverclock;
        }
        public override void SetStaticDefaults()
        {
            string name = string.Format("{0} Upgrade: {1}", ItemToUpgrade.DisplayName, ItemName);
            if (IsOverclock)
            {
                name = name.Replace("Upgrade", "Overclock");
            }
            DisplayName.SetDefault(name);
            Tooltip.SetDefault(ItemTooltip);
        }
        public override void SetDefaults()
        {
            if (IsOverclock)
            Item.width = 30;
            Item.height = 30;
        }
    }
}