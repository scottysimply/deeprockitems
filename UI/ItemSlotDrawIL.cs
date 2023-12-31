using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using MonoMod.Cil;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.UI.UpgradeItem;
using deeprockitems.Assets.Textures;
using System.Reflection;
using Mono.Cecil.Cil;
using Humanizer;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using System.Reflection.Metadata;

namespace deeprockitems.UI
{
    public class ItemSlotDrawIL : ModSystem
    {
        public override void Load()
        {
            IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += DrawHook;
        }
        private static int _timer = 0;
        public override void UpdateUI(GameTime gameTime)
        {
            _timer++;
        }
        private void DrawHook(ILContext il)
        {
            try
            {
                // Initialize cursor
                ILCursor c = new ILCursor(il);

                // Find where the entry point of this code will be. This is where flag2 is loaded as a local.
                c.GotoNext(i => i.MatchLdloc(9));
                c.Index++;
                Mod.Logger.Info($"Found entry point at line {c.Index}");

                // Emit all required values to stack
                Mod.Logger.Info("Emitting values to stack");

                c.Emit(OpCodes.Ldarg_1); // Inventory array
                c.Emit(OpCodes.Ldarg_3); // Slot number
                c.Emit(OpCodes.Ldloc_S, (byte)8); // color2
                c.Emit(OpCodes.Ldsfld, typeof(ItemSlotDrawIL).GetField(nameof(_timer), BindingFlags.NonPublic | BindingFlags.Static)); // Pushing my timer onto the stack.

                // Emit the delegate (the code)
                Mod.Logger.Info("Begin emitting delegate");
                c.EmitDelegate<Func<Item[], int, Color, int, Color>>((inv, slot, initialColor, timer) => {
                    // This is the actual code that is modified.
                    UpgradeableItemTemplate modItem = UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate;
                    if (modItem != null && modItem.ValidUpgrades.Contains(inv[slot].type))
                    {
                        float adjusted_time = 0.2f * MathF.Sin(timer / 30f) + 0.65f;
                        return Color.Yellow * adjusted_time;
                    }
                    return initialColor;
                });

                Mod.Logger.Info("Emitting return value");
                c.Emit(OpCodes.Stloc_S, (byte)8);

                Mod.Logger.Info("Inserting code to draw a slot outline.");
                c.Index += 16; // Moving the cursor up

                c.Emit(OpCodes.Ldarg_1); // Inventory array
                c.Emit(OpCodes.Ldarg_3); // Slot number
                c.Emit(OpCodes.Ldarg_0); // SpriteBatch
                c.Emit(OpCodes.Ldsfld, typeof(DRGTextures).GetField(nameof(DRGTextures.SlotOutline), BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)); // Blank slot that we will draw.
                c.Emit(OpCodes.Ldarg_S, (byte)4); // Position
                c.Emit(OpCodes.Ldloc_2); // Inventory Scale

                c.EmitDelegate<Action<Item[], int, SpriteBatch, Texture2D, Vector2, float>>((inv, slot, spriteBatch, texture, position, scale) => {
                    // This is the actual code that is modified.
                    UpgradeableItemTemplate modItem = UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate;
                    if (modItem != null && modItem.ValidUpgrades.Contains(inv[slot].type))
                    {
                        spriteBatch.Draw(texture, position, null, Color.Yellow, 0f, new(0, 0), scale, SpriteEffects.None, 0f);
                    }
                });

                MonoModHooks.DumpIL(Mod, il);
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(Mod, il);
                Mod.Logger.Info("Exception found somewhere. Dumping IL.");
            }
        }
    }
}
