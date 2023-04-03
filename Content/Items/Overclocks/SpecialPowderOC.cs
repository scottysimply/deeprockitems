using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Humanizer;

namespace deeprockitems.Content.Items.Overclocks
{
    public class SpecialPowderOC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock: Special Powder");
            Tooltip.SetDefault("Right click this item on a Jury-Rigged Boomstick to upgrade the weapon\n" +
                               "Each shot launches you in the opposite direction you aim\n" +
                               "Consumable");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.height = 30;
            Item.width = 30;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
    }
}
