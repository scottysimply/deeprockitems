using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    public class Blowthrough : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<Blowthrough>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.MythrilBar, 15)
            .AddIngredient(ItemID.CrystalBullet, 30)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<Blowthrough>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.OrichalcumBar, 15)
            .AddIngredient(ItemID.CrystalBullet, 15)
            .AddTile(TileID.MythrilAnvil);
            upgrade.Register();
        }
    }
}
