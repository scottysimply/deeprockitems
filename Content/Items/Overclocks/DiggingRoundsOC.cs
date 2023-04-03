using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Humanizer;

namespace deeprockitems.Content.Items.Overclocks
{
    public class DiggingRoundsOC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock: Digger Rounds");
            Tooltip.SetDefault("Right click this item on an M1000 to upgrade the weapon\n" +
                               "Focus shots can pierce through tiles\n" +
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
