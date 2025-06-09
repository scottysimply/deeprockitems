using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Upgrades;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Items.Misc
{
    public class BlankMatrixCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.maxStack = Item.CommonMaxStack;
        }
        public override void AddRecipes() {
            Recipe.Create(Type)
                .AddRecipeGroup(nameof(ItemID.IronBar), 8)
                .AddIngredient(ItemID.Glass, 6)
                .AddIngredient(ItemID.Diamond, 2);
        }
    }
}