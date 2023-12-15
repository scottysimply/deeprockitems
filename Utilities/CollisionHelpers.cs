using Terraria;
using Microsoft.Xna.Framework;

namespace deeprockitems.Utilities
{
    public static class CollisionHelpers
    {
        /// <summary>
        /// Performs <see cref="Collision.CanHitLine(Vector2, int, int, Vector2, int, int)"/> with only 2 entities.
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns></returns>
        public static bool CanHitLine(Entity entity1, Entity entity2)
        {
            return Collision.CanHitLine(entity1.position, entity1.width, entity1.height, entity2.position, entity2.width, entity2.height);
        }
    }
}
