using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.JuryShotgun;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistol
{
    public class EzBoomOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<EzBoomOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.Bomb, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
