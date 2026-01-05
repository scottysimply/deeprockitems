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
        /// Common rewards make up the bulk of rewards and act like angler quest/crate loot. <br/>
        /// Chances
        /// </summary>
        public static List<QuestReward> CommonRewards { get; set; }
        /// <summary>
        /// Rare rewards serve as a boost to the gameplay loop.
        /// Chance scales with total quests completed.
        /// </summary>
        public static List<QuestReward> RareRewards { get; set; }
        /// <summary>
        /// One of these is given after completing an assignment. <br/>
        /// Duplicates will not be given until all other rewards were given.
        /// </summary>
        public static List<QuestReward> AssignmentReward { get; private set; }
        public override void OnWorldLoad() {
            RecalculateRewards();
        }
        private void RecalculateRewards() {
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
                new(ItemID.SoulofMight, 5, NPC.downedMechBoss1, 10),
                new(ItemID.SoulofSight, 5, NPC.downedMechBoss2),
                new(ItemID.SoulofFright, 5, NPC.downedMechBoss3),
            };
            AssignmentReward = new()
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
}