using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;

namespace deeprockitems.Common.Items
{
    public class DRGQuestsModPlayer : ModPlayer
    {
        public int QuestsCompleted { get; set; } = 0;
        public int QuestsCompletedThisSession { get; set; } = 0;
        public bool PlayerHasClaimedRewards { get; set; } = true;
        /// <summary>
        /// Information regarding the current quest. <br></br>
        /// [0] = Quest type. 1 : Mining, 2 : Gather, 3 : Fighting. Additionally, 0 : Quest available, 99 : Quest completed, and rewards available, -1 : Quest unavailable <br></br>
        /// [1] = What ID will be looked for. Quest type 1 : ID will be TileID. Quest type 2 : ItemID. Type 3 : NPCID <br></br>
        /// [2] = How much of an ID will be required. Presumably, no explanation needed.
        /// [3] = Current quest progress.
        /// </summary>
        public int[] CurrentQuestInformation { get; set; } = new int[4];

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("DRGQuestsCompleted") && tag.ContainsKey("DRGHasClaimedRewards") && tag.ContainsKey("DRGQuestInformation"))
            {
                QuestsCompleted = (int)tag["DRGQuestsCompleted"];
                PlayerHasClaimedRewards = tag.GetBool("DRGHasClaimedRewards");
                CurrentQuestInformation = (int[])tag["DRGQuestInformation"];
            }
            base.LoadData(tag);
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("DRGQuestsCompleted", QuestsCompleted);
            tag.Add("DRGHasClaimedRewards", PlayerHasClaimedRewards);
            tag.Add("DRGQuestInformation", CurrentQuestInformation);
            base.SaveData(tag);
        }
    }
}
