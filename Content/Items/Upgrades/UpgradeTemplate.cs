using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Content.Items.Weapons;
using Terraria.UI;
using System.Linq;
using System;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual bool IsOverclock { get; set; }
        /// <summary>
        ///  This is used for displaying what weapons the upgrade can be used on. I elected to use this syntax since it's slightly cleaner lol
        /// </summary>
        public virtual List<int> ValidWeapons { get; set; } = new List<int>() { 0 };
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = Item.height = 30;
            Item.value = IsOverclock ? Item.buyPrice(0, 5, 0, 0) : Item.buyPrice(0, 3, 0, 0);
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color c = new(74, 177, 211);
            if (ItemSlot.ShiftInUse && ValidWeapons.Count > 0)
            {
                string upgrades_text = Language.GetTextValue("Mods.deeprockitems.Misc.ValidUpgradesText", DisplayName);
                foreach (int type in ValidWeapons)
                {
                    upgrades_text += " " + Lang.GetItemName(type) +",";


                }
                upgrades_text = upgrades_text.TrimEnd(',');
                TooltipLine line = new(Mod, "Upgrades", upgrades_text)
                {
                    OverrideColor = c,
                };
                tooltips.Add(line);
            }
            else if (ValidWeapons.Count > 0)
            {
                TooltipLine line = new(Mod, "ShiftToView", Language.GetTextValue("Mods.deeprockitems.Misc.WeaponShiftTooltip"))
                {
                    OverrideColor = c,
                };
                tooltips.Add(line);
            }
        }
    }
}