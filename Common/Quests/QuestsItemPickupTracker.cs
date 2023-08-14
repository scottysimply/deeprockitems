using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestsItemPickupTracker : GlobalItem
    {
        private int old_stack = 0;
        public override bool InstancePerEntity => true;
        public override void UpdateInventory(Item item, Player player)
        {
            if (old_stack != item.stack)
            {
                old_stack = item.stack;
                if (QuestsBase.CurrentQuest[0] == 2 && QuestsBase.CurrentQuest[1] == item.type)
                {
                    QuestsBase.Progress = QuestsBase.CurrentQuest[2] - item.stack;
                    QuestsBase.DecrementProgress();
                }
            }

        }
    }
}
