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
        int _detonatorCount = 0;
        public void IncrementDetonators() {
            _detonatorCount++;
        }
        /// <summary>
        /// Tries to detonate the detonators on a given NPC. Returns true if it was successful
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        public bool TryDetonateOnNPC(NPC npc, Player player)
        {
            if (_detonatorCount <= 0) return false;
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, position: npc.Center);
            player.StrikeNPCDirect(npc, new NPC.HitInfo() { Damage = (int)Math.Floor(150 + 400*Math.Log10(_detonatorCount)), Knockback = 1f });
            _detonatorCount = 0;
            return true;
        }
    }
}
