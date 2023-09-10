using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Common.Quests;

namespace deeprockitems.Common.Quests
{
    public class QuestsKillTracker : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc) // Used for fighting quests
        {
            // Convert our player to a ModPlayer
            DRGQuestsModPlayer modPlayer = Main.LocalPlayer.GetModPlayer<DRGQuestsModPlayer>();
            if (modPlayer is null) return; // Return if null.

            // Check if quest is fighting, if quest pertains to this NPC, and if the client got the kill
            if (modPlayer.CurrentQuestInformation[0] == 3 && modPlayer.CurrentQuestInformation[1] == npc.type && npc.lastInteraction == Main.myPlayer)
            {
                // Decrease progress
                modPlayer.CurrentQuestInformation[3]--;
                QuestsBase.DecrementProgress(modPlayer);
            }
        }
    }
}
