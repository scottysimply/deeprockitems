using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Assets.Textures
{
    public class DRGTextures
    {
        public static Texture2D InventorySlot = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/InventorySlot").Value;
        public static Texture2D SlotOutline = ModContent.Request<Texture2D>("deeprockitems/Assets/Textures/SlotOutline").Value;
    }
}
