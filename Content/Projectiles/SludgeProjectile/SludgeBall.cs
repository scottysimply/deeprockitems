using deeprockitems.Audio;
using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static System.Math;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeBall : ModProjectile
    {
        public bool ShouldSplatter { get; set; } = false;
        public int NumProjectilesToSpawn { get; set; } = 8;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = 0;
            DrawOffsetX = -8;
            DrawOriginOffsetY = -8;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y <= 30f) // Set gravity cap
            {
                Projectile.velocity.Y += .5f;
            }

            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.AddInstancedBuff(300, out Sludged? buff))
            {
                buff.SlowingSludge = Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("SlowingSludge");
                buff.StrongSludge = Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("StrongSludge");
                buff.AmContagious = Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("SpreadingSludge");
            }        
        }
        public override void OnKill(int timeLeft)
        {
            // Hit effects, dusts, sound
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(DRGSoundIDs.SludgeBallHit with { Volume = .3f }, Projectile.position);
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SludgeDust>(), Scale: Main.rand.NextFloat(1.1f, 1.5f));
            }

            // Check if projectile should splatter
            if (ShouldSplatter && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < NumProjectilesToSpawn; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}