using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Utilities
{
    public static class ProjectileExtension
    {
        /// <summary>
        /// Determines if the given projectile is colliding with a desired projectile type.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="target_ID"></param>
        /// <returns>Returns the instance of the colliding projectile. Returns null if no collision occured.</returns>
        public static Projectile? IsCollidingWithProjectile(this Projectile projectile, int target_ID)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.type == target_ID)
                {
                    continue;
                }
                if (projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    return proj;
                }
            }
            return null;
        }
    }
}
