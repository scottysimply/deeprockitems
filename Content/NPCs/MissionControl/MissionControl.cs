using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using deeprockitems.Common.Quests;
using Terraria.Map;
using System;
using System.Linq;

namespace deeprockitems.Content.NPCs.MissionControl
{
    public class MissionControl : ModNPC
    {
        readonly string location = "Mods.deeprockitems.Dialogue.MissionControl.";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 14;

            NPCID.Sets.ExtraFramesCount[Type] = 0;
            NPCID.Sets.AttackFrameCount[Type] = 0;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
        }
        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 42;
            NPC.townNPC = NPC.friendly = true;
            NPC.aiStyle = 7;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            

            AnimationType = NPCID.Nymph;

            NPC.knockBackResist = 0.5f;
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            if (numTownNPCs > 8 && (NPC.downedBoss3 || Main.hardMode))
            {
                return true;
            }

            return false;
        }
        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            // Always available
            dialogue.Add(Language.GetTextValue(location + "StandardDialogue1", Main.LocalPlayer.name));
            dialogue.Add(Language.GetTextValue(location + "StandardDialogue2"));
            dialogue.Add(Language.GetTextValue(location + "StandardDialogue3"));

            // Only available if quest is ongoing, with a 75% chance of pulling one of these 3
            if (QuestsBase.CurrentQuest[0] < 99 && QuestsBase.CurrentQuest[0] > 0)
            {
                dialogue.Add(Language.GetTextValue(location + "QuestOngoing1"), 3);
                dialogue.Add(Language.GetTextValue(location + "QuestOngoing2"), 3);
                dialogue.Add(Language.GetTextValue(location + "QuestOngoing3"), 3);
            }

            return dialogue;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("LegacyInterface.64");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            // Clicked on "shop"
            if (firstButton)
            {
                shop = "Shop";
            }
            else // This is where the magic (quests) happen.
            {
                // If rewards were not claimed and a quest is not ongoing, odds are rewards are owed.
                DRGQuestsModPlayer modPlayer = Main.LocalPlayer.GetModPlayer<DRGQuestsModPlayer>();
                if (!modPlayer.PlayerHasClaimedRewards && QuestsBase.CurrentQuest[0] == -1)
                {
                    QuestsRewards.IssueRewards(modPlayer);
                    modPlayer.PlayerHasClaimedRewards = true;
                    QuestsBase.Progress = 0;
                    int chat = Main.rand.Next(3);
                    Main.npcChatText = chat switch
                    {
                        0 => Language.GetTextValue(location + "QuestCompleted1"),
                        1 => Language.GetTextValue(location + "QuestCompleted2"),
                        _ => Language.GetTextValue(location + "QuestCompleted3")
                    };
                }
                else if (QuestsBase.CurrentQuest[0] == -1)
                {
                    int chat = Main.rand.Next(3);
                    Main.npcChatText = chat switch
                    {
                        0 => Language.GetTextValue(location + "QuestInactive1"),
                        1 => Language.GetTextValue(location + "QuestInactive2"),
                        _ => Language.GetTextValue(location + "QuestInactive3")
                    };
                }
                else
                {
                    if (QuestsBase.CurrentQuest[0] == 0)
                    {
                        QuestsBase.Talk_CreateQuest();
                    }
                    bool chat = !Main.rand.NextBool(2);
                    // This is messy, but this just plays the appropriate dialogue based on which quest was chosen.
                    Main.npcChatText = QuestsBase.CurrentQuest[0] switch
                    {
                        // LOOOONG lines. this is just further randomizing between two options to add flavor.
                        1 => chat ? Language.GetTextValue(location + "QuestStartMining1", Lang.GetMapObjectName(MapHelper.tileLookup[QuestsBase.CurrentQuest[1]]), QuestsBase.Progress).Pluralizer() : Language.GetTextValue(location + "QuestStartMining2", Lang.GetMapObjectName(MapHelper.tileLookup[QuestsBase.CurrentQuest[1]]), QuestsBase.Progress).Pluralizer(),
                        2 => chat ? Language.GetTextValue(location + "QuestStartGather1", Lang.GetItemName(QuestsBase.CurrentQuest[1]), QuestsBase.Progress).Pluralizer() : Language.GetTextValue(location + "QuestStartGather2", Lang.GetItemName(QuestsBase.CurrentQuest[1]), QuestsBase.Progress).Pluralizer(),
                        _ => chat ? Language.GetTextValue(location + "QuestStartSlay1", Lang.GetNPCName(QuestsBase.CurrentQuest[1]), QuestsBase.Progress).Pluralizer() : Language.GetTextValue(location + "QuestStartSlay2", Lang.GetNPCName(QuestsBase.CurrentQuest[1]), QuestsBase.Progress).Pluralizer()
                    };
                }

            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type)
            .Add<Items.Misc.UpgradeToken>()
            .Add<Items.Misc.MatrixCore>();
            ;
            npcShop.Register();
        }
    }
    public static class Extensions
    {
        public static string Pluralizer(this string str)
        {
            if (str.ToLower().Contains(" ore"))
            {
                return str;
            }
            string[] words = str.Split(" ");
            string sentence = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int n))
                {
                    if (n == 1)
                    {
                        return str;
                    }
                    else
                    {
                        i += 1;
                        if (words[i].Contains(",") || words[i].Contains("."))
                        {
                            words[i] = words[i][..^1] + "s" + words[i][^1];
                        }
                    }
                }
            }
            for (int x = 0; x < words.Length; x++)
            {
                sentence += words[x] + " ";
            }
            return sentence.TrimEnd();
        }
    }
}