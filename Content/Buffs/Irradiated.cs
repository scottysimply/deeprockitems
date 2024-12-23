using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class Irradiated : InstancedBuff
    {
        int _reapplyCount = 0;
        public override bool ReapplyNPC(NPC npc) {
            _reapplyCount++;
            return true;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            switch (_reapplyCount)
            {
                case <= 3:
                    npc.lifeRegen -= 24;
                    damage = 6;
                    break;
                case <= 6:
                    npc.lifeRegen -= 48;
                    damage = 12;
                    break;
                default:
                    npc.lifeRegen -= 60;
                    damage = 15;
                    break;
            }
        }
    }
}
