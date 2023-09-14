using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.SludgePump
{
    public class GooSpecialOC : UpgradeTemplate
    {
        public override string ItemName { get => "Goo Bomber Special"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Charge shots drop goo fragments as they travel"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<GooSpecialOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddIngredient(ItemID.Gel, 20)
            .AddIngredient(ItemID.Ale, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<GooSpecialOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.OrichalcumBar, 10)
            .AddIngredient(ItemID.Gel, 20)
            .AddIngredient(ItemID.Ale, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
