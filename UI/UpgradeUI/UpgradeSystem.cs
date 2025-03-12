using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class UpgradeSystem : ModSystem
    {
        public UpgradeState? UpgradeUIState { get => (Interface.CurrentState as UpgradeState) ?? null; }
        public UserInterface Interface;
        public static UserInterface StaticInterface { get => ModContent.GetInstance<UpgradeSystem>().Interface; }
        public static bool IsUIOpen { get => ModContent.GetInstance<UpgradeSystem>().Interface.CurrentState != null; }
        public static void OpenUpgradeInterface() {
            var self = ModContent.GetInstance<UpgradeSystem>();
            UpgradeState state = new();
            state.Activate();
            self.Interface.SetState(state);
        }
        public static void CloseUpgradeInterface() {
            var self = ModContent.GetInstance<UpgradeSystem>();
            self.Interface.SetState(null);

            // Try giving item back to player
            if (self.Interface.CurrentState != null && self.UpgradeUIState.Panel.ParentSlot.ItemInSlot != null && self.UpgradeUIState.Panel.ParentSlot.ItemInSlot.type != 0)
            {
                Main.LocalPlayer.QuickSpawnItem(self.UpgradeUIState.Panel.ParentSlot.ItemInSlot.GetSource_ReleaseEntity(), self.UpgradeUIState.Panel.ParentSlot.ItemInSlot);
            }
        }
        public override void Load()
        {
            Interface = new();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            Interface?.Update(gameTime);
            if (UpgradeUIState?.IsMouseHovering ?? false)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }        
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex == -1) return;
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("deeprockitems: UpgradeStationUI",
                                                                       () =>
                                                                       {
                                                                           Interface.Draw(Main.spriteBatch, new GameTime());
                                                                           return true;
                                                                       },
                                                                       InterfaceScaleType.UI));
        }
    }
}
