using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Utilities;
using Terraria.ID;
using Terraria.Audio;

namespace deeprockitems.Content.Projectiles.ZhukovProjectiles
{
    public class CryoMineletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
        }
        private Point _collidedTile;
        private float currentState { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        private float armingTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override void OnSpawn(IEntitySource source)
        {
            _collidedTile = new((int)Projectile.ai[1], (int)Projectile.ai[2]);
            Projectile.ai[1] = Projectile.ai[2] = 0f;
        }
        public override void AI()
        {
            // Current State 1 means the projectile is ready to freeze enemies
            if (currentState == 1f)
            {
                // Delay before projectile freezes enemies
                if (armingTimer-- > 0)
                {
                    return;
                }

                // Freeze all enemies in range!
                FreezeAllNPCsInRange();
                SpawnNiceDust();
                Projectile.Kill();
                return;
            }
            
            /* Check for enemies code */
            // Delay before the projectile is "armed"
            if (armingTimer-- > 0)
            {
                return;
            }

            // Kill projectile if its embedded tile was removed.
            if (!Main.tile[_collidedTile].HasTile)
            {
                Projectile.Kill();
                return;
            }

            // Detect if any enemy is in close range:
            foreach (var npc in Main.npc)
            {
                // Don't activate for town NPCs or critters.
                if (npc.friendly || npc.CountsAsACritter)
                {
                    continue;
                }

                // If the projectile is within 4.75 blocks of an enemy
                if (Projectile.Center.DistanceSQ(npc.Center) <= 5000)
                {
                    // Set current state. We'll wait a short while before freezing enemies.
                    currentState = 1f;
                    armingTimer = 10f; // Time to wait (in ticks).
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
                if (Projectile.Center.DistanceSQ(npc.Center) <= 5000)
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
            SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, position: Projectile.Center);
        }
    }
}
