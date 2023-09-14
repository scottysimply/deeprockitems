using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class BumpFire : UpgradeTemplate
    {
        public override string ItemName { get => "Bump Fire"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "16% increased fire rate"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<BumpFire>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.IllegalGunParts)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
