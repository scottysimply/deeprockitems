using deeprockitems.Audio;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.Globals;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static System.Math;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeBall : ModProjectile
    {
        public bool ShouldSplatter { get => Projectile.ai[0] > 0f; set => Projectile.ai[0] = value ? 1f : -1f; }
        public int NumProjectilesToSpawn { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = 0;
            DrawOffsetX = -8;
            DrawOriginOffsetY = -8;
            NumProjectilesToSpawn = 8;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y <= 16f) // Gravity cap
            {
                Projectile.velocity.Y += .5f;
            }

            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddInstancedBuff(300, out Sludged _);
        }
        public override void OnKill(int timeLeft)
        {
            // Hit effects, dusts, sound
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(DRGSoundIDs.SludgeBallHit with { Volume = .3f }, Projectile.position);
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SludgeDust>(), Scale: Main.rand.NextFloat(1.1f, 1.5f));
            }

            // Check if projectile should splatter
            if (ShouldSplatter && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), Projectile.damage, 0f, Projectile.owner);
                for (int i = 0; i < NumProjectilesToSpawn; i++)
                {
                    float velocityAngle = -MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
                    const float spread = MathHelper.Pi;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit(velocityAngle - 0.5f * spread, spread) * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}