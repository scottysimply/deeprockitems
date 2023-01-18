using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static System.Math;
using static Humanizer.In;

namespace deeprockitems.Content.Projectiles
{
    public class SludgeBall : ModProjectile
    {
        bool charged = false;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = Main.rand.NextFloat(0f, 6.28f);
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y != 10f) // cap gravity
            {
                Projectile.velocity.Y += .5f;
            }
            if (Projectile.ai[0] == Projectile.ai[1]) // it is fully charged
            {
                charged = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpHit") with {Volume = .3f}, Projectile.position);
            if (charged)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor((float)Projectile.damage * .3), Projectile.knockBack, Main.myPlayer);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (charged)
            {
                target.AddBuff(BuffID.Venom, 600);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 150);
            }
        }
    }
}