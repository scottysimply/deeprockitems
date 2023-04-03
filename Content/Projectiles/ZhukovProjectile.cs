/*using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Projectiles
{
    public class ZhukovBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {

        }
        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, )
            }
        }
    }
    public class CryoMinelet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 720;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            if ()
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
*/