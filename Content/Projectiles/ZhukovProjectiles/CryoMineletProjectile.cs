using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Utilities;
using Terraria.ID;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class CryoMineletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
        }
        private Point _collidedTile;
        private float armingTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void OnSpawn(IEntitySource source)
        {
            _collidedTile = new((int)Projectile.ai[1], (int)Projectile.ai[2]);
            Projectile.ai[1] = Projectile.ai[2] = 0f;
        }
        public override void AI()
        {
            if (armingTimer-- > 0) return;

            // Kill projectile if its embedded tile was removed.
            if (!Main.tile[_collidedTile].HasTile)
            {
                Projectile.Kill();
                return;
            }

            // Detect if any enemy is in close range:
            foreach (var npc in Main.npc)
            {
                if (npc.friendly) continue;
                if (Projectile.Center.DistanceSQ(npc.Center) <= 2500 && CollisionHelpers.CanHitLine(Projectile, npc))
                {
                    // Freeze all enemies in range!
                    FreezeAllNPCsInRange();
                    SpawnNiceDust();
                    Projectile.Kill();
                    return;
                }
            }

        }
        /// <summary>
        /// Freeze all NPCs in range of NPC.
        /// </summary>
        private void FreezeAllNPCsInRange()
        {
            // If enemies are nearby
            foreach (var npc in Main.npc)
            {
                if (Projectile.Center.DistanceSQ(npc.Center) <= 2500)
                {
                    // Apply frostburn as a test.
                    npc.AddBuff(BuffID.Frostburn, 120);
                }
            }
        }
        private void SpawnNiceDust()
        {
            const int DUST_COUNT = 15;
            const int WIDTH = 30;
            const int HEIGHT = 30;
            for (int i = 0; i < DUST_COUNT; i++)
            {
                Dust.NewDust(new Vector2(Projectile.Center.X - WIDTH / 2, Projectile.Center.Y - HEIGHT / 2), WIDTH, HEIGHT, DustID.SnowflakeIce, SpeedX: Main.rand.NextFloat(-2f, +2f), SpeedY: Main.rand.NextFloat(-0.5f, 2f), Scale: Main.rand.NextFloat(0.75f, 1.75f));
            }
        }
    }
}
