using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.IO;

namespace deeprockitems.Common.Quests
{
    public class QuestSystem : ModSystem
    {
        public QuestCollection Quests { get; set; }
        public QuestData CurrentQuest { get; set; }
        public override void NetSend(BinaryWriter writer)
        {
            // Write current quest
            writer.Write((int)CurrentQuest.QuestType); // Quest ID
            writer.Write(CurrentQuest.TypeRequired); // Quest type
            writer.Write(CurrentQuest.AmountRequired); // Amount required
            writer.Write(CurrentQuest.Predicate); // Predicate required to pull the quest
        }
        public override void NetReceive(BinaryReader reader)
        {
            // Net is FIFO. This is in the exact same order as above
            CurrentQuest = new((QuestID)reader.ReadInt32(),
                               reader.ReadInt32(),
                               reader.ReadInt32(),
                               reader.ReadBoolean());
        }
        public override void OnWorldLoad()
        {
            RecalculateQuests();
        }
        public void RecalculateQuests()
        {
            Quests = [new QuestData(QuestID.Gathering, ItemID.Daybloom, 10, true)];
            RecalculateMiningQuests();
            RecalculateGatheringQuests();
            RecalculateFightingQuests();
        }
        private void RecalculateFightingQuests()
        {
            Quests.Add(QuestID.Fighting, NPCID.BlueSlime, 10, true)
                .Add(QuestID.Fighting, NPCID.DemonEye, 10, !Main.hardMode)
                .Add(QuestID.Fighting, NPCID.WanderingEye, 10, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.Zombie, 10, true)
                .Add(QuestID.Fighting, NPCID.Skeleton, 15, true)
                .Add(QuestID.Fighting, NPCID.GreekSkeleton, 10, true)
                .Add(QuestID.Fighting, NPCID.GraniteFlyer, 10, true)
                .Add(QuestID.Fighting, NPCID.GraniteGolem, 10, true)
                .Add(QuestID.Fighting, NPCID.Medusa, 1, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.Demon, 10, NPC.downedBoss3)
                .Add(QuestID.Fighting, NPCID.FireImp, 10, NPC.downedBoss3)
                .Add(QuestID.Fighting, NPCID.RedDevil, 5, NPC.downedMechBossAny)
                .Add(QuestID.Fighting, NPCID.CaveBat, 10, !Main.hardMode)
                .Add(QuestID.Fighting, NPCID.GiantBat, 10, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.ArmoredSkeleton, 15, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.SkeletonArcher, 15, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.ChaosElemental, 3, Main.hardMode)
                .Add(QuestID.Fighting, NPCID.AngryBones, 10, NPC.downedBoss3)
                ;
        }
        private void RecalculateGatheringQuests()
        {
            Quests.Add(QuestID.Gathering, ItemID.Moonglow, 10, true)
                .Add(QuestID.Gathering, ItemID.Waterleaf, 10, true)
                .Add(QuestID.Gathering, ItemID.Fireblossom, 10, true)
                .Add(QuestID.Gathering, ItemID.Deathweed, 10, true)
                .Add(QuestID.Gathering, ItemID.Blinkroot, 10, true)
                .Add(QuestID.Gathering, ItemID.Shiverthorn, 10, true)
                .Add(QuestID.Gathering, ItemID.YellowMarigold, 1, true)
                .Add(QuestID.Gathering, ItemID.OrangeBloodroot, 1, true)
                .Add(QuestID.Gathering, ItemID.TealMushroom, 1, true)
                .Add(QuestID.Gathering, ItemID.GreenMushroom, 1, true)
                .Add(QuestID.Gathering, ItemID.LimeKelp, 1, true)
                .Add(QuestID.Gathering, ItemID.SkyBlueFlower, 1, true)
                .Add(QuestID.Gathering, ItemID.BlueBerries, 1, true)
                .Add(QuestID.Gathering, ItemID.PinkPricklyPear, 1, true)
                .Add(QuestID.Gathering, ItemID.VileMushroom, 10, !WorldGen.crimson)
                .Add(QuestID.Gathering, ItemID.ViciousMushroom, 10, WorldGen.crimson)
                .Add(QuestID.Gathering, ItemID.GlowingMushroom, 25, true)
                .Add(QuestID.Gathering, ItemID.Mushroom, 20, true)
                ;
        }
        private void RecalculateMiningQuests()
        {
            // Ensures that mining quests can actually work (since they rely on ore tiers)
            if (WorldGen.SavedOreTiers.Copper != -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Prehardmode tiered ores
                Quests.Add(QuestID.Mining, ItemID.CopperOre, 75, WorldGen.SavedOreTiers.Copper == TileID.Copper)
                    .Add(QuestID.Mining, ItemID.TinOre, 75, WorldGen.SavedOreTiers.Copper == TileID.Tin)
                    .Add(QuestID.Mining, ItemID.IronOre, 60, WorldGen.SavedOreTiers.Iron == TileID.Iron)
                    .Add(QuestID.Mining, ItemID.LeadOre, 60, WorldGen.SavedOreTiers.Iron == TileID.Lead)
                    .Add(QuestID.Mining, ItemID.SilverOre, 50, WorldGen.SavedOreTiers.Silver == TileID.Silver)
                    .Add(QuestID.Mining, ItemID.TungstenOre, 50, WorldGen.SavedOreTiers.Silver == TileID.Tungsten)
                    .Add(QuestID.Mining, ItemID.GoldOre, 30, WorldGen.SavedOreTiers.Gold == TileID.Gold)
                    .Add(QuestID.Mining, ItemID.PlatinumOre, 30, WorldGen.SavedOreTiers.Gold == TileID.Platinum)
                    // Evil ores
                    .Add(QuestID.Mining, ItemID.DemoniteOre, 40, NPC.downedBoss2 && !WorldGen.crimson)
                    .Add(QuestID.Mining, ItemID.CrimtaneOre, 40, NPC.downedBoss2 && WorldGen.crimson)
                    // Gems and hellstone
                    .Add(QuestID.Mining, ItemID.Hellstone, 25, NPC.downedBoss2)
                    .Add(QuestID.Mining, ItemID.Amethyst, 10, true)
                    .Add(QuestID.Mining, ItemID.Topaz, 10, true)
                    .Add(QuestID.Mining, ItemID.Sapphire, 7, true)
                    .Add(QuestID.Mining, ItemID.Emerald, 7, true)
                    .Add(QuestID.Mining, ItemID.Ruby, 5, true)
                    .Add(QuestID.Mining, ItemID.Diamond, 5, true)
                    .Add(QuestID.Mining, ItemID.Amber, 5, true)
                    // Hardmode tiered ores
                    .Add(QuestID.Mining, ItemID.CobaltOre, 50, WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                    .Add(QuestID.Mining, ItemID.MythrilOre, 35, WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                    .Add(QuestID.Mining, ItemID.AdamantiteOre, 25, WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                    .Add(QuestID.Mining, ItemID.PalladiumOre, 50, WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                    .Add(QuestID.Mining, ItemID.OrichalcumOre, 35, WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                    .Add(QuestID.Mining, ItemID.TitaniumOre, 25, WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                    // Chlorophyte
                    .Add(QuestID.Mining, ItemID.ChlorophyteOre, 40, NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            // EMERGENCY ONLY: Generate quest if all is null:
            if (CurrentQuest is null)
            {
                RecalculateQuests();
                GenerateQuest();
            }
            // Save the world's current quest data
            tag["questType"] = (int)CurrentQuest.QuestType;
            tag["questFind"] = CurrentQuest.TypeRequired;
            tag["questAmount"] = CurrentQuest.AmountRequired;
            tag["questHardmode"] = CurrentQuest.Predicate;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            // load the current quest data
            if (tag.ContainsKey("questType") && tag.ContainsKey("questFind") && tag.ContainsKey("questAmount") && tag.ContainsKey("questHardmode"))
            {
                CurrentQuest = new((QuestID)tag.GetAsInt("questType"), tag.GetAsInt("questFind"), tag.GetAsInt("questAmount"), tag.GetBool("questHardmode"));
            }
            else
            {
                // If failed to load (or is new world), generate a new quest to the world
                GenerateQuest();
            }
        }
        public override void PostUpdateTime()
        {
            // If it's early morning and this is on the host, OR the CurrentQuest is null
            if ((CurrentQuest is null) || (Main.time == 1d && Main.dayTime && Main.netMode != NetmodeID.MultiplayerClient))
            {
                // Review all possible quests:
                RecalculateQuests();
                // Create quest
                GenerateQuest();
            }

        }
        public void GenerateQuest()
        {
            // Reset quests for all
            foreach (Player player in Main.player)
            {
                if (!player.TryGetModPlayer(out QuestModPlayer modPlayer))
                {
                    continue;
                }
                modPlayer.ActiveQuest = null;
            }
            // If the quest list is null, cancel. This gives a default quest.
            if (Quests is null) return;

            // Generate quest
            int questType = Main.rand.Next(1, 4);
            QuestCollection questsToChooseFrom = Quests.Where(q => ((int)q.QuestType == questType) && q.Predicate);
            CurrentQuest = questsToChooseFrom.TakeRandom();
        }
    }
}
