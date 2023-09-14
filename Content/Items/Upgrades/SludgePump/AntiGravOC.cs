using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.SludgePump
{
    public class AntiGravOC : UpgradeTemplate
    {
        public override string ItemName { get => "AG Mixture"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shots are no longer affected by gravity"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<AntiGravOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)            
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.Feather, 15)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
