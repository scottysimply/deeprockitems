using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Common.Weapons
{
    public class DualWieldPlayer : ModPlayer
    {
        public static readonly Vector2 OFFHAND_DRAWING_OFFSET = new(6f, -6f);
        public Vector2 ThisPlayerMouseWorld { get; set; }
        public float OffHandItemRotation { get; set; }
        public Vector2 OffHandItemLocation { get => _offHandItemLocation; set => _offHandItemLocation = value; }
        private Vector2 _offHandItemLocation;
        public Vector2 OffHandOriginOffset { get => _offHandOriginOffset; set => _offHandOriginOffset = value; }
        private Vector2 _offHandOriginOffset;
        public override void PostUpdate()
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                // This will come in handy!
                ThisPlayerMouseWorld = Main.MouseWorld;

                // Set the offhand item's true location
                _offHandItemLocation = Player.itemLocation + OFFHAND_DRAWING_OFFSET;
                // Set the origin of rotation for the offhand item
                _offHandOriginOffset = Player.Center + OFFHAND_DRAWING_OFFSET;
                // Set the rotation of the offhand item
                OffHandItemRotation = _offHandOriginOffset.DirectionTo(Main.MouseWorld).ToRotation();
                if (Player.direction == -1) // Change this all for when facing left :p
                {
                    // Set the offhand item's true location
                    _offHandItemLocation.X -= 2 * OFFHAND_DRAWING_OFFSET.X;
                    // Set the origin of rotation for the offhand item
                    _offHandOriginOffset.X -= 2 * OFFHAND_DRAWING_OFFSET.X;
                    // Set the rotation of the offhand item
                    OffHandItemRotation += MathHelper.Pi;
                }
            }
        }
    }
}
