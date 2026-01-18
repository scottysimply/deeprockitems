using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaBullet : ModProjectile
    {
        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hostile = false;
            Projectile.ai[0] = -1f;
        }
        public bool IsExploding { get => Projectile.ai[0] > 0f; set => Projectile.ai[0] = value ? 1f : -1f; }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            if (IsExploding)
            {
                return;
            }
            hitbox = new Rectangle(hitbox.X + 23, hitbox.Y + 23, 14, 14);
        }
        public void Explode() {
            Projectile.Resize(60, 60);
            IsExploding = true;
            Projectile.frameCounter = 0;
            Projectile.frame = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 11;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
        }
        public override void AI() {
            // Normal AI
            if (!IsExploding)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter % 4 == 0)
                {
                    // Purple
                    Vector2 pos = Projectile.position + Projectile.velocity;
                    Dust.NewDust(pos, Projectile.width, Projectile.height, DustID.Shadowflame, SpeedX: 0.1f * Projectile.velocity.X, SpeedY: 0.1f * Projectile.velocity.Y, Scale: 0.75f, newColor: new Color(0.8f, 0.6f, 0.78f));
                }
                return;
            }
            // Exploding AI
            if (Projectile.frameCounter % 3 == 0 && Projectile.frame < 2)
            {
                Projectile.frame++;
            }
            if (Projectile.frameCounter > 10)
            {
                Projectile.Kill();
            }
        }
        public override bool PreKill(int timeLeft) {
            for (int i = 0; i < 4; i++)
            {
                float radial = 2 * MathHelper.Pi / 16f;
                Vector2 offset = new Vector2(MathF.Cos(radial), MathF.Sin(radial));
                Dust.NewDust(Projectile.position + 6 * offset, Projectile.width / 2, Projectile.height / 2, DustID.Shadowflame, SpeedX: 0.5f * offset.X, 0.5f * offset.Y);
            }
            return base.PreKill(timeLeft);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/PlasmaProjectile/PlasmaBullet";
        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(75, 22, 90).RGBToVector3());
            return true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
