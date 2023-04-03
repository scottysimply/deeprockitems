using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Humanizer;

namespace deeprockitems.Content.Items.Overclocks
{
    public class SupercoolOC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overclock: Supercooling Chamber");
            Tooltip.SetDefault("Right click this item on an M1000 to upgrade the weapon\n" +
                               "Focus shots deal more damage in exchange for less mobility\n" +
                               "Consumable");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            Item.height = 30;
            Item.width = 30;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
    }
}
