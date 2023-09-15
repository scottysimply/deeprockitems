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



            
            /*if (modPlayer.CurrentQuestInformation[0] == 3 && modPlayer.CurrentQuestInformation[1] == npc.type && npc.lastInteraction == Main.myPlayer)
            {
                modPlayer.CurrentQuestInformation[3]--;
                QuestsBase.DecrementProgress(modPlayer);
            }*/


            int bannerID = Item.NPCtoBanner(BannerID(npc.type, npc.netID)); // Fingers crossed, this converts the NPC id correctly...
            // Check if quest is fighting, if quest pertains to this NPC, and if the client got the kill
            if (modPlayer.CurrentQuestInformation[0] == 3 && Item.NPCtoBanner(modPlayer.CurrentQuestInformation[1]) == bannerID && npc.lastInteraction == Main.myPlayer)
            {
                // Decrease progress
                modPlayer.CurrentQuestInformation[3]--;
                QuestsBase.DecrementProgress(modPlayer);
            }
        }
        private static int BannerID(int type, int netID)
        {
            if (netID >= -10)
            {
                return netID;
            }
            return type;
        }
    }
}
