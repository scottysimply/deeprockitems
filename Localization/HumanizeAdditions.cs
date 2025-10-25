using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Humanizer.Inflections;
using Terraria.Localization;

namespace deeprockitems.Localization
{
    public class HumanizeAdditions : ModSystem
    {
        public override void SetStaticDefaults() {
            var itemText = Language.FindAll(Lang.CreateDialogFilter("Mods.deeprockitems.Pluralizer.Items"));
            var npcText = Language.FindAll(Lang.CreateDialogFilter("Mods.deeprockitems.Pluralizer.NPCs"));
            foreach (var item in itemText)
            {
                Vocabularies.Default.AddPlural(item.Key.Replace("Mods.deeprockitems.Pluralizer.Items.", ""), item.Value);
            }
            foreach (var npc in npcText)
            {
                Vocabularies.Default.AddPlural(npc.Key.Replace("Mods.deeprockitems.Pluralizer.NPCs.", ""), npc.Value);
            }
        }
    }
}
