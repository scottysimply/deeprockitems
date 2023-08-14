using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Common.Quests;

namespace deeprockitems.Common.Quests
{
    public class QuestsKillTracker : GlobalNPC
    {
        public override void OnKill(NPC npc) // Used for fighting quests
        {
            // Check if quest is fighting, and pertains to this NPC.
            if (QuestsBase.CurrentQuest[0] == 3 && QuestsBase.CurrentQuest[1] == npc.type)
            {
                // Check if player was responsible for damaging npc.
                if (npc.lastInteraction != 255) // not 255 means it was a player.
                {
                    QuestsBase.Progress--;
                    QuestsBase.DecrementProgress();
                }
            }
        }
    }
}
