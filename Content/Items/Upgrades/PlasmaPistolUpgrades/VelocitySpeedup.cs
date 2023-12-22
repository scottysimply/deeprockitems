using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades
{
    public class VelocitySpeedup : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<VelocitySpeedup>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 10)
            .AddIngredient(ItemID.FallenStar, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
