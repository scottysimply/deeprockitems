using Terraria;

namespace deeprockitems.Utilities
{
    public static class ProjectileExtension
    {
        /// <summary>
        /// Determines if the given projectile is colliding with a desired projectile type.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="targetType"></param>
        /// <returns>Returns the whoAmI if <see cref="projectile"/> intersects with a target type. Returns -1 if no collision occured.</returns>
        public static int IsCollidingWithProjectile(this Projectile projectile, int targetType)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (!Main.projectile[i].active) continue;
                if (Main.projectile[i].type != targetType) continue;
                if (projectile.Hitbox.Intersects(Main.projectile[i].Hitbox)) return i;
            }
            return -1;
        }
    }
}
