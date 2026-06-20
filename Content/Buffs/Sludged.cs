using System.Linq;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Sludged : InstancedBuff
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            int dps = StrongSludge ? 23 : 15;
            npc.lifeRegen -= dps * 2;
            damage = dps;
            if (SlowingSludge)
            {
                // Update movement
                if (npc.noGravity)
                {
                    // Flying NPCs get vertical slowing
                    npc.position.Y -= npc.velocity.Y * (1 - SlowMultiplier);
                }
                // All NPCs get horizontal slowing
                npc.position.X -= npc.velocity.X * (1 - SlowMultiplier);
            }
            if (AmContagious)
            {
                // Get all NPCs around this npc
                var query = Main.npc.Where(n => n.active && npc.Center.DistanceSQ(n.Center) <= 9162);
                foreach (NPC n in query)
                {
                    // If an NPC is not sludged, add the buff
                    if (!n.HasInstancedBuff<Sludged>(out _))
                    {
                        n.AddInstancedBuff<Sludged>(TimeLeft, out _);
                    }
                }
            }
        }
        private float SlowMultiplier => 0.75f;
        public bool SlowingSludge { get; set; } = false;
        public bool StrongSludge { get; set; } = false;
        public bool AmContagious { get; set; } = false;
    }
}