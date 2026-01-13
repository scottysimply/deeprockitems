using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaPistolHelper : HeldProjectileBase
    {
        public override int ProjectileToSpawn { get; set; } = ModContent.ProjectileType<PlasmaBullet>();
        public override SoundStyle? ChargeSound { get; set; } = SoundID.Item117;
        public override SoundStyle? FireSound { get; set; } = SoundID.Item114;
        public override float ChargeTime { get; set; } = 45f;
        private static bool _noSpreadOnNextShot = false;
        public override void NewSetDefaults() {
            ChargeShotDamageMultiplier = 2f;
            ChargeShotCooldownMultiplier = 6f;
        }
        public override void WhenReachedFullCharge()
        {
            ProjectileToSpawn = ModContent.ProjectileType<BigPlasma>();
            Projectile.velocity *= .4f;
            Spread = 0;
            FireSound = SoundID.Item105;
            Cooldown = 4;
            _noSpreadOnNextShot = true;
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) {
            if (_noSpreadOnNextShot && type == ModContent.ProjectileType<PlasmaBullet>())
            {
                spread = 0;
                _noSpreadOnNextShot = false;
            }
        }
        public override void ModifyProjectileAfterSpawning(Projectile projectile) {
            if (ProjectileToSpawn == ModContent.ProjectileType<BigPlasma>())
            {
                Main.player[Projectile.owner].CheckMana(7, true, false);
            }
        }
    }
}
