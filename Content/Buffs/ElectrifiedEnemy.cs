using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class ElectrifiedEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ElectrifiedEnemyNPC>().IsElectrified = true;
        }
    }
    public class ElectrifiedEnemyNPC : GlobalNPC
    {
        public bool IsElectrified { get; set; }
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            IsElectrified = false;
        }
        private int buffDamageTimer = 0;
        public override void AI(NPC npc)
        {
            if (!IsElectrified)
            {
                buffDamageTimer = 0;
                return;
            }
            buffDamageTimer++;
            // Determine if buff should tick
            if (buffDamageTimer % 60 == 0)
            {
                // Get adjusted whoAmI
                int adjustedWhoAmI = npc.whoAmI;
                if (npc.realLife >= 0)
                {
                    adjustedWhoAmI = npc.realLife;
                }

                // If npc can be damaged
                if (!Main.npc[adjustedWhoAmI].immortal)
                {
                    // Tick buff
                    Main.npc[adjustedWhoAmI].life -= 30;
                }
                // Spawn dust
                Dust.NewDust(npc.Center, 8, 8, DustID.Electric, SpeedX: Main.rand.NextFloat() - 0.5f, SpeedY: Main.rand.NextFloat() - 0.5f, Scale: 0.8f);

                // Spawn combattext
                CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), CombatText.LifeRegenNegative, 15, dramatic: false, dot: true);

                // Kill NPC if life is 0.
                if (Main.npc[adjustedWhoAmI].life <= 0)
                {
                    Main.npc[adjustedWhoAmI].life = 1;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Main.npc[adjustedWhoAmI].StrikeInstantKill();
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, adjustedWhoAmI, 9999f);
                        }
                    }
                }
            }
        }
    }
}
