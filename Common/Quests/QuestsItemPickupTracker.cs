using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using deeprockitems.Common.Items;

namespace deeprockitems.Common.Quests
{
    public class QuestsItemPickupTracker : GlobalItem
    {
        private int old_stack = 0;
        public override bool InstancePerEntity => true;
        public override void UpdateInventory(Item item, Player player)
        {
            // Convert our player to a ModPlayer
            DRGQuestsModPlayer modPlayer = player.GetModPlayer<DRGQuestsModPlayer>();
            if (modPlayer is null) return; // Return if null.


            if (old_stack != item.stack)
            {
                old_stack = item.stack;
                if (modPlayer.CurrentQuestInformation[0] == 2 && modPlayer.CurrentQuestInformation[1] == item.type)
                {
                    modPlayer.CurrentQuestInformation[3] = modPlayer.CurrentQuestInformation[2] - item.stack;
                    QuestsBase.DecrementProgress(modPlayer);
                }
            }

        }
    }
}
