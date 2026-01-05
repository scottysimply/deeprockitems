using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Beers
{
    public class PotsOfGold : BaseBeer
    {
        public override void SetBuff() {
            Item.buffType = ModContent.BuffType<PogBuff>();
        }
        public class PogBuff : BuffForBeer {

        }
    }
}
