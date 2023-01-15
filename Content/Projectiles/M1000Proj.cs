using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static System.Math;

namespace deeprockitems.Content.Projectiles
{
    public class M1000Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 7;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.height, Projectile.width);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
        public override void AI()
        {
            if (Projectile.ai[0] == Projectile.ai[1]) // ai 1 is max charge, ai0 is current charge
            {
                Projectile.penetrate = 5;
                Projectile.ai[1] = 0f;
            }
            Lighting.AddLight(Projectile.position, new Vector3 (0.55f, 0.35f, 0.1f));
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)Floor(Projectile.damage *.7);
        }
    }
}