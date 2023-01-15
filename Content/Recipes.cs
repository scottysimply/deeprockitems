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
        public override void AddRecipes()
        {
            Recipe M1k = Recipe.Create(ModContent.ItemType<Content.Items.Weapons.M1000>());
            M1k.AddIngredient(ItemID.Musket, 1);
            M1k.AddIngredient(ItemID.IllegalGunParts, 1);
            M1k.AddIngredient(ItemID.HallowedBar, 15);
            M1k.AddIngredient(ItemID.SoulofFright, 10);
            M1k.AddTile(TileID.MythrilAnvil);
            M1k.Register();

            M1k = Recipe.Create(ModContent.ItemType<Content.Items.Weapons.M1000>());
            M1k.AddIngredient(ItemID.TheUndertaker, 1);
            M1k.AddIngredient(ItemID.IllegalGunParts, 1);
            M1k.AddIngredient(ItemID.HallowedBar, 15);
            M1k.AddIngredient(ItemID.SoulofFright, 10);
            M1k.AddTile(TileID.MythrilAnvil);
            M1k.Register();

            Recipe SludgePump = Recipe.Create(ModContent.ItemType<Content.Items.Weapons.SludgePump>());
            SludgePump.AddIngredient(ItemID.HellstoneBar, 15);
            SludgePump.AddIngredient(ItemID.Gel, 50);
            SludgePump.AddIngredient(ItemID.Bone, 15);
            SludgePump.AddTile(TileID.Solidifier);
            SludgePump.Register();

            Recipe JuryShotgun = Recipe.Create(ModContent.ItemType<Content.Items.Weapons.JuryShotgun>());
            JuryShotgun.AddIngredient(ItemID.Boomstick, 1);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.GoldBar), 8);
            JuryShotgun.AddIngredient(ItemID.Bone, 10);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.VilePowder), 10);
            JuryShotgun.AddTile(TileID.Anvils);
            JuryShotgun.Register();
        }
    }
}