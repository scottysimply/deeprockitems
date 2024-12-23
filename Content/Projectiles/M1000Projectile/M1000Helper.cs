using Terraria;
using Terraria.ID;
using Terraria.Audio;
using deeprockitems.Audio;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Helper : HeldProjectileBase
    {
        public override float ChargeTime { get; set; } = 30f;
        public override SoundStyle? ChargeSound => DRGSoundIDs.M1000Focus;
        public override SoundStyle? FireSound => DRGSoundIDs.M1000Fire;
        public override void NewSetDefaults() {
            ChargeShotCooldownMultiplier = 2f;
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            /*if (ProjectileToSpawn == ProjectileID.Bullet)
            {
                ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                projectile.penetrate = projectile.maxPenetrate = 1;
            }*/
        }
        public override void WhenReachedFullCharge()
        {
            Projectile.damage *= 2;
        }
    }
}