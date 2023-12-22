using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class SludgeExplosionOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
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
