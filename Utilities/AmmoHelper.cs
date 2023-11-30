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
            // Retrieve the item that coorespondes to the ItemID of the ammo type.
            Item AmmoUsed = ContentSamples.ItemsByType[itemID];
            return AmmoUsed.shoot; // Return the AmmoID.
        }
    }
}
