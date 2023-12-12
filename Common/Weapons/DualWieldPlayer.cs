using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

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
                // Gotta keep the rotation looking at least _somewhat_ nice.
                float distance = Main.MouseWorld.DistanceSQ(_offHandOriginOffset);
                if (distance < 4000)
                {
                    OffHandItemRotation = OffHandItemRotation + ((OffHandItemRotation - Player.itemRotation) % MathHelper.TwoPi) * (distance / 4000);
                }
                // Stepup? Good bird, apollo!
                int height_difference = (int)(Player.Bottom.Y - _offHandItemLocation.Y);
                int width_left = (int)(_offHandItemLocation.X - Player.Left.X);
                int width_right = (int)(Player.Right.X - _offHandItemLocation.X);
                float dummy_step = Player.stepSpeed;
                float dummy_gfxoffy = Player.gfxOffY;
                // If player is travelling left:
                if (Player.velocity.X < 0)
                {
                    Collision.StepUp(ref _offHandItemLocation, ref Player.velocity, width_left, height_difference, ref dummy_step, ref dummy_gfxoffy);
                }
                else if (Player.velocity.X > 0)
                {
                    Collision.StepUp(ref _offHandItemLocation, ref Player.velocity, width_right, height_difference, ref dummy_step, ref dummy_gfxoffy);
                }
            }
        }
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (!drawInfo.drawPlayer.ItemAnimationActive) return;
            if (drawInfo.heldItem.type != ModContent.ItemType<Content.Items.Weapons.Zhukovs>()) return;

            PlayerDrawLayers.HeldItem.Hide();
        }
    }
}
