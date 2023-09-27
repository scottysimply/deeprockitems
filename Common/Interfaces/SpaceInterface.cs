using Terraria.GameContent.Personalities;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Common.Interfaces
{
    public class SpaceBiome : IShoppingBiome, ILoadable
    {
        public string NameKey { get; set; }
        public SpaceBiome()
        {
            NameKey = "Space";
        }
        

        public bool IsInBiome(Player player)
        {
            return player.position.Y / 16f < Main.worldSurface * .4f;
        }

        void ILoadable.Load(Mod mod)
        {
        }

        void ILoadable.Unload()
        {
        }
    }
}
