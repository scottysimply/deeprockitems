using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Utilities
{
    public class ModInformation
    {
        /// <summary>
        /// Gets whether the given projectile type is from vanilla or not.
        /// </summary>
        /// <param name="type">The projectile ID.</param>
        /// <returns></returns>
        public static bool IsProjectileVanilla(int type)
        {
            return type <= ProjectileID.Count;
        }
        public static bool IsProjectileMyMod(int type)
        {
            if (ProjectileLoader.GetProjectile(type) is null)
            {
                return false;
            }
            return ProjectileLoader.GetProjectile(type).Mod == ModContent.GetInstance<deeprockitems>();
        }
    }
}
