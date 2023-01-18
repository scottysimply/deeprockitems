using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static System.Math;

namespace deeprockitems.Content.Projectiles
{
    public class M1000Better : ModProjectile
    {
        private const float MAX_CHARGE = 40f;
        
        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 3600;
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
            
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)Floor(Projectile.damage *.7);
        }
        public override void WeaponCharge(Projectile projectile, )
        {
            
        }
    }
}