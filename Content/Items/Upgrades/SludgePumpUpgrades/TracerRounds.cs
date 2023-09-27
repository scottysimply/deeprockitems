using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class TracerRounds : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override List<int> ValidWeapons => new List<int>()
        {
            ModContent.ItemType<SludgePump>(),
        };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<TracerRounds>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.RottenChunk, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<TracerRounds>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.Vertebrae, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
