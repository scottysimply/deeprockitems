using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles.CryoCannonProjectiles
{
    public class CryoProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 45;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public float VelocityDecay { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public float CoolingAmount { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public float AliveTime { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void OnSpawn(IEntitySource source)
        {
            CoolingAmount = -8f;
            VelocityDecay = 0.95f;
        }
        public override void AI()
        {
            // Draw dust like the flamethrower does
            if (AliveTime++ % 2 == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: 2f);
                dust.customData = "CC";
                dust.alpha = 100;
            }
            // Slow down!
            Projectile.velocity *= VelocityDecay;

            // Kill older dust
            foreach (Dust dust in Main.dust)
            {
                if (dust.customData is string cc && cc == "CC")
                {
                    dust.scale *= 0.99f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ChangeTemperature((sbyte)(int)CoolingAmount, Projectile.owner);
        }
    }
}
