using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades.PlasmaPistol;

namespace deeprockitems.Content.Items.Upgrades
{
    public class ArmorPierce : UpgradeTemplate
    {
        public override string ItemName { get => "Pen Rounds"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shots ignore 10 defense"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe upgrade = Recipe.Create(ModContent.ItemType<ArmorPierce>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.CobaltBar, 15)
            .AddIngredient(ItemID.SharkToothNecklace)
            .AddTile(TileID.Anvils);
            upgrade.Register();

            upgrade = Recipe.Create(ModContent.ItemType<ArmorPierce>())
            .AddIngredient<Misc.UpgradeToken>()
            .AddIngredient(ItemID.PalladiumBar, 15)
            .AddIngredient(ItemID.SharkToothNecklace)
            .AddTile(TileID.Anvils);
            upgrade.Register();
        }
    }
}
