using deeprockitems.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeHelper : HeldProjectileBase
    {
        public override int ProjectileToSpawn { get; set; } = ModContent.ProjectileType<SludgeBall>();
        public override float ChargeTime { get; set; } = 50f;
        public override SoundStyle? ChargeSound => DRGSoundIDs.SludgePumpFocus with { Volume = .8f, PitchVariance = 1f};
        public override SoundStyle? FireSound => DRGSoundIDs.SludgePumpFire with { Volume = .5f, PitchVariance = .75f};
        public override void NewSetDefaults() {
            ChargeShotCooldownMultiplier = 2f;
            ChargeShotDamageMultiplier = 2f;
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            if (!HasReachedFullCharge) return;

            (projectile.ModProjectile as SludgeBall).ShouldSplatter = true;
            (projectile.ModProjectile as SludgeBall).NumProjectilesToSpawn = 8;
        }
    }
}