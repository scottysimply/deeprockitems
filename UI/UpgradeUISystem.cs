using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace deeprockitems.UI
{
    public class UpgradeUISystem : ModSystem
    {
        internal UpgradeUIPanel UIPanel;
        private UserInterface _uiPanel;


        public override void Load()
        {
            UIPanel = new();
            UIPanel.Activate();
            _uiPanel = new();
            _uiPanel.SetState(UIPanel);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _uiPanel?.Update(gameTime);


        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        _uiPanel.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}