using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using deeprockitems.Content.Items.Weapons;
using Terraria.UI;
using System.Linq;
using System;

namespace deeprockitems.Content.Items.Upgrades
{
    public abstract class UpgradeTemplate : ModItem
    {
        public virtual string ItemName { get; set; }
        public virtual string ItemTooltip { get; set; }
        public virtual bool IsOverclock { get; set; }
        /// <summary>
        ///  This is used for displaying what weapons the upgrade can be used on. I elected to use this syntax since it's slightly cleaner lol
        /// </summary>
        public virtual List<int> ValidWeapons { get; set; }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = Item.height = 30;
        }
        public override bool CanStack(Item item2)
        {
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ItemSlot.ShiftInUse && ValidWeapons.Count > 0)
            {
                string upgrades_text = Language.GetTextValue("Mods.deeprockitems.Misc.ValidUpgradesText");
                foreach (int type in ValidWeapons)
                {
                    upgrades_text += $" {Lang.GetItemName(type)},";


                }
                upgrades_text = upgrades_text.TrimEnd(',');
                TooltipLine line = new(Mod, "Upgrades", upgrades_text);
                tooltips.Add(line);
            }
            else if (ValidWeapons.Count > 0)
            {
                TooltipLine line = new(Mod, "ShiftToView", Language.GetTextValue("Mods.deeprockitems.Misc.WeaponsShiftTooltip", Main.cTorch))
                {
                    OverrideColor = new(74, 177, 211),
                };
                tooltips.Add(line);
            }
        }
    }
}