using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;

namespace deeprockitems.Common.NPCs
{
    public class EmbeddedDetsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        int _strongDetonatorCount = 0;
        int _weakDetonatorCount = 0;
        public void IncrementDetonators() {
            _strongDetonatorCount++;
        }
        /// <summary>
        /// Tries to detonate the detonators on a given NPC. Returns true if it was successful
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        public bool TryDetonateOnNPC(NPC npc, Player player)
        {
            if (_strongDetonatorCount <= 0 && _weakDetonatorCount <= 0) return false;
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, position: npc.Center);
            NPC.HitModifiers modifiers = new();
            int calcDamage = 1;
            if (npc.defense <= 1000)
            {
                if (_strongDetonatorCount > 0)
                {
                    calcDamage += (int)Math.Floor(150 + 400 * Math.Log10(_strongDetonatorCount));
                }
                if (_weakDetonatorCount > 0)
                {
                    calcDamage += (int)Math.Floor(25 + 100 * Math.Log10(_weakDetonatorCount));
                }
            }
            NPC.HitInfo hit = modifiers.ToHitInfo(calcDamage, false, 1f);
            player.StrikeNPCDirect(npc, hit);
            _strongDetonatorCount = 0;
            _weakDetonatorCount = 0;
            return true;
        }
        /// <summary>
        /// Converts strong detonators into weak detonators
        /// </summary>
        /// <returns></returns>
        public int ConvertDetonatorsToWeaker() {
            int oldDetonatorCount = _strongDetonatorCount;
            _strongDetonatorCount = 0;
            _weakDetonatorCount += oldDetonatorCount;
            
            return oldDetonatorCount;
        }
        public bool CanDetonatorsActivate() {
            return _strongDetonatorCount > 0 || _weakDetonatorCount > 0;
        }
    }
}
