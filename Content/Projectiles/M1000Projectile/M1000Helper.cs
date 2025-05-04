using Terraria;
using Terraria.ID;
using Terraria.Audio;
using deeprockitems.Audio;
using Microsoft.Xna.Framework;
using deeprockitems.Utilities;

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
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.BulletHighVelocity;
            }
            if (!ModInformation.IsProjectileVanilla(type) && !ModInformation.IsProjectileMyMod(type))
            {
                type = ProjectileID.BulletHighVelocity;
            }
            if (!HasReachedFullCharge)
            {
                spread = MathHelper.Pi / 32;
            }
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            if (ProjectileToSpawn == ProjectileID.Bullet)
            {
                projectile.penetrate = projectile.maxPenetrate = 1;
            }
        }
        public override void WhenReachedFullCharge()
        {
            Projectile.damage *= 2;
        }
    }
}