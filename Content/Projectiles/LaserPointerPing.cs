using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Enums;
using static System.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles
{
    public class LaserPointerPing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 500;
            Projectile.extraUpdates = 99;
            Projectile.hide = true;
            DrawOffsetX = -16;
            DrawOriginOffsetY = -16;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Main.player[Projectile.owner].team switch
            {
                1 => new(.9f, .3f, .3f, .85f), // Red
                2 => new(.3f, .9f, .3f, .85f), // Green
                3 => new(.4f, .8f, .8f, .8f), // Blue
                4 => new(.8f, .8f, .25f, .8f), // Yellow
                5 => new(.8f, .25f, .75f, .8f), // Pink
                _ => new(.95f, .95f, .95f, .85f) // No team
            };
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            OnHitAll();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            OnHitAll();
            return false;
        }
        public void OnHitAll()
        {
            Projectile.hide = false;
            Projectile.extraUpdates = 1;
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = 540;
            SoundEngine.PlaySound(SoundID.Item67 with { Volume = .5f }, Projectile.Center);
        }
    }
}
        