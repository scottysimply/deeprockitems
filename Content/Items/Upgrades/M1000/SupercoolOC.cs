using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Items.Upgrades.M1000
{
    public class SupercoolOC : UpgradeTemplate
    {
        public override string ItemName { get => "Supercooling Chamber"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "2x focus shot damage, with slower focus speed and lower normal shot fire rate"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SupercoolOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.MusketBall, 75)
            .AddIngredient(ItemID.FrostCore)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

        }
    }
}
