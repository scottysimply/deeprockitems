using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.ID;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestsBase : ModSystem
    {
        private static bool oldDay = true;
        public static List<int> MiningQuestTypes { get; set; } = new List<int>();
        public static List<int> MiningQuestAmounts { get; set; } = new List<int>();
        public static List<int> GatherQuestTypes { get; set; } = new List<int>();
        public static List<int> GatherQuestAmounts { get; set; } = new List<int>();
        public static List<int> FightQuestTypes { get; set; } = new List<int>();
        public static List<int> FightQuestAmounts { get; set; } = new List<int>();
        /// <summary>
        /// Information regarding the current quest. <br></br>
        /// CurrentQuest[0] = Quest type. 1 : Mining, 2 : Gather, 3 : Fighting. Additionally, 0 : Quest available (time has reset), -1 : Quest unavailable (quest has been completed) <br></br>
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
        
        public static void UpdateQuests(DRGQuestsModPlayer modPlayer)
        {
            if (modPlayer.CurrentQuestInformation[3] <= 0 && modPlayer.CurrentQuestInformation[0] > 0)
            {
                modPlayer.CurrentQuestInformation[0] = -1; // Type -1 means no quest is ongoing.
            }
        }
        public static void DecrementProgress(DRGQuestsModPlayer modPlayer)
        {
            if (modPlayer is null) return; // Return if null.

            Rectangle rect = new Rectangle((int)modPlayer.Player.position.X - 10, (int)modPlayer.Player.position.Y, 40, 10);
            Color c = new(190, 60, 165);
            if (modPlayer.CurrentQuestInformation[3] > 0) // If quest was still ongoing
            {
                CombatText.NewText(rect, c, string.Format("{0} left!", modPlayer.CurrentQuestInformation[3]));
                return;
            }
            // Else if quest was finished

            AdvancedPopupRequest request = new AdvancedPopupRequest()
            {
                Text = "Quest complete!",
                Color = c,
                DurationInFrames = 180,
                Velocity = new(0, -20),
            };
            PopupText.NewText(request, modPlayer.Player.Center);
            modPlayer.PlayerHasClaimedRewards = false;
            modPlayer.CurrentQuestInformation[0] = -1;
        }
        /*public override void SaveWorldData(TagCompound tag)
        {
            tag["CurrentQuest"] = CurrentQuest;
            tag["QuestProgress"] = Progress;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            // THIS IS IMPORTANT! This allows the mod to load worlds that don't contain the saved key!
            // If this wasn't here, the world would be unloadable if the key wasn't found. In fact, it would just prevent any world from being loaded, since all worlds are pre-loaded.
            if (tag.ContainsKey("CurrentQuest") && tag.ContainsKey("QuestProgress"))
            {
                CurrentQuest = (int[])tag["CurrentQuest"];
                Progress = (int)tag["QuestProgress"];
            }
        }*/
        public override void SetStaticDefaults()
        {
            InitializeQuests();
        }
        public override void PostUpdateWorld()
        {
            
        }
        public override void PostUpdateTime()
        {
            if (Main.dayTime && !oldDay)
            {
                foreach (Player player in Main.player)
                {
                    if (!player.active) continue;
                    DRGQuestsModPlayer modPlayer = player.GetModPlayer<DRGQuestsModPlayer>();
                    modPlayer.PlayerHasClaimedRewards = false;
                    modPlayer.CurrentQuestInformation[3] = 0; // Reset progress
                    modPlayer.CurrentQuestInformation[0] = 0; // Reset quest type
                }
            }
            oldDay = Main.dayTime;

        }
        public static void Talk_CreateQuest(DRGQuestsModPlayer modPlayer)
        {
            // Randomize quest type
            int quest_type = Main.rand.Next(1, 4);
            switch (quest_type)
            {
                // Mining quest pulled
                case 1:
                    modPlayer.CurrentQuestInformation[0] = 1;
                    quest_type = Main.rand.Next(MiningQuestTypes.Count);
                    modPlayer.CurrentQuestInformation[1] = MiningQuestTypes[quest_type];
                    modPlayer.CurrentQuestInformation[2] = MiningQuestAmounts[quest_type];
                    break;

                // Gather quest pulled
                case 2:
                    modPlayer.CurrentQuestInformation[0] = 2;
                    quest_type = Main.rand.Next(GatherQuestTypes.Count);
                    modPlayer.CurrentQuestInformation[1] = GatherQuestTypes[quest_type];
                    modPlayer.CurrentQuestInformation[2] = GatherQuestAmounts[quest_type];
                    break;

                // Fighting quest pulled
                default:
                    modPlayer.CurrentQuestInformation[0] = 3;
                    quest_type = Main.rand.Next(FightQuestTypes.Count);
                    modPlayer.CurrentQuestInformation[1] = FightQuestTypes[quest_type];
                    modPlayer.CurrentQuestInformation[2] = FightQuestAmounts[quest_type];
                    break;
            }
            modPlayer.CurrentQuestInformation[3] = modPlayer.CurrentQuestInformation[2];
        }
    }

}
