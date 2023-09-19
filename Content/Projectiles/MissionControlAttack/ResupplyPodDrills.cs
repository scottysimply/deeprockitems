using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.MissionControlAttack
{
    public class ResupplyPodDrills : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = true;
            Projectile.velocity = new(0, 10);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        // These are the textures of the rest of the supply pod. The projectile is only the drill, since thats the only part that should deal damage.
        private readonly Texture2D body = ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/MissionControlAttack/ResupplyPodBody").Value;
        private readonly Texture2D glowmask = ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/MissionControlAttack/ResupplyPodBodyGlowMask").Value;
        // This is the draw position of the projectile, the top left of the
        Vector2 drawPos;
        public override bool PreDraw(ref Color lightColor)
        {
            drawPos = new Vector2(Projectile.position.X + 2, Projectile.position.Y - body.Height + 2) - Main.screenPosition; // Gonna need this
            float light_flash = 5 * (float)Math.Sin(Projectile.frameCounter / 30) + 5;
            // Manually drawing the rest of the fucking owl (resupply pod).
            Main.EntitySpriteDraw(body, drawPos, body.Bounds, new Color(new Vector4(Projectile.Opacity)), 0, new Vector2(0, 0), 1f, SpriteEffects.None);
            Main.EntitySpriteDraw(glowmask, drawPos, glowmask.Bounds, new Color(new Vector4(Projectile.Opacity) * light_flash), 0, new Vector2(0, 0), 1f, SpriteEffects.None);
            return true;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0) // ai[1] will determine if the projectile has hit anything since we want it to sync
            {
                // If the projectile has fallen below the desired NPC's height
                if (Projectile.Center.Y >= Projectile.ai[0])
                {
                    Projectile.tileCollide = true; // The projectile will now die on contact with its next tile.
                }
                // Velocity will always equal 10, unless digging (we set it to 6)
                Projectile.velocity = new(0, 10);
                Point tileCoords = Projectile.Center.ToTileCoordinates();
                if (Main.tile[tileCoords].HasUnactuatedTile)
                {   
                    // Draw dust when the projectile "drills"
                    for (int i = 0; i < 10; i++)
                    {
                        Collision.HitTiles(Projectile.position, new Vector2(0, 500), Projectile.width, Projectile.height);
                    }
                    Projectile.velocity = new(0, 6);
                }
                if (Projectile.frameCounter++ % 4 == 0)
                {
                    Projectile.frame = ++Projectile.frame % 3;
                }
            }
            else
            {
                if (Projectile.frameCounter > 180)
                {
                    Projectile.Kill();
                }
                Projectile.Opacity *= .97f;
            }
        }
        public override bool? CanDamage()
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.frameCounter = 0;
            Projectile.ai[1] = 1f;
            return false;
        }
    }
}
