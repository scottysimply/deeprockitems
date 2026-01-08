using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class StunnedEnemy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffImmune[Type]) return;
            // If we hit an npc with realLife, remove buff from here and apply to the parent (for worms)
            if (npc.realLife > -1 && npc.realLife != npc.whoAmI)
            {
                if (Main.npc[npc.realLife].buffImmune[Type]) return;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.npc[npc.realLife].AddBuff(Type, npc.buffTime[buffIndex]);
                    npc.DelBuff(buffIndex);
                }
                Main.npc[npc.realLife].GetGlobalNPC<StunnedEnemyNPC>().IsStunned = true;
                return;
            }
            if (npc.buffTime[buffIndex] > 300)
            {
                npc.GetGlobalNPC<StunnedEnemyNPC>().IsStunned = true;
            }
        }
        public override bool ReApply(NPC npc, int time, int buffIndex) {
            return true;
        }
    }
    public class StunnedEnemyNPC : GlobalNPC
    {
        public bool IsStunned { get; set; } = false;
        private bool _oldStun = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC entity)
        {
            entity.buffImmune[ModContent.BuffType<StunnedEnemy>()] = entity.boss;
        }
        public override void ResetEffects(NPC npc)
        {
            IsStunned = false;
        }
        public override bool PreAI(NPC npc)
        {
            // If NPC was just stunned:
            if (IsStunned && !_oldStun)
            {
                npc.velocity = new(0, 0);
                // Share stun to the head of the npc.

            }
            _oldStun = IsStunned;

            // If npc is still stunned
            if (IsStunned)
            {
                npc.velocity.X = 0;
                if (!npc.noGravity)
                {
                    npc.velocity.Y += npc.gravity;
                }
                else
                {
                    npc.velocity.Y = 0;
                }
                return false;
            }
            return base.PreAI(npc);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (IsStunned) return false;
            return base.CanHitNPC(npc, target);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (IsStunned) return false;
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
    }
}
