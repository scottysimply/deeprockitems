using deeprockitems.Content.Items.Misc;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestModPlayer : ModPlayer
    {
        public Quest ActiveQuest { get; set; }
        public int TotalQuestsCompleted { get; set; }
        public int QuestsCompletedThisSession { get; set; }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("DRGQuestsCompleted") && tag.ContainsKey("DRGActiveQuest"))
            {
                TotalQuestsCompleted = (int)tag["DRGQuestsCompleted"];
                ActiveQuest = tag.Get<Quest>("DRGActiveQuest");
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("DRGQuestsCompleted", TotalQuestsCompleted);
            tag.Add("DRGActiveQuest", ActiveQuest);
            base.SaveData(tag);
        }
        public void UpdateQuestProgress(int newProgress)
        {
            // Add progress to quest if not completed
            if (!ActiveQuest.Completed)
            {
                ActiveQuest.Progress = newProgress;
                Color color = new(190, 60, 165);
                // If quest was completed afterawrd, give congratulatory message
                if (ActiveQuest.Completed)
                {
                    // Give popup message
                    AdvancedPopupRequest message = new AdvancedPopupRequest()
                    {
                        Text = "Quest complete!",
                        Color = color,
                        DurationInFrames = 180,
                        Velocity = new(0, -20),
                    };
                    PopupText.NewText(message, Player.Center);
                }
                else // Else, give progress message
                {
                    // Display combattext
                    Rectangle destination = new Rectangle((int)Player.position.X - 10, (int)Player.position.Y, 40, 10);
                    CombatText.NewText(destination, color, $"{ActiveQuest.Data.AmountRequired - ActiveQuest.Progress} left!");
                }
            }
            
        }
        public void GiveDeepRockReward()
        {
            // Increment quests completed
            TotalQuestsCompleted++;
            QuestsCompletedThisSession++;

            // Give matrix core after 4 quests
            if (TotalQuestsCompleted % 4 == 0)
            {
                Player.QuickSpawnItem(Player.GetSource_DropAsItem(), ModContent.ItemType<MatrixCore>());
            }

            // Get how many vanilla bosses were killed
            bool[] bosses =
            {
                NPC.downedBoss1,
                NPC.downedBoss2,
                NPC.downedBoss3,
                NPC.downedQueenBee,
                NPC.downedDeerclops,
                Main.hardMode,
                NPC.downedQueenSlime,
                NPC.downedMechBoss1,
                NPC.downedMechBoss2,
                NPC.downedMechBoss3,
                NPC.downedPlantBoss,
                NPC.downedGolemBoss,
                NPC.downedEmpressOfLight,
                NPC.downedFishron,
                NPC.downedAncientCultist,
            };

            // Get number of common reward rerolls using the number of bosses killed
            int commonRewardCount = (int)MathF.Floor(0.4f * bosses.Count(true) + 1);
            // Get chance increase to unique rewards. Doing 10 quests in a session will cap out the probability at 3x the base chance, which increases again in hardmode
            float uniqueChance = (Main.hardMode ? 1 / 60f : 1 / 120f) * MathF.Max(3 * QuestsCompletedThisSession / 10f, 3);

            // Generate max weight for common rewards
            int maxWeight = QuestRewardSystem.CommonRewards.Sum(r => r.Weight);

            // Pull common rewards
            for (int i = 0; i < commonRewardCount; i++)
            {
                // Generate weighted index
                int weightedIndex = Main.rand.Next(0, maxWeight);
                int weightToChoose = 0;
                int index = 0;

                // Convert to weighted index
                QuestRewardSystem.CommonRewards.ForEach((r) => {
                    weightToChoose += r.Weight;
                    if (weightToChoose >= weightedIndex)
                    {
                        // Spawn reward
                        Player.QuickSpawnItem(Player.GetSource_DropAsItem(), QuestRewardSystem.CommonRewards[index].RewardType);
                        return; // Break loop
                    }
                    index++;
                });
            }
            // Pull unique rewards
            float chance = Main.rand.NextFloat();
            if (chance <= uniqueChance)
            {
                // Pick random index
                int index = Main.rand.Next(0, QuestRewardSystem.UniqueRewards.Count);
                // Spawn reward
                Player.QuickSpawnItem(Player.GetSource_DropAsItem(), QuestRewardSystem.UniqueRewards[index].RewardType);
            }
        }
    }
}
/*using Terraria;
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
*/