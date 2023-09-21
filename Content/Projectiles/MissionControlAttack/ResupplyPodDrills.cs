using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

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
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
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
                // Velocity will always equal 10, unless digging (we set it to 6)
                Projectile.velocity = new(0, 10);
                Point tileCoords = Projectile.Bottom.ToTileCoordinates();
                if (WorldGen.SolidTile(tileCoords))
                {
                    // Draw dust when the projectile "drills"
                    for (int i = 0; i < 2; i++)
                    {
                        WorldGen.KillTile(tileCoords.X, tileCoords.Y, fail: true, effectOnly: true);
                    }
                    if (Projectile.frameCounter % 2 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                    }
                    // Projectile.velocity *= .3f;
                }
                // If the projectile has fallen below the desired NPC's height
                if (Projectile.ai[0] <= Projectile.Center.Y)
                {
                    Projectile.tileCollide = true; // The projectile will now die on contact with its next tile.
                }

                
                if (Projectile.frameCounter++ % 4 == 0)
                {
                    Projectile.frame = ++Projectile.frame % 3;
                }
            }
            else
            {
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
