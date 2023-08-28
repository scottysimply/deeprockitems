using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace deeprockitems.Utilities
{
    public static class AmmoHelper
    {
        public static int AmmoID_GetProjectileID(int itemID)
        {
            Dictionary<int, Item> Items = ContentSamples.ItemsByType;
            Item AmmoUsed = ContentSamples.ItemsByType[itemID];

            return AmmoUsed.shoot;
        }
    }
}
