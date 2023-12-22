using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;

namespace deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades
{
    public class AntiGravOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<AntiGravOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HellstoneBar, 10)            
            .AddIngredient(ItemID.Gel, 10)
            .AddIngredient(ItemID.Feather, 15)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
