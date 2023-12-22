using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class GooSpecialOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
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
