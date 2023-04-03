using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Humanizer;

namespace deeprockitems.Content.Items.Overclocks
{
    public class PelletAlignmentOC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock: Magnetic Pellet Alignment");
            Tooltip.SetDefault("Right click this item on a Jury-Rigged Boomstick to upgrade the weapon\n" +
                               "Tighter spread in exchange for reduced damage\n" +
                               "Consumable");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.height = 30;
            Item.width = 30;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
    }
}
