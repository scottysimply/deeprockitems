using Terraria.ModLoader;
using Terraria;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 240;
            Projectile.frame = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 3 == 0 && Projectile.frame < 2)
            {
                Projectile.frame++;
            }
            if (Projectile.frameCounter > 10)
            {
                Projectile.Kill();
            }
        }
    }
}
