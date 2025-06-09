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
    public class MatrixCore : ModItem
    {
        public bool IsInfused { get => _infusedOverclock != null; }
        private Overclock _infusedOverclock;
        public Overclock InfusedOverclock { get => _infusedOverclock; }
        public void InfuseWthOverclock(Overclock overclock) {
            if (overclock is null) throw new ArgumentNullException("Matrix cores cannot be set with null!");
            _infusedOverclock = overclock;
        }
        public override LocalizedText DisplayName => IsInfused ? Language.GetOrRegister("Mods.deeprockitems.Items.MatrixCore.InfusedDisplayName") : base.DisplayName;
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
            if (index <= -1) return;
            // if not infused, add the blank tooltip
            if (!IsInfused)
            {
                tooltips.Insert(index, new TooltipLine(Mod, "BlankMatrixTooltip", Language.GetOrRegister("Mods.deeprockitems.Items.MatrixCore.BlankTooltip").Value));
                return;
            }

            tooltips.Insert(index, new TooltipLine(Mod, "InfusedMatrixTooltip", Language.GetOrRegister("Mods.deeprockitems.Items.MatrixCore.InfusedTooltip").Value));
        }
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