using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace deeprockitems
{
    public class Recipes : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup AnyGold = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), AnyGold);
            RecipeGroup AnyEvilBar = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), AnyEvilBar);
            RecipeGroup AnyPowder = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.VilePowder)}", ItemID.VilePowder, ItemID.ViciousPowder);
            RecipeGroup.RegisterGroup(nameof(ItemID.VilePowder), AnyPowder);
            RecipeGroup AnyTier1Ore = new(() => $"{Language.GetTextValue("LegacyMisc")} {Lang.GetItemNameValue(ItemID.CobaltBar)}", ItemID.CobaltBar, ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CobaltBar), AnyTier1Ore);
        }
    }
}