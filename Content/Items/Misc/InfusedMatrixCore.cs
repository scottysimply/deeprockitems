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
    public class InfusedMatrixCore : ModItem
    {
        private Overclock _infusedOverclock;
        public Overclock InfusedOverclock { get => _infusedOverclock; }
        public void InfuseWthOverclock(Overclock overclock) {
            if (overclock is null) throw new ArgumentNullException("Matrix cores cannot be set with null!");
            _infusedOverclock = overclock;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
            if (index <= -1) return;

            tooltips.Insert(index, new TooltipLine(Mod, "InfusedMatrixTooltip", string.Format(Language.GetOrRegister("Mods.deeprockitems.Items.InfusedMatrixCore.InfusedTooltip").Value, _infusedOverclock.WeaponName, _infusedOverclock.DisplayName)));
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.maxStack = 1;
        }
    }
}