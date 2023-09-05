﻿using Terraria.ID;
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
            Spread = PI / 40;
        }
        // This sound is going to play when the projectile fully charges :)
        public override SoundStyle Charge_Sound => SoundID.Item117;
        public override SoundStyle Fire_Sound => SoundID.Item114;
        public override float CHARGE_TIME { get; set; } = 75f;
        public override void AtFullCharge()
        {
            ProjectileToSpawn = ModContent.ProjectileType<BigPlasma>();
            Projectile.velocity *= .4f;
            Projectile.damage *= 4;
            Spread = 0;

            Projectile.Kill();
        }
        public override void SpecialAI()
        {
            // This section is used for playing a sound to help time the projectile
            float critical_time = CHARGE_TIME / 3; // This is how often we're going to play the sound effect
            float charge_timer = Projectile.timeLeft - BUFFER_TIME; // Adjusted timeLeft, just saves us a step.

            // Actually play the sound
            if (charge_timer % critical_time < 1 && charge_timer < CHARGE_TIME && Projectile.timeLeft != 0)
            {
                SoundEngine.PlaySound(SoundID.MaxMana with { Volume = .7f, Pitch = .2f });
            }


        }
    }
}
