using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class CryoMineletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
        }
        private float armingTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
    }
}
