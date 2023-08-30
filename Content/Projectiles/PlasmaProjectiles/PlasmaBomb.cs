using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.Audio;
using Terraria.ID;
using System;

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
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation = 0;
            Projectile collidingProjectile = Projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());

            if (collidingProjectile is not null)
            {
                if (Projectile.owner == collidingProjectile.owner)
                {
                    // collidingProjectile.active = false;
                    collidingProjectile.Kill();
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f }); // Sound of the projectile 
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaProjectiles.PlasmaExplosion>(), Projectile.damage, .1f);
                    Projectile.Kill();
                }
            }
        }

    }
}
