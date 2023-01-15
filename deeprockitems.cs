using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static System.Math;

namespace deeprockitems
{
	public class deeprockitems : Mod
	{

    }
	public class MyFunctions // These are my helper functions. These are useful bits in my program that I am using often enough to warrant putting them in here.
	{
		public static Vector2 MouseRel(Vector2 mousePos, Player player)
		{
			Vector2 difference = mousePos - player.Center;
			return difference;
		}
		public static void RenderItem(Player player, Vector2 mouseRelative, Vector2 mouseAbsolute)
		{
			float myRotation;
			if (player.itemTime == 1)
			{
				player.itemTime = 2;
				player.itemAnimation = 2;
			}
			if (mouseAbsolute.X < player.Center.X) // cursor is to the left of the player
			{
				if (mouseAbsolute.Y < player.Center.Y) // Mouse is above the player
				{
                    myRotation = mouseRelative.ToRotation() + (float)PI;
                }
				else
				{
					myRotation = mouseRelative.ToRotation() - (float)PI;
				}
				player.direction = -1; // make player face left
			}
			else // cursor is to the right of the player
			{
				myRotation = mouseRelative.ToRotation();
				player.direction = 1; // make player face right
			}
			player.itemRotation = myRotation;
		}
	}
}