using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.ID;
using System.Linq;
using Microsoft.Xna.Framework;

namespace deeprockitems.Common.Quests
{
    public static class QuestsBase
    {
        public static List<int> MiningQuestTypes { get; set; } = new List<int>();
        public static List<int> MiningQuestAmounts { get; set; } = new List<int>();
        public static List<int> GatherQuestTypes { get; set; } = new List<int>();
        public static List<int> GatherQuestAmounts { get; set; } = new List<int>();
        public static List<int> FightQuestTypes { get; set; } = new List<int>();
        public static List<int> FightQuestAmounts { get; set; } = new List<int>();
        /// <summary>
        /// Information regarding the current quest. <br></br>
        /// CurrentQuest[0] = Quest type. 1 : Mining, 2 : Gather, 3 : Fighting. Additionally, 0 : Quest available, 99 : Quest completed, and rewards available, -1 : Quest unavailable <br></br>
        /// CurrentQuest[1] = What ID will be looked for. Quest type 1 : ID will be TileID. Quest type 2 : ItemID. Type 3 : NPCID <br></br>
        /// CurrentQuest[2] = How much of an ID will be required. Presumably, no explanation needed.
        /// </summary>
        public static int[] CurrentQuest { get; set; } = new int[3];
        public static int Progress { get; set; }
        public static void InitializeQuests()
        {
            // Mining quest TileIDs
            MiningQuestTypes.AddThis(TileID.Copper)
                .AddThis(TileID.Iron)
                .AddThis(TileID.Silver)
                .AddThis(TileID.Gold)
                .AddThis(TileID.Amethyst)
                .AddThis(TileID.Topaz)
                .AddThis(TileID.Emerald)
                .AddThis(TileID.Sapphire)
                .AddThis(TileID.Ruby)
                .AddThis(TileID.Diamond);
            // Mining quest amounts
            MiningQuestAmounts.AddThis(200)
                .AddThis(150)
                .AddThis(125)
                .AddThis(100)
                .AddThis(20)
                .AddThis(20)
                .AddThis(15)
                .AddThis(15)
                .AddThis(10)
                .AddThis(5);

            // Gather quest ItemIDs
            GatherQuestTypes.AddThis(ItemID.SkyBlueFlower)
                .AddThis(ItemID.OrangeBloodroot)
                .AddThis(ItemID.YellowMarigold)
                .AddThis(ItemID.Sluggy)
                .AddThis(ItemID.Cobweb)
                .AddThis(ItemID.Bomb)
                .AddThis(ItemID.Daybloom)
                .AddThis(ItemID.Moonglow)
                .AddThis(ItemID.Blinkroot);
            // Gather quest amounts
            GatherQuestAmounts.AddThis(3)
                .AddThis(3)
                .AddThis(3)
                .AddThis(10)
                .AddThis(50)
                .AddThis(25)
                .AddThis(10)
                .AddThis(10)
                .AddThis(10);

            // Fighting quest NPCIDs
            FightQuestTypes.AddThis(NPCID.Skeleton)
                .AddThis(NPCID.GraniteGolem)
                .AddThis(NPCID.GraniteFlyer)
                .AddThis(NPCID.GreekSkeleton);
            // Fighting quest amounts
            FightQuestAmounts.AddThis(20)
                .AddThis(10)
                .AddThis(10)
                .AddThis(15);
        }
        public static void Talk_CreateQuest()
        {
            // Randomize quest type
            int quest_type = Main.rand.Next(1, 4);
            int playercount = Main.player.Take(Main.maxPlayers).Count(p => p.active);
            switch (quest_type)
            {
                // Mining quest pulled
                case 1:
                    CurrentQuest[0] = 1;
                    quest_type = Main.rand.Next(MiningQuestTypes.Count);
                    CurrentQuest[1] = MiningQuestTypes[quest_type];
                    CurrentQuest[2] = MiningQuestAmounts[quest_type] * playercount;
                    break;

                // Gather quest pulled
                case 2:
                    CurrentQuest[0] = 2;
                    quest_type = Main.rand.Next(GatherQuestTypes.Count);
                    CurrentQuest[1] = GatherQuestTypes[quest_type];
                    CurrentQuest[2] = GatherQuestAmounts[quest_type] * playercount;
                    break;

                // Fighting quest pulled
                default:
                    CurrentQuest[0] = 3;
                    quest_type = Main.rand.Next(FightQuestTypes.Count);
                    CurrentQuest[1] = FightQuestTypes[quest_type];
                    CurrentQuest[2] = FightQuestAmounts[quest_type] * playercount;
                    break;
            }
            Progress = CurrentQuest[2];
        }
        public static void UpdateQuests()
        {
            if (Progress <= 0 && CurrentQuest[0] > 0 && CurrentQuest[0] < 99)
            {
                CurrentQuest[0] = 99; // type 99 indicates completed, this means no quest can be started, and rewards will be given next time Mission Control is talked to.
            }
        }
        public static void DecrementProgress()
        {
            foreach (Player player in Main.player)
            {
                if (!player.active)
                {
                    continue;
                }
                Rectangle rect = new Rectangle((int)player.position.X - 10, (int)player.position.Y, 40, 10);
                Color c = new(190, 60, 165);
                if (Progress > 0)
                {
                    CombatText.NewText(rect, c, string.Format("{0} left!", Progress));
                    continue;
                }
                AdvancedPopupRequest request = new AdvancedPopupRequest()
                {
                    Text = "Quest complete!",
                    Color = c,
                    DurationInFrames = 180,
                    Velocity = new(0, -10)
                };
                PopupText.NewText(request, player.Center);
            }
        }
    }

}
