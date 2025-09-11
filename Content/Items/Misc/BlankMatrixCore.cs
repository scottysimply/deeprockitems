using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Upgrades;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Utilities;
using deeprockitems.Localization;

namespace deeprockitems.Content.Items.Misc
{
    public class BlankMatrixCore : ModItem
    {
        private Overclock _infusedOverclock;
        public Overclock InfusedOverclock { get => _infusedOverclock; }
        public override void SetStaticDefaults()
        {
            if (_infusedOverclock == null)
            {
                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
                return;
            }
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            if (_infusedOverclock == null)
            {
                Item.rare = ItemRarityID.Blue;
                Item.value = Item.buyPrice(0, 1, 50, 0);
                Item.maxStack = Item.CommonMaxStack;
                return;
            }
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.maxStack = 1;
        }
        public override void AddRecipes() {
            Recipe.Create(Type)
                .AddRecipeGroup(nameof(ItemID.IronBar), 8)
                .AddIngredient(ItemID.Glass, 6)
                .AddIngredient(ItemID.Diamond, 2);
        }
        internal string ModifiableInternalName = nameof(BlankMatrixCore);
        public override string Texture
        {
            get
            {
                return "deeprockitems/Content/Items/Misc/BlankMatrixCore";
            }
        }
        protected override bool CloneNewInstances => true;
        public override LocalizedText DisplayName
        {
            get
            {
                if (_infusedOverclock == null)
                {
                    return base.DisplayName;
                }
                return Language.GetText("Mods.deeprockitems.Items.BlankMatrixCore.InfusedDisplayName").WithFormatArgs(_infusedOverclock.DisplayName);
            }
        }
        public override LocalizedText Tooltip
        {
            get
            {
                if (ModifiableInternalName is null || ModifiableInternalName == nameof(BlankMatrixCore))
                {
                    return base.Tooltip;
                }
                return Language.GetText("Mods.deeprockitems.Items.BlankMatrixCore.InfusedTooltip");
            }
        }
        internal string ModifiableTooltip;
        public override string Name
        {
            get
            {
                if (_infusedOverclock == null)
                {
                    return base.Name;
                }
                return ModifiableInternalName;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            if (_infusedOverclock is null) return;
            int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
            if (index <= -1) return;
            // must apply tooltips in reverse order
            if (_infusedOverclock.Negatives.Value != _infusedOverclock.Negatives.Key)
            {
                var negatives = _infusedOverclock.Negatives.Value.Split('\n');
                for (int i = 0; i < negatives.Length; i++)
                {
                    tooltips.Insert(index, new TooltipLine(Mod, $"Negative{i}", $"▼ {negatives[i]}".TextColor(DRGText.NegativeText)));
                }
            }
            if (_infusedOverclock.Positives.Value != _infusedOverclock.Positives.Key)
            {
                var positives = _infusedOverclock.Positives.Value.Split('\n');
                for (int i = 0; i < positives.Length; i++)
                {
                    tooltips.Insert(index, new TooltipLine(Mod, $"Positive{i}", $"▲ {positives[i]}".TextColor(DRGText.PositiveText)));
                }
            }
            tooltips.Insert(index, new TooltipLine(Mod, "InfusedMatrixTooltip", string.Format(Language.GetOrRegister("Mods.deeprockitems.Items.BlankMatrixCore.AppliesTo").Value, Mod.Find<ModItem>(_infusedOverclock.WeaponName).DisplayName)));
        }
        public void InfuseWthOverclock(Overclock overclock) {
            if (overclock is null) throw new ArgumentNullException("Matrix cores cannot be set with null!");
            _infusedOverclock = overclock;
            ModifiableInternalName = overclock.WeaponName + overclock.UpgradeName + "Core";
        }
    }
}