using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistol
{
    public class PiercingPlasmaOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<PiercingPlasmaOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
