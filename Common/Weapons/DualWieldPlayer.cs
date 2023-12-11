using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Common.Weapons
{
    public class DualWieldPlayer : ModPlayer
    {
        public Vector2 offHandOffset
        {
            get => new(8f, -8f);
        }
        public float offHandItemRotation
        {
            get
            {
                if (Player.direction == 1)
                {
                    return offHandItemLocation.DirectionTo(Main.MouseWorld).ToRotation();
                }
                else
                {
                    return offHandItemLocation.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.Pi;
                }
            }
        }
        public Vector2 offHandItemLocation
        {
            get
            {
                if (Player.direction == 1)
                {
                    return new(Player.itemLocation.X + offHandOffset.X, Player.itemLocation.Y + offHandOffset.Y);
                }
                else
                {
                    return new(Player.itemLocation.X - offHandOffset.X, Player.itemLocation.Y + offHandOffset.Y);
                }
            }
        }
        public Vector2 thisPlayerMouseWorld { get; set; }
        public override void PreUpdate()
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                thisPlayerMouseWorld = Main.MouseWorld;
            }
        }
    }
}
