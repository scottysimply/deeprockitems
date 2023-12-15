using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class CryoMineletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.velocity = Vector2.Zero;
        }
        private Point _collidedTile;
        private float armingTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void AI()
        {
            if (armingTimer-- > 0) return;

            // Kill projectile if the attached block is removed.
            /*if (!Main.tile[_collidedTile].HasTile)
            {
                Projectile.Kill();
            }*/
            
            // Detect if enemy is above projectile:

        }
       /* public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X > Projectile.velocity.X)
            {
                _tileCollideSide = TileDirection.Left;
                return false;
            }
            if (oldVelocity.X < Projectile.velocity.X)
            {
                _tileCollideSide = TileDirection.Right;
                return false;
            }
            if (oldVelocity.Y > Projectile.velocity.Y)
            {
                _tileCollideSide = TileDirection.Up;
                return false;
            }
            if (oldVelocity.Y < Projectile.velocity.Y)
            {
                _tileCollideSide = TileDirection.Down;
                return false;
            }

        }*/
        /// <summary>
        /// This will kill the projectile, but also spawn some dust for when an enemy gets frozen.
        /// </summary>
        private void FreezeAndKillSelf(NPC target)
        {


        }
    }
}
