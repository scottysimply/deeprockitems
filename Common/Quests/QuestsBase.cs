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
        public static Dictionary<int, int> MiningQuestInformation { get; set; } = new Dictionary<int, int>();
        public static Dictionary<int, int> GatherQuestInformation { get; set; } = new Dictionary<int, int>();
        public static Dictionary<int, int> FightingQuestInformation { get; set; } = new Dictionary<int, int>();
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
            MiningQuestInformation.Clear();
            GatherQuestInformation.Clear();
            FightingQuestInformation.Clear();
            /*int[] newAndShinyOres = { TileID.Tin, TileID.Lead, TileID.Tungsten, TileID.Platinum, TileID.Crimtane, TileID.Palladium, TileID.Orichalcum, TileID.Titanium };
            BitsByte hasNewAndShinyOres = new();
            for (int i = 0; i < Main.tile.Width; i++)
            {
                for (int j = 0; j < Main.tile.Height; j++)
                {
                    for (int x = 0; x < newAndShinyOres.Length; x++)
                    {
                        if (!hasNewAndShinyOres[x] && Main.tile[i, j].TileType == newAndShinyOres[x])
                        {
                            hasNewAndShinyOres[x] = true;
                        }
                    }
                }
            }*/

            // IMPORTANT INFORMATION: These first 3 major blocks of code are set after skeletron has been defeated.
            

            // SavedOreTiers is essentially just the TileID of the current ore in the world.
            MiningQuestInformation.AddThis(WorldGen.SavedOreTiers.Copper, 100)
                .AddThis(WorldGen.SavedOreTiers.Iron, 85)
                .AddThis(WorldGen.SavedOreTiers.Silver, 75)
                .AddThis(WorldGen.SavedOreTiers.Gold, 75)
                .AddThis(TileID.Amethyst, 20)
                .AddThis(TileID.Topaz, 15)
                .AddThis(TileID.Sapphire, 13)
                .AddThis(TileID.Emerald, 8)
                .AddThis(TileID.Ruby, 5)
                .AddThis(TileID.Diamond, 3);
            // Pre-hardmode gather quests, mostly dye ingredients, plants, misc crafting materials, etc.
            GatherQuestInformation.AddThis(ItemID.SkyBlueFlower)
                .AddThis(ItemID.OrangeBloodroot)
                .AddThis(ItemID.YellowMarigold, 3)
                .AddThis(ItemID.LimeKelp)
                .AddThis(ItemID.Mushroom, 20)
                .AddThis(ItemID.Bomb, 20)
                .AddThis(ItemID.ScarabBomb, 10)
                .AddThis(ItemID.Daybloom, 10)
                .AddThis(ItemID.Waterleaf, 10)
                .AddThis(ItemID.Shiverthorn, 10)
                .AddThis(ItemID.Deathweed, 10)
                .AddThis(ItemID.Blinkroot, 10)
                .AddThis(ItemID.Moonglow, 10)
                .AddThis(ItemID.Fireblossom, 10)
                .AddThis(ItemID.FallenStar, 15);
            // Pre-hardmode fighting-type quests. Check QuestKillTracker to see how i deal with 'variants'.
            FightingQuestInformation.AddThis(NPCID.Skeleton, 25)
                .AddThis(NPCID.GreekSkeleton, 15)
                .AddThis(NPCID.GraniteGolem, 10)
                .AddThis(NPCID.GraniteFlyer, 10)
                .AddThis(NPCID.Zombie, 20)
                .AddThis(NPCID.DemonEye, 10)
                .AddThis(NPCID.GoblinScout, 3)
                .AddThis(NPCID.Nymph, 1)
                .AddThis(NPCID.Tim, 1)
                .AddThis(NPCID.Gnome, 3);
            if (WorldGen.crimson) // if world is crimson
            {
                MiningQuestInformation.AddThis(TileID.Crimtane, 50);
                GatherQuestInformation.AddThis(ItemID.ViciousMushroom, 10);
                FightingQuestInformation.AddThis(NPCID.BloodCrawler, 8)
                    .AddThis(NPCID.FaceMonster, 8)
                    .AddThis(NPCID.Crimera, 8);
            }
            else
            {
                MiningQuestInformation.AddThis(TileID.Demonite, 50);
                GatherQuestInformation.AddThis(ItemID.VileMushroom, 10);
                FightingQuestInformation.AddThis(NPCID.EaterofSouls, 10)
                    .AddThis(NPCID.DevourerHead, 5);
            }
            if (Main.hardMode) // If world is in hardmode
            {
                // Generic hardmode types:
                FightingQuestInformation.AddThis(NPCID.ChaosElemental, 5)
                    .AddThis(NPCID.EnchantedSword, 5)
                    .AddThis(NPCID.GiantBat, 15)
                    .AddThis(NPCID.ArmoredSkeleton, 15)
                    .AddThis(NPCID.SkeletonArcher, 15)
                    .AddThis(NPCID.BigMimicHallow);


                if (WorldGen.crimson) // if world is crimson
                {
                    FightingQuestInformation.AddThis(NPCID.FloatyGross, 5)
                        .AddThis(NPCID.CrimsonAxe, 5)
                        .AddThis(NPCID.BigMimicCrimson);
                }
                else
                {
                    FightingQuestInformation.AddThis(NPCID.CursedHammer, 10)
                        .AddThis(NPCID.Clinger, 10)
                        .AddThis(NPCID.Corruptor, 10)
                        .AddThis(NPCID.BigMimicCorruption);
                }
                if (WorldGen.SavedOreTiers.Cobalt != -1)
                {
                    MiningQuestInformation.Add(WorldGen.SavedOreTiers.Cobalt, 50);
                }
                if (WorldGen.SavedOreTiers.Mythril != -1)
                {
                    MiningQuestInformation.Add(WorldGen.SavedOreTiers.Mythril, 45);
                }
                if (WorldGen.SavedOreTiers.Adamantite != -1)
                {
                    MiningQuestInformation.Add(WorldGen.SavedOreTiers.Adamantite, 30);
                }
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                MiningQuestInformation.Add(TileID.Chlorophyte, 25);
            }
            if (NPC.downedPlantBoss)
            {
                FightingQuestInformation.AddThis(NPCID.LihzahrdCrawler, 10)
                    .AddThis(NPCID.FlyingSnake, 10);
            }
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
        public override void OnWorldLoad()
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
                    quest_type = Main.rand.Next(MiningQuestInformation.Keys.ToList());
                    modPlayer.CurrentQuestInformation[1] = quest_type;
                    modPlayer.CurrentQuestInformation[2] = MiningQuestInformation[quest_type];
                    break;

                // Gather quest pulled
                case 2:
                    modPlayer.CurrentQuestInformation[0] = 2;
                    quest_type = Main.rand.Next(GatherQuestInformation.Keys.ToList());
                    modPlayer.CurrentQuestInformation[1] = quest_type;
                    modPlayer.CurrentQuestInformation[2] = GatherQuestInformation[quest_type];
                    break;

                // Fighting quest pulled
                default:
                    modPlayer.CurrentQuestInformation[0] = 3;
                    quest_type = Main.rand.Next(FightingQuestInformation.Keys.ToList());
                    modPlayer.CurrentQuestInformation[1] = quest_type;
                    modPlayer.CurrentQuestInformation[2] = FightingQuestInformation[quest_type];
                    break;
            }
            modPlayer.CurrentQuestInformation[3] = modPlayer.CurrentQuestInformation[2];
        }
    }

}
