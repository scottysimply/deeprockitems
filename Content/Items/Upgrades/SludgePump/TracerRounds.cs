using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.SludgePump
{
    public class TracerRounds : UpgradeTemplate
    {
        public override string ItemName { get => "Tracer Rounds"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shows the projectile's trajectory"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
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
            .AddIngredient(ItemID.Vertebrae, 10)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
