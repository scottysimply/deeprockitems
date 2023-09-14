using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.SludgePump
{
    public class SludgeExplosionOC : UpgradeTemplate
    {
        public override string ItemName { get => "Waste Ordnance"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Focus shots explode instead of fragmenting"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<SludgeExplosionOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.CobaltBar, 10)
            .AddIngredient(ItemID.Gel, 15)
            .AddIngredient(ItemID.Bomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<SludgeExplosionOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.PalladiumBar, 10)
            .AddIngredient(ItemID.Gel, 15)
            .AddIngredient(ItemID.Bomb, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
