using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.M1000Upgrades
{
    public class HipsterOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override List<int> ValidWeapons => new List<int>()
        {
            ModContent.ItemType<M1000>(),
        };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<HipsterOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.ChlorophyteBar, 25)
            .AddIngredient(ItemID.MusketBall, 150)
            .AddIngredient(ItemID.IllegalGunParts)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
