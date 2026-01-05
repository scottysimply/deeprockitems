using System.Linq;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Sludged : InstancedBuff
    {
        public override void UpdateLifeRegenNPC(NPC npc, ref int damage) {
            int dps = StrongSludge ? 30 : 15;
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
                    // If an NPC has the buff but is not contagious, add buff
                    if (n.HasInstancedBuff(out Sludged buff))
                    {
                        if (buff.AmContagious) continue;

                        n.AddInstancedBuff<Sludged>(180, out _);
                        continue;
                    }
                    // Else, add buff regardless
                    n.AddInstancedBuff<Sludged>(180, out _);
                }
            }
        }
        private float SlowMultiplier => 0.75f;
        public bool SlowingSludge { get; set; } = false;
        public bool StrongSludge { get; set; } = false;
        public bool AmContagious { get; set; } = false;
    }
}