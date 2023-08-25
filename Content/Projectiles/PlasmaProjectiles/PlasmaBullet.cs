using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
    }
}
