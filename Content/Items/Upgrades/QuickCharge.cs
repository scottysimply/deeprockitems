using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class QuickCharge : UpgradeTemplate
    {
        public override string ItemName { get => "Quick Charge"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "25% faster charge speed"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
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
