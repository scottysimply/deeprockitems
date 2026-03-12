using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Common.NPCs
{
    public class PrimitiveImmunityNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Dictionary<int, int> StaticImmunityFrames;
        public override void SetDefaults(NPC entity) {
            StaticImmunityFrames = [];
        }
        public override bool PreAI(NPC npc) {
            foreach (var timer in StaticImmunityFrames)
            {
                if (StaticImmunityFrames[timer.Key]-- <= 0)
                {
                    StaticImmunityFrames.Remove(timer.Key);
                }
            }
            return base.PreAI(npc);
        }
    }
}
