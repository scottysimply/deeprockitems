using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using deeprockitems.Common.Quests;
using Terraria.GameContent.Personalities;
using deeprockitems.Content.Projectiles.MissionControlAttack;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using deeprockitems.Content.Items.Misc;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Upgrades;
using System.Linq;

namespace deeprockitems.Content.NPCs.MissionControl
{
    [AutoloadHead]
    public class MissionControl : ModNPC
    {
        readonly static string location = "Mods.deeprockitems.Dialogue.MissionControl.";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;

            NPCID.Sets.ExtraFramesCount[Type] = 8;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 1600; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 1;
            NPCID.Sets.PrettySafe[Type] = 400;
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            NPC.Happiness
                .SetBiomeAffection(new Common.Interfaces.SpaceBiome(), AffectionLevel.Love) // Loves space!
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like) // Likes the underground
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate); // Hates snow
            for (int id = 0; id < NPCID.Count; id++)
            {
                NPC npc;
                if (!ContentSamples.NpcsByNetId.TryGetValue(id, out npc))
                {
                    continue;
                }
                if (npc.townNPC)
                {
                    AffectionLevel level = AffectionLevel.Dislike;
                    switch (id)
                    {
                        case NPCID.GoblinTinkerer:
                            level = AffectionLevel.Love;
                            break;
                        case NPCID.TaxCollector:
                            level = AffectionLevel.Love;
                            break;
                        case NPCID.Steampunker:
                            level = AffectionLevel.Like;
                            break;
                        case NPCID.Princess:
                            level = AffectionLevel.Like;
                            break;
                        case NPCID.Truffle:
                            level = AffectionLevel.Hate;
                            break;
                        case NPCID.Stylist:
                            level = AffectionLevel.Hate;
                            break;
                        default:
                            break;
                    }
                    NPC.Happiness.SetNPCAffection(id, level);
                }
            }

        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return toKingStatue;
        }
        public override void SetDefaults()
        {
            
            NPC.width = 18;
            NPC.height = 40;
            NPC.townNPC = NPC.friendly = true;
            NPC.aiStyle = 7;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.Dryad;
            DrawOffsetY = 2;


            NPC.knockBackResist = 0.5f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

                new FlavorTextBestiaryInfoElement("Mods.deeprockitems.NPCs.MissionControl.BestiaryText"),
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            if (numTownNPCs > 5 && NPC.downedSlimeKing)
            {
                return true;
            }
            return false;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 1f;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 1;
            knockback = 0f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 480;
            randExtraCooldown = 120;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<ResupplyPodMarker>();
            attackDelay = 1;
        }
        public override string GetChat()
        {
            // Try getting modplayer
            if (!Main.LocalPlayer.TryGetModPlayer(out QuestModPlayer modPlayer)) return "Quest ModPlayer unable to instantiate. Create a bug report on https://scottysimply.github.com/deeprockitems";

            // Create RNG to pull quest
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            // Add general dialogue
            AddChat(dialogue, "StandardDialogue1", 1, Main.LocalPlayer.name);
            AddChat(dialogue, "StandardDialogue2");
            AddChat(dialogue, "StandardDialogue3");

            // If quest is ongoing, add quest-specific dialogue
            if (modPlayer.ActiveQuest is not null && !modPlayer.ActiveQuest.Completed)
            {
                AddChat(dialogue, "QuestOngoing1", 3);
                AddChat(dialogue, "QuestOngoing2", 3);
                AddChat(dialogue, "QuestOngoing3", 3);
            }

            return dialogue;
        }
        private static void AddChat(WeightedRandom<string> dialogue, string key, double weight = 1, params object[] format)
        {
            dialogue.Add(Language.GetTextValue(location + key, format), weight);
        }
        private BlankMatrixCore currentBlankCore;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.64");
            for (int i = 0; i < Main.InventoryItemSlotsCount; i++)
            {
                if (Main.LocalPlayer.inventory[i].ModItem is BlankMatrixCore core && core.InfusedOverclock == null)
                {
                    currentBlankCore = core;
                    button2 = Language.GetTextValue("Mods.deeprockitems.Misc.UsefulWords.Infuse");
                }
            }
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            // If the quest button was clicked
            if (firstButton)
            {
                PerformQuestLogic();
                return;
            }

            PerformCoreInfusion();
        }
        private void PerformQuestLogic() {
            // Get modplayer
            if (!Main.LocalPlayer.TryGetModPlayer(out QuestModPlayer modPlayer)) return;

            // Give player a quest if they don't have one.
            if (modPlayer.ActiveQuest is null)
            {
                // Get system
                QuestSystem system = ModContent.GetInstance<QuestSystem>();
                // Give quest from system
                modPlayer.ActiveQuest = system.CurrentQuest.CreateQuestFromThis();
            }

            // If player is owed quest rewards, give them rewards
            if (modPlayer.ActiveQuest.Completed && !modPlayer.ActiveQuest.HasQuestBeenRewarded)
            {
                // Give rewards
                modPlayer.GiveDeepRockReward();

                // Set chat
                WeightedRandom<string> dialogue = new();
                AddChat(dialogue, "QuestCompleted1");
                AddChat(dialogue, "QuestCompleted2");
                AddChat(dialogue, "QuestCompleted3");
                Main.npcChatText = dialogue;

                // Disable rewards
                modPlayer.ActiveQuest.HasQuestBeenRewarded = true;
            }
            // Else if quest has been completed
            else if (modPlayer.ActiveQuest.Completed)
            {
                // Set chat
                WeightedRandom<string> dialogue = new();
                AddChat(dialogue, "QuestInactive1");
                AddChat(dialogue, "QuestInactive2");
                AddChat(dialogue, "QuestInactive3");
                Main.npcChatText = dialogue;
            }
            // Else, give player quest and display the correct chat message
            else
            {
                // Set dialogue variation
                int chatVariation = Main.rand.Next(1, 3);

                // Get types and amounts
                int type = modPlayer.ActiveQuest.Data.TypeRequired;
                int amount = modPlayer.ActiveQuest.Data.AmountRequired;

                // Change dialogue based on variation and what quest type
                switch (modPlayer.ActiveQuest.Type)
                {
                    case QuestID.Mining:
                        // Find if the map object name has a name--if else, use block name
                        Main.npcChatText = Language.GetTextValue(location + $"QuestStartMining{chatVariation}", Lang.GetItemNameValue(type).Pluralizer(amount), amount);
                        break;
                    case QuestID.Gathering:
                        Main.npcChatText = Language.GetTextValue(location + $"QuestStartGather{chatVariation}", Lang.GetItemNameValue(type).Pluralizer(amount), amount);
                        break;
                    case QuestID.Fighting:
                        Main.npcChatText = Language.GetTextValue(location + $"QuestStartSlay{chatVariation}", Lang.GetNPCNameValue(type).Pluralizer(amount), amount);
                        break;
                }
                Main.npcChatCornerItem = modPlayer.ActiveQuest.ItemIcon;
            }
        }
        private void PerformCoreInfusion() {
            List<int> weapons = new();
            // select indices for potential weapons
            for (int i = 0; i < Main.InventoryItemSlotsCount; i++)
            {
                if (Main.LocalPlayer.inventory[i].ModItem is UpgradableWeapon { UpgradeMasterList: UpgradeList upgrades })
                {
                    bool shouldContinue = false;
                    foreach (var upgrade in upgrades[UpgradeBuilder.OVERCLOCK_TIER])
                    {
                        if (!upgrade.UpgradeState.IsUnlocked)
                        {
                            shouldContinue = true;
                        }
                    }
                    if (!shouldContinue) return;
                    weapons.Add(i);
                }
            }
            if (weapons.Count == 0) return;
            // choose random overclock
            int weaponindex = Main.rand.NextFromList([.. weapons]);
            UpgradableWeapon weapon = Main.LocalPlayer.inventory[weaponindex].ModItem as UpgradableWeapon;
            int overclockIndex = Main.rand.Next(weapon.UpgradeMasterList[UpgradeBuilder.OVERCLOCK_TIER].Length);
            Overclock chosenOverclock = weapon.UpgradeMasterList[UpgradeBuilder.OVERCLOCK_TIER][overclockIndex] as Overclock;
            // spawn new infused matrix core
            currentBlankCore.Item.stack--;
            int type = Mod.GetContent<BlankMatrixCore>().Where(core => core.InfusedOverclock?.UpgradeName == chosenOverclock.UpgradeName).First().Type;
            Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_GiftOrReward(), type);
        }
    }
    public static class Extensions
    {
        private static List<string> ores = new List<string>()
        {
            "copper",
            "iron",
            "silver",
            "gold",
            "tin",
            "lead",
            "tungsten",
            "platinum",
            "demonite",
            "crimtane",
            "cobalt",
            "mythril",
            "adamantite",
            "palladium",
            "orichalcum",
            "titanium",
            "chlorophyte",
        };
        public static string Pluralizer(this string str, int count)
        {
            if (ores.Contains(str.ToLower()))
            {
                return str + " Ore";
            }
            if (count == 1)
            {
                return str;
            }
            return str + "s";
        }
    }
}