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
    public class InfusedMatrixCore : ModItem
    {
        private Overclock _infusedOverclock;
        public Overclock InfusedOverclock { get => _infusedOverclock; }
        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.maxStack = 1;
        }
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
            if (index <= -1) return;
            if (_infusedOverclock is null) return;
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
            tooltips.Insert(index, new TooltipLine(Mod, "InfusedMatrixTooltip", string.Format(Language.GetOrRegister("Mods.deeprockitems.Items.InfusedMatrixCore.InfusedTooltip").Value, _infusedOverclock.WeaponName, _infusedOverclock.DisplayName)));
        }
        public void InfuseWthOverclock(Overclock overclock) {
            if (overclock is null) throw new ArgumentNullException("Matrix cores cannot be set with null!");
            _infusedOverclock = overclock;
            Item.SetNameOverride(DisplayName.Format(overclock.DisplayName));
        }
    }
}