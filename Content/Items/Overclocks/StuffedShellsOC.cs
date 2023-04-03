using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Humanizer;

namespace deeprockitems.Content.Items.Overclocks
{
    public class StuffedShellsOC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock: Stuffed Shells");
            Tooltip.SetDefault("Right click this item on a Jury-Rigged Boomstick to upgrade the weapon\n" +
                               "Fires twice as many shells in exchange for heavily increased spread and a lower fire rate\n" +
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
