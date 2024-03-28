using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using deeprockitems.Common.Types;

namespace deeprockitems.Common.Weapons
{
    public class EntityTrackerPlayer : ModPlayer
    {
        public override void SetStaticDefaults()
        {
            EmbeddedDetsTrackedNPCs = new(25);
        }
        /// <summary>
        /// This is the list of NPCs with embedded detonators in it.
        /// </summary>
        private static FixedQueue<int> EmbeddedDetsTrackedNPCs;
        public static void AddNPCToTracker(NPC npc)
        {
            EmbeddedDetsTrackedNPCs.Enqueue(npc.whoAmI);
        }
        public static int[] RetrieveNPCsFromTracker(bool dequeue = true)
        {
            int[] npc_array = EmbeddedDetsTrackedNPCs.ToArray();
            if (dequeue)
            {
                EmbeddedDetsTrackedNPCs.Clear();
            }
            return npc_array;
        }
        public override void PostUpdate()
        {
            
        }
    }
}
