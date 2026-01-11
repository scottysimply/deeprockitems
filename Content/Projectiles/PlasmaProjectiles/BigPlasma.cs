using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class BigPlasma : ModProjectile // Darn big plasma.. and their exploding!
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.rotation = 0;
            Projectile.timeLeft = 600;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public bool CancelAoE { get => Projectile.ai[0] > 0f; set => Projectile.ai[0] = value ? 1f : -1f; }
        public bool IsExploding { get => Projectile.ai[1] > 0f; set => Projectile.ai[1] = value ? 1f : -1f; }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
            height = 24;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/PlasmaProjectile/BigPlasma";
        public override bool PreDraw(ref Color lightColor)
        {
            if (IsExploding) return false;
            Lighting.AddLight(Projectile.Center, new Vector3(100, 30, 120).RGBToVector3());
            return true;
        }
        public override void AI()
        {
            if (IsExploding) return;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 3 == 0)
            {
                Projectile.rotation = Main.rand.Next(0, 3) * MathHelper.PiOver2;
                Projectile.frame = Main.rand.Next(0, 3);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (!CancelAoE && !IsExploding && Projectile.owner == Main.myPlayer)
            {
                Explode();
                return false;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (!CancelAoE && !IsExploding && Projectile.owner == Main.myPlayer)
            {
                Explode();
            }
        }
        public void Explode() {
            IsExploding = true;
            Projectile.frameCounter = 0;
            Projectile.frame = 1;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.position += 2*Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Projectile.Resize(96, 96);
            Projectile.timeLeft = 2;
        }
        public override bool PreKill(int timeLeft) {
            for (int i = 0; i < 16; i++)
            {
                float radial = 2 * MathHelper.Pi / 16f;
                Vector2 offset = new Vector2(MathF.Cos(radial), MathF.Sin(radial));
                Dust.NewDust(Projectile.position + 6 * offset, Projectile.width / 2, Projectile.height / 2, DustID.Shadowflame, SpeedX: 0.5f * offset.X, 0.5f * offset.Y);
            }
            return base.PreKill(timeLeft);
        }
    }
}
