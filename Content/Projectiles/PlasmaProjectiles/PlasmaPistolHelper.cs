using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static System.Math;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaPistolHelper : HeldProjectileBase
    {
        public PlasmaPistolHelper()
        {
            ProjectileToSpawn = ModContent.ProjectileType<PlasmaBullet>();
            Cooldown = 3;
            Spread = PI / 48;
        }
        // This sound is going to play when the projectile fully charges :)
        public override SoundStyle Charge_Sound => SoundID.Item117;
        public override SoundStyle Fire_Sound => SoundID.Item114;
        public override float CHARGE_TIME => 75f;
        public override void AtFullCharge()
        {
            ProjectileToSpawn = ModContent.ProjectileType<PlasmaBomb>();
            Projectile.velocity *= .4f;
            Projectile.damage *= 4;
            Spread = 0;

            Projectile.Kill();
        }
    }
}
