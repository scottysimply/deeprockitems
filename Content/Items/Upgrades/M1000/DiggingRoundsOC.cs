using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Items.Upgrades.M1000
{
    public class DiggingRoundsOC : UpgradeTemplate
    {
        public override string ItemName { get => "Digger Rounds"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Focus shots fire through tiles"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<DiggingRoundsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddIngredient(ItemID.MusketBall, 30)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<DiggingRoundsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.OrichalcumBar, 10)
            .AddIngredient(ItemID.MusketBall, 30)
            .AddIngredient(ItemID.ScarabBomb, 5)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
