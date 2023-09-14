using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class DamageUpgrade : UpgradeTemplate
    {
        public override string ItemName { get => "Small Damage Upgrade"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "+5 base damage"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
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
