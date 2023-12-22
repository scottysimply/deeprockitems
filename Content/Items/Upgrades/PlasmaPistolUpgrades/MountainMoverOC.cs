using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    public class MountainMoverOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<MountainMoverOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.CobaltBar, 10)
            .AddIngredient(ItemID.FallenStar, 15)
            .AddIngredient(ItemID.Dynamite, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<MountainMoverOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.PalladiumBar, 10)
            .AddIngredient(ItemID.FallenStar, 15)
            .AddIngredient(ItemID.Dynamite, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
