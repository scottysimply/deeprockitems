using deeprockitems.Content.Projectiles.MissionControlAttack;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class TargettedByMC : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        private int timer = 0;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] == 180)
            {
                float rand = Main.rand.NextFloat(-5f, 5f);
                Vector2 spawnpos = new Vector2(npc.Center.X + rand, npc.Center.Y - Main.screenPosition.Y);
                Projectile.NewProjectile(npc.GetSource_FromAI(), spawnpos, new Vector2(0, 10), ModContent.ProjectileType<ResupplyPodDrills>(), 500, .1f, ai0: npc.position.Y);
                npc.DelBuff(buffIndex);
            }
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            // Don't extend the timer.
            return true;
        }
    }
}
