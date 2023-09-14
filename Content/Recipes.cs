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
            RecipeGroup AnyPowder = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.VilePowder)}", ItemID.VilePowder, ItemID.ViciousPowder);
            RecipeGroup.RegisterGroup(nameof(ItemID.VilePowder), AnyPowder);
        }
    }
}