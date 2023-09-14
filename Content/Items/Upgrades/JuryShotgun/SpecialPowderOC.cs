using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.M1000;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgun
{
    public class SpecialPowderOC : UpgradeTemplate
    {
        public override string ItemName { get => "Special Powder"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shots propel you backward, but reduced damage"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SpecialPowderOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.CobaltBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 30)
            .AddIngredient(ItemID.SoulofFlight, 10)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<SpecialPowderOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.PalladiumBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 30)
            .AddIngredient(ItemID.SoulofFlight, 10)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
