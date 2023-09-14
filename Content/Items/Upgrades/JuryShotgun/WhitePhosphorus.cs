using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgun
{
    public class WhitePhosphorus : UpgradeTemplate
    {
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<WhitePhosphorus>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Fireblossom, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
