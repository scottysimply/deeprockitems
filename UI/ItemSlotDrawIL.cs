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
                var drawVanillaLabel = c.DefineLabel();

                // I want to learn how to emit raw IL, so this section is raw IL

                Mod.Logger.Info($"Found entry point at line {c.Index}");
                c.MoveAfterLabels();
                // Emit all required values to stack
                Mod.Logger.Info("Emitting conditional");
                c.Emit(OpCodes.Call, typeof(UpgradeUISystem).GetProperty(nameof(UpgradeUISystem.TheParentItem), BindingFlags.Public | BindingFlags.Static).GetMethod); // Pulls an UpgradeableItemTemplate (modItem) onto stack
                c.Emit(OpCodes.Brfalse_S, drawVanillaLabel); // If modItem is null, continue vanilla code

                c.Emit(OpCodes.Call, typeof(UpgradeUISystem).GetProperty(nameof(UpgradeUISystem.TheParentItem), BindingFlags.Public | BindingFlags.Static).GetMethod); // Pulls an UpgradeableItemTemplate (modItem) onto stack
                c.Emit(OpCodes.Callvirt, typeof(UpgradeableItemTemplate).GetProperty(nameof(UpgradeableItemTemplate.ValidUpgrades), BindingFlags.Public | BindingFlags.Instance).GetMethod); // List<int> on stack: ValidUpgrades
                c.Emit(OpCodes.Ldarg_1); // Item[] on stack: inv
                c.Emit(OpCodes.Ldarg_3); // Item[] & int on stack: inv & slot
                c.Emit(OpCodes.Ldelem_Ref); // Item on stack: inv[slot]
                c.Emit(OpCodes.Ldfld, typeof(Item).GetField(nameof(Item.type), BindingFlags.Public | BindingFlags.Instance)); // int on stack: inv[slot].type
                c.Emit(OpCodes.Callvirt, typeof(System.Collections.Generic.List<int>).GetMethod(nameof(System.Collections.Generic.List<int>.Contains), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, new Type[] { typeof(int) })); // Nothing on stack
                c.Emit(OpCodes.Brfalse_S, drawVanillaLabel); // If didn't contain item, continue vanilla code

                Mod.Logger.Info("Emitting code to set color and texture");
                c.Emit(OpCodes.Call, typeof(DRGTextures).GetProperty(nameof(DRGTextures.InventorySlot), BindingFlags.Public | BindingFlags.Static).GetMethod); // Texture2D on stack: InventorySlot
                c.Emit(OpCodes.Stloc_S, (byte)7); // Store to texture2D value

                c.Emit(OpCodes.Call, typeof(Color).GetProperty(nameof(Color.Yellow), BindingFlags.Public | BindingFlags.Static).GetMethod); // Color on stack: Color.Yellow
                c.Emit(OpCodes.Ldc_R4, 0.25f); // Added float to stack. Amplitude of Sine wave
                c.Emit(OpCodes.Ldsfld, typeof(ItemSlotDrawIL).GetField(nameof(_timer), BindingFlags.NonPublic | BindingFlags.Static)); // Added _timer to stack
                c.Emit(OpCodes.Conv_R4); // Convert _timer to float
                c.Emit(OpCodes.Ldc_R4, 30f); // Added period to stack
                c.Emit(OpCodes.Div); // Divided _timer by period
                c.Emit(OpCodes.Call, typeof(MathF).GetMethod(nameof(MathF.Sin), BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(float) })); // Sin of _timer / period
                c.Emit(OpCodes.Mul); // Multiply amplitude & Sine resultant
                c.Emit(OpCodes.Ldc_R4, 0.75f); // Pushing Y-offset to stack
                c.Emit(OpCodes.Add); // Adding Y-offset
                c.Emit(OpCodes.Call, typeof(Color).GetMethod(nameof(Color.Multiply), BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(Color), typeof(float) })); // Multiply opacity of Color.Yellow by Sin resultant
                c.Emit(OpCodes.Stloc_S, (byte)8); // Store color

                c.MarkLabel(drawVanillaLabel);

                // Begin drawing slot outline
                c.Index += 17;
                var drawOutlineLabel = c.DefineLabel();
                c.MoveAfterLabels();

                c.Emit(OpCodes.Ldarg_1); // Inventory array
                c.Emit(OpCodes.Ldarg_3); // Slot number
                c.Emit(OpCodes.Ldarg_0); // SpriteBatch
                c.Emit(OpCodes.Call, typeof(DRGTextures).GetProperty(nameof(DRGTextures.SlotOutline), BindingFlags.Public | BindingFlags.Static).GetMethod); // Slot outline texture
                c.Emit(OpCodes.Ldarg_S, (byte)4); // Position
                c.Emit(OpCodes.Ldloc_2); // Inventory Scale

                c.EmitDelegate<Action<Item[], int, SpriteBatch, Texture2D, Vector2, float>>((inv, slot, spriteBatch, texture, position, scale) => {
                    if (UpgradeUISystem.TheParentItem != null && UpgradeUISystem.TheParentItem.ValidUpgrades.Contains(inv[slot].type))
                    {
                        spriteBatch.Draw(texture, position, null, Color.Yellow, 0f, new(0, 0), scale, SpriteEffects.None, 0f);
                    }
                });

                c.MarkLabel(drawOutlineLabel);


                /*c.Emit(OpCodes.Ldarg_1); // Inventory array
                c.Emit(OpCodes.Ldarg_3); // Slot number
                c.Emit(OpCodes.Ldloc_S, (byte)8); // color2
                c.Emit(OpCodes.Ldsfld, typeof(ItemSlotDrawIL).GetField(nameof(_timer), BindingFlags.NonPublic | BindingFlags.Static)); // Pushing my timer onto the stack.

                // Emit the delegate (the code)
                Mod.Logger.Info("Begin emitting delegate");
                c.EmitDelegate<Action<Item[], int, Color, int>>((inv, slot, initialColor, timer) => {
                    // This is the actual code that is modified.
                    UpgradeableItemTemplate modItem = UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay.ModItem as UpgradeableItemTemplate;
                    if (modItem != null && modItem.ValidUpgrades.Contains(inv[slot].type))
                    {
                        float adjusted_time = 0.1f * MathF.Sin(timer / 30f) + 0.65f;
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
                });*/

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
