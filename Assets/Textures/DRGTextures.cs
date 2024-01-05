using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace deeprockitems.Assets.Textures
{
    public class DRGTextures : ModSystem
    {
        public override void SetStaticDefaults()
        {
            InventorySlot = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/InventorySlot", AssetRequestMode.ImmediateLoad).Value;
            SlotOutline = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/SlotOutline", AssetRequestMode.ImmediateLoad).Value;
        }
        public static Texture2D InventorySlot { get; private set; }
        public static Texture2D SlotOutline { get; private set; }
    }
}
