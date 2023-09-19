using Terraria.GameContent.Personalities;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Common.Interfaces
{
    public class SpaceBiome : IShoppingBiome, ILoadable
    {
        public string NameKey { get; set; }
        SpaceBiome()
        {
            NameKey = "Space";
        }

        public bool IsInBiome(Player player)
        {
            return player.ZoneSkyHeight;
        }

        void ILoadable.Load(Mod mod)
        {
        }

        void ILoadable.Unload()
        {
        }
    }
}
