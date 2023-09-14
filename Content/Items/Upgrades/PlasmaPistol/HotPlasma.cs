using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.PlasmaPistol
{
    public class HotPlasma : UpgradeTemplate
    {
        public override bool IsOverclock => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<HotPlasma>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.Fireblossom, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
