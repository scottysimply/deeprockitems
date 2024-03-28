﻿using deeprockitems.Content.Items.Upgrades.SludgePumpUpgrades;
using deeprockitems.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Upgrades.ZhukovsUpgrades
{
    public class EmbeddedDetsOC : UpgradeTemplate
    {
        public override bool IsOverclock => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            /*Recipe upgrade = Recipe.Create(ModContent.ItemType<CryoMineletsOC>())
            .AddIngredient<Misc.MatrixCore>()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.FrostCore, 3)
            .AddIngredient(ItemID.Grenade, 15)
            .AddTile(TileID.Anvils);
            upgrade.Register();*/
        }
    }
}