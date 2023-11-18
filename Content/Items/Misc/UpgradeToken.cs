using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Misc
{
    public class UpgradeToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
    }
}