using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.Audio;
using Terraria.ID;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
        }
        public override bool PreAI()
        {
            Projectile.rotation = 0;
            if (Projectile.frame > 0)
            {
                return true;
            }
            Projectile collidingProjectile = Projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());
            
            if (collidingProjectile is not null)
            {
                if (Projectile.owner == collidingProjectile.owner)
                {
                    collidingProjectile.active = false;
                    collidingProjectile.Kill();
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f });
                    return true;
                }
            }
            return false;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.Resize(60, 60);

            if (Projectile.frameCounter % 2 == 0)
            {
                Projectile.frame++;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 7)
            {
                Projectile.Kill();
            }
        }

    }
}
