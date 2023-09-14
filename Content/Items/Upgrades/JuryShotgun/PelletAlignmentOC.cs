using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.M1000;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgun
{
    public class PelletAlignmentOC : UpgradeTemplate
    {
        public override string ItemName { get => "Magnetic Pellet Alignment"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Tighter spread"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<PelletAlignmentOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 15)
            .AddIngredient(ItemID.CelestialMagnet)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
