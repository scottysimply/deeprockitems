using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades
{
    public class QuickCharge : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override List<int> ValidWeapons => new List<int>()
        {
            ModContent.ItemType<M1000>(),
            ModContent.ItemType<SludgePump>(),
            ModContent.ItemType<PlasmaPistol>(),
        };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<QuickCharge>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.CobaltBar, 15)
            .AddIngredient(ItemID.SwiftnessPotion, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<QuickCharge>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.PalladiumBar, 15)
            .AddIngredient(ItemID.SwiftnessPotion, 5)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
