using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static System.Math;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaPistolHelper : HeldProjectileBase
    {
        public override double Spread => 0;
        public override int ProjectileToSpawn { get; set; } = ModContent.ProjectileType<PlasmaBullet>();
        public override SoundStyle? ChargeSound => SoundID.Item117;
        public override SoundStyle? FireSound => SoundID.Item114;
        public override float ChargeTime { get; set; } = 75f;
        public override void WhenReachedFullCharge()
        {
            ProjectileToSpawn = ModContent.ProjectileType<BigPlasma>();
            Projectile.velocity *= .4f;
            Projectile.damage *= 2;
            Spread = 0;
            projectileOwner.CheckMana(15, true);

            Projectile.Kill();
            
        }
        public override void SpecialAI()
        {
            // This section is used for playing a sound to help time the projectile
            float critical_time = ChargeTime / 3; // This is how often we're going to play the sound effect
            float charge_timer = Projectile.timeLeft - (int)ProjectileTime; // Adjusted timeLeft, just saves us a step.
            
            // Actually play the sound
            if (charge_timer % critical_time < 1 && charge_timer < ChargeTime && Projectile.timeLeft != 0)
            {
                projectileOwner.itemLocation = projectileOwner.ShakeWeapon(3);
                float dustSpeedX = Main.rand.NextFloat(-.1f, .1f);
                float dustSpeedY = Main.rand.NextFloat(-.1f, .1f);
                Terraria.Dust dust = Terraria.Dust.NewDustDirect(projectileOwner.position, projectileOwner.width, projectileOwner.height, DustID.Obsidian, dustSpeedX, dustSpeedY);
                //dust.noGravity = true;
                SoundEngine.PlaySound(SoundID.MaxMana with { Volume = .7f, Pitch = .2f });
            }


        }
    }
}
