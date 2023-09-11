using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using deeprockitems.Content.Items.Misc;
using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using MonoMod.Utils;
using System;

namespace deeprockitems.Common.Quests
{
    public class QuestsRewards : ModSystem
    {
        public static int RewardsTier { get; set; }
        public static List<int> UniqueRewards { get; set; }
        public static Dictionary<int, int> CommonPreHardmodeRewards { get; set; }
        public static Dictionary<int, int> CommonHardmodeRewards { get; set; }
        public static Dictionary<int, int> RarePreHardmodeRewards { get; set; }
        public static Dictionary<int, int> RareHardmodeRewards { get; set; }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("DRGRewardsTier", RewardsTier);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("DRGRewardsTier"))
            {
                RewardsTier = (int)tag["DRGRewardsTier"];
            }
        }
        public override void OnWorldLoad()
        {
            // Rarest tier of rewards, at roughly 1 in 15 quests. The odds never change. Only 1 item will be given of each since the majority of them are unstackable
            UniqueRewards = new List<int>()
            {
                ItemID.LeprechaunHat,
                ItemID.GoldenCrate,
                ItemID.LavaFishingHook,
                ItemID.ExtendoGrip,
                ItemID.DiggingMoleMinecart,
                ItemID.PaintSprayer,
                ItemID.BrickLayer,
                ItemID.PortableCementMixer,

            };
            // Common rewards are *always* given.
            CommonPreHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.SilverCoin] = 130,
                [ItemID.IronOre] = 25,
                [ItemID.LeadOre] = 25,
                [ItemID.SilverOre] = 30,
                [ItemID.TungstenOre] = 30,
                [ItemID.GoldOre] = 30,
                [ItemID.PlatinumOre] = 30,
                [ItemID.IronBar] = 5,
                [ItemID.LeadBar] = 5,
                [ItemID.SilverBar] = 7,
                [ItemID.TungstenBar] = 7,
                [ItemID.Bone] = 10,
                [ItemID.Bomb] = 7,
                [ItemID.Dynamite] = 2,
                [ItemID.Ale] = 10,


            };
            CommonHardmodeRewards = new Dictionary<int, int>
            {
                [ItemID.CobaltOre] = 25,
                [ItemID.PalladiumOre] = 25,
                [ItemID.MythrilOre] = 20,
                [ItemID.OrichalcumOre] = 20,
                [ItemID.CobaltBar] = 5,
                [ItemID.PalladiumBar] = 5,
                [ItemID.MythrilOre] = 4,
                [ItemID.OrichalcumBar] = 4,


            };
            // Adding pre-existing items
            CommonHardmodeRewards.AddRange(CommonPreHardmodeRewards);
            // Rare rewards are given less often, but they're not super rare. One every few quests at lower rates is fair.
            RarePreHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.EnchantedNightcrawler] = 3,
                [ItemID.GoldBar] = 8,
                [ItemID.PlatinumBar] = 8,
                [ItemID.BattlePotion] = 3,
                [ItemID.EndurancePotion] = 3,
                [ItemID.RagePotion] = 3,
                [ItemID.RagePotion] = 3,
                [ItemID.CalmingPotion] = 3,
                [ItemID.HeartreachPotion] = 3,
                [ItemID.LifeforcePotion] = 1,
                [ItemID.IronskinPotion] = 5,
                [ItemID.RegenerationPotion] = 5,
                [ItemID.SwiftnessPotion] = 5,
                [ItemID.ScarabBomb] = 5,
            };
            RareHardmodeRewards = new Dictionary<int, int>()
            {
                [ItemID.AdamantiteOre] = 15,
                [ItemID.TitaniumOre] = 15,
                [ItemID.AdamantiteBar] = 3,
                [ItemID.TitaniumBar] = 3,
                [ItemID.TeleportationPotion] = 3,
            };
            // Adding pre-existing items
            RareHardmodeRewards.AddRange(RarePreHardmodeRewards);
        }
        public static void IssueRewards(DRGQuestsModPlayer modPlayer)
        {
            RewardsTier = SetRewardTiers();
            modPlayer.QuestsCompleted += 1;
            modPlayer.QuestsCompletedThisSession += 1;

            // If quests are threeven
            if (modPlayer.QuestsCompleted % 3 == 0)
            {
                // We're giving matrix cores every 3 quests
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), ModContent.ItemType<MatrixCore>());
            }
            else
            {
                // Otherwise, give an upgrade token
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), ModContent.ItemType<UpgradeToken>());
            }

            // Generate items
            double weird_number = Math.Log(RewardsTier * 2 / 3 + 1.7); // This is just an overly complex variable
            int numCommonRewards = (int)Math.Floor(Math.Log(weird_number)) + Main.rand.Next(1, 3); // between 1 and 5 common rewards. I felt like using a complex generator :>
            int rareRewardChance = (int)Math.Ceiling(Math.Pow(weird_number - 1.3, 2) / 10 + .2) * 10; // Odds of getting a rare reward

            int uniqueRewardChance = ((modPlayer.QuestsCompletedThisSession + 1) ^ 2) / 3;

            Dictionary<int, int> commonRewardSet; // This is the pool of common rewards that the generator will pull from.
            Dictionary<int, int> rareRewardSet; // Pool of rare rewards to take from.
            Dictionary<int, int> rewardsAndAmounts = new(); // This is the rewards that will be given to the player after this.
            
            // If in hardmode, use hardmode reward set.
            if (Main.hardMode)
            {
                commonRewardSet = CommonHardmodeRewards;
                rareRewardSet = RareHardmodeRewards;
            }
            else
            {
                commonRewardSet = CommonPreHardmodeRewards;
                rareRewardSet = RarePreHardmodeRewards;
            }

            // Repeat given number of times
            for (int item = 0; item < numCommonRewards; item++)
            {
                int valueToTake = Main.rand.Next(commonRewardSet.Keys.ToList()); // which key to pull from
                if (rewardsAndAmounts.ContainsKey(valueToTake))
                {
                    rewardsAndAmounts[valueToTake] = (int)Math.Floor(rewardsAndAmounts[valueToTake] * 1.5); // If same quests pulled multiple times, "reward' the user for being lucky
                    continue; // Don't add key and value
                }
                rewardsAndAmounts.Add(valueToTake, commonRewardSet[valueToTake]); // Add corresponding key and value
            }
            // Check if should take rare reward
            if (Main.rand.NextBool(rareRewardChance, 10))
            {
                int valueToTake = Main.rand.Next(rareRewardSet.Keys.ToList()); // which key to pull from
                rewardsAndAmounts.Add(valueToTake, rareRewardSet[valueToTake]); // Add corresponding key and value
            }
            // Check if should take unique reward
            if (Main.rand.NextBool(uniqueRewardChance, 100))
            {
                int valueToTake = Main.rand.Next(UniqueRewards);
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), valueToTake);
            }

            // Convert rewards dictionary to items:
            foreach (KeyValuePair<int, int> values in rewardsAndAmounts)
            {
                float rewardsMultipler = (float)Main.rand.NextFloat(.85f, 1.15f);
                int amount = (int)Math.Ceiling(rewardsMultipler * values.Value);
                modPlayer.Player.QuickSpawnItem(NPC.GetSource_NaturalSpawn(), values.Key, amount);
            }




        }
        public static int SetRewardTiers()
        {
            if (NPC.downedMoonlord)
            {
                return 7;
            }
            if (NPC.downedGolemBoss)
            {
                return 6;
            }
            if (NPC.downedPlantBoss)
            {
                return 5;
            }
            // If all 3 mechs downed
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                return 4;
            }

            // If any of 2 mechs are downed
            if ((NPC.downedMechBoss1 && NPC.downedMechBoss2) || (NPC.downedMechBoss2 && NPC.downedMechBoss3) || (NPC.downedMechBoss1 && NPC.downedMechBoss3))
            {
                return 3;
            }
            // If any of 1 mechs are downed
            if (NPC.downedMechBossAny)
            {
                return 2;
            }
            if (Main.hardMode)
            {
                return 1;
            }
            return 0;
        }
    }
}
