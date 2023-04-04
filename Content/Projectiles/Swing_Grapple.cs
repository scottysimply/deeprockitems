using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles
{
    public class SwingGrapple : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.scale = 5f;

            Projectile.aiStyle = 0;
            DrawOriginOffsetY = 10;
        }
        Player owner;

        public override void OnSpawn(IEntitySource source)
        {
            owner = Main.player[Projectile.owner];
        }

        public override void AI()
        {
            if (Projectile.velocity != new Vector2(0, 0))
            {
                if (owner.Center.Distance(Projectile.Center) > 500f)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (owner.Center.X < Projectile.Center.X) // Accelerate player to the right if they are to the left of hook
                {
                    owner.velocity.X += .2f;
                }
                else // Accelerate player to the left if they are to the right of hook
                {
                    owner.velocity.X -= .2f;
                }
                if ((Projectile.Center.X - 10f < owner.Center.X) && (owner.Center.X < Projectile.Center.X + 10f)) // If the player is near the hook's X position:
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            owner.velocity = new();
            Projectile.velocity = new(0, 0);
            return false;
        }
    }
}
