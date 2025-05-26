using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Buffs;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles.CryoCannonProjectiles
{
    public class IceSpear : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.height = Projectile.width = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 1;
        }
        public float CoolingAmount { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public float AliveTime { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void OnSpawn(IEntitySource source) {
            CoolingAmount = -10f;
        }
        public override void AI() {
            // Draw dust like the flamethrower does
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: 2f);
                dust.customData = "CC";
                dust.alpha = 100;
            }

            // Kill older dust
            foreach (Dust d in Main.dust)
            {
                if (d.customData is string cc && cc == "CC")
                {
                    d.scale *= 0.99f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.ChangeTemperature((sbyte)(int)CoolingAmount, Projectile.owner);
        }
    }
}
