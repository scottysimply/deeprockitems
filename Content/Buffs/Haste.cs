using Terraria.ModLoader;
using Terraria;

namespace deeprockitems.Content.Buffs
{
    public class Haste : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<HastePlayer>().HasHaste = true;
            player.moveSpeed += 0.25f;
        }
    }
    public class HastePlayer : ModPlayer
    {
        public bool HasHaste { get; set; } = false;
        public override void ResetEffects()
        {
            HasHaste = false;
        }
        public override void PostUpdateRunSpeeds() {
            
            if (HasHaste) {
                Player.accRunSpeed *= 1.25f;
            }
        }
    }
}
