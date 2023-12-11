using System;
using System.Collections.Generic;
using Terraria.ModLoader;
/*
 * Created by stormytuna
 */

namespace deeprockitems.Utilities
{
    public static class TooltipHelpers
    {
        private static readonly List<string> TooltipNames = new() {
        "ItemName", "Favorite", "FavoriteDesc", "Social", "SocialDesc", "Damage", "CritChance", "Speed", "Knockback", "FishingPower", "NeedsBait", "BaitPower", "Equipable", "WandConsumes", "Quest", "Vanity", "Defense", "PickPower", "AxePower", "HammerPower", "TileBoost", "HealLife", "HealMana", "UseMana", "Placeable", "Ammo", "Consumable", "Material", "Tooltip0", "EtherianManaWarning", "WellFedExpert", "BuffTime", "OneDropLogo", "PrefixDamage", "PrefixSpeed", "PrefixCritChance", "PrefixUseMana", "PrefixSize", "PrefixShootSpeed", "PrefixKnockback", "PrefixAccDefense", "PrefixAccMaxMana", "PrefixAccCritChance", "PrefixAccDamage", "PrefixAccMoveSpeed", "PrefixAccMeleeSpeed", "SetBonus", "Expert", "SpecialPrice", "Price"
    };

        public static void InsertTooltips(this List<TooltipLine> tooltips, List<TooltipLine> tooltipsToInsert, string insertAfter)
        {
            int indexInNames = TooltipNames.IndexOf(insertAfter);
            if (indexInNames <= -1)
            {
                throw new IndexOutOfRangeException($"Could not find {insertAfter} in list of tooltip names!");
            }

            while (indexInNames-- > 0)
            {
                string name = TooltipNames[indexInNames];
                int indexInTooltip = tooltips.FindIndex(tip => tip.Name == name);
                if (indexInTooltip <= -1)
                {
                    continue;
                }

                tooltips.InsertRange(indexInTooltip + 1, tooltipsToInsert);
                return;
            }
        }

        public static void InsertTooltip(this List<TooltipLine> tooltips, TooltipLine tooltipToInsert, string insertAfter) => InsertTooltips(tooltips, new List<TooltipLine>() { tooltipToInsert }, insertAfter);
    }
}