using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Pets.Molly;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Common.Quests
{
    public class QuestRewardSystem : ModSystem
    {
        /// <summary>
        /// Common rewards make up the majority of quest rewards.
        /// Number of rewards scales with each vanilla boss downed.
        /// </summary>
        public static List<QuestReward> CommonRewards { get; set; }
        /// <summary>
        /// Rare rewards serve as a boost to the gameplay loop.
        /// Chance scales with total quests completed.
        /// </summary>
        [Obsolete("Rare rewards are not currently implemented.")]
        public static List<QuestReward> RareRewards { get; set; }
        /// <summary>
        /// These rewards are seldom rewarded, but they act as alternative ways to obtain rare items, or are outright new items altogether.
        /// Doing multiple quests in a session will greatly increase the chances of getting these rewards.
        /// </summary>
        public static List<QuestReward> UniqueRewards { get; private set; }
        public override void OnWorldLoad() {
            RecalculateQuests();
        }
        private void RecalculateQuests() {
            // Create list of rewards
            CommonRewards = new List<QuestReward>()
            {
                // Note for those contributing: 10 is the default weight to keep everything equally likely.
                new(ItemID.SilverCoin, 75, true, 15),
                new(ItemID.GoldCoin, 1, true, 15),
                new(ItemID.EndurancePotion, 5, true, 1),
                new(ItemID.MiningPotion, 5, true, 1),
                new(ItemID.BuilderPotion, 5, true, 1),
                new(ItemID.PotionOfReturn, 5, true, 1),
                new(ItemID.SpelunkerPotion, 5, true, 1),
                new(ItemID.RagePotion, 5, WorldGen.crimson, 1),
                new(ItemID.WrathPotion, 5, !WorldGen.crimson, 1),
                new(ItemID.LifeforcePotion, 5, true, 1),
                new(ItemID.SummoningPotion, 5, true, 1),
                new(ItemID.HallowedBar, 15, NPC.downedMechBossAny, 10),
                new(ItemID.LifeFruit, 3, NPC.downedMechBossAny, 10),
                new(ItemID.SoulofMight, 10, NPC.downedMechBoss1, 10),
                new(ItemID.SoulofSight, 10, NPC.downedMechBoss2),
                new(ItemID.SoulofFright, 10, NPC.downedMechBoss3),
            };
            UniqueRewards = new()
            {
                new(ModContent.ItemType<ChunkOfNitra>(), 1, NPC.downedBoss3, 1),
                // Able to get weapons through quests as a treat.
                new(ModContent.ItemType<M1000>(), 1, NPC.downedBoss3, 1),
                new(ModContent.ItemType<SludgePump>(), 1, NPC.downedBoss3, 1),
                new(ModContent.ItemType<Zhukovs>(), 1, Main.hardMode, 1),
                new(ModContent.ItemType<PlasmaPistol>(), 1, true, 1),
                new(ModContent.ItemType<JuryShotgun>(), 1, Main.hardMode, 1),
            };
        }
    }
    public class QuestReward
    {
        /// <summary>
        /// What ItemID will be rewarded for this quest
        /// </summary>
        public int RewardType { get; set; }
        /// <summary>
        /// Number of rewards to give
        /// </summary>
        public int RewardAmount { get; set; }
        /// <summary>
        /// The condition required to make this reward available. Supplying true results in the quest always being available. <br/>
        /// Use this to make certain rewards available at different stages in the game.
        /// </summary>
        public bool Condition { get; set; }
        /// <summary>
        /// The cumulative weight of this item in the RNG pool
        /// </summary>
        public int Weight { get; set; }
        public QuestReward(int rewardType, int rewardAmount, bool condition, int weight = 10) {
            RewardType = rewardType;
            RewardAmount = rewardAmount;
            Condition = condition;
            Weight = weight;
        }
    }
    public enum QuestRewardType
    {
        None = 0,
        Common, // Common rewards are the bulk of a givens quest reward.
        Rare, // Rare rewards get more common with the total amount of quests completed.
        Unique, // Unique rewards are items like accessories or upgrades, scaling with the amount of quests completed per session.

    }
}