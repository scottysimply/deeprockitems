using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles.Globals
{
    public class ProjectileSourcePasser : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public IEntitySource Source { get; set; }
        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            Source = source;
        }
    }
    public static class ProjectileSourceExtension {
        /// <summary>
        /// Gets the EntitySource responsible for spawning this projectile. Values are only relevant when the projectile is spawned. Returns an empty source if retrieved before OnSpawn() was called.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        public static IEntitySource GetSource(this Projectile projectile) {
            return projectile.GetGlobalProjectile<ProjectileSourcePasser>().Source ?? new EntitySource_Parent(null);
        }
    }
}
