using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades
{
    public class StuffedShellsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override List<int> ValidWeapons => new List<int>()
        {
            ModContent.ItemType<JuryShotgun>(),
        };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<StuffedShellsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.MythrilBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 25)
            .AddIngredient(ItemID.MusketBall, 100)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<StuffedShellsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.OrichalcumBar, 10)
            .AddRecipeGroup(nameof(ItemID.VilePowder), 25)
            .AddIngredient(ItemID.MusketBall, 100)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
