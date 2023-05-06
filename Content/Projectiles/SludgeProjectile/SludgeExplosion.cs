using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 72;
            Projectile.scale = 2f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            DrawOffsetX = Projectile.height / 2;
            DrawOriginOffsetY = Projectile.height / 2;

        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 150);
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
    }
}
