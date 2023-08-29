using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        public override bool PreAI()
        {
            Projectile collidingProjectile = Projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());
            
            if (collidingProjectile is not null)
            {
                if (Projectile.owner == collidingProjectile.owner)
                {
                    collidingProjectile.Kill();
                }
            }
            return false;
        }

    }
}
