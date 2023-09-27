using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    public class DamageUpgrade : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override List<int> ValidWeapons => new List<int>()
        {
            ModContent.ItemType<M1000>(),
            ModContent.ItemType<SludgePump>(),
            ModContent.ItemType<JuryShotgun>(),
            ModContent.ItemType<PlasmaPistol>(),
        };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<DamageUpgrade>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 15)
            .AddIngredient(ItemID.RagePotion, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<DamageUpgrade>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddRecipeGroup(nameof(ItemID.GoldBar), 15)
            .AddIngredient(ItemID.WrathPotion, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
