using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.NPCs.MissionControl
{
    public class MissionControl : ModNPC
    {
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
        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (numTownNPCs > 14 && (NPC.downedBoss3 || Main.hardMode))
            {
                return true;
            }

            return false;
        }
    }
}