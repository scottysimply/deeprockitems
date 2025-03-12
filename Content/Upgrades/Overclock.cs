using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace deeprockitems.Content.Upgrades
{
    public class Overclock
    {
        public Upgrade InternalUpgrade { get; set; }
        public LocalizedText[] Positives { get; set; }
        public LocalizedText[] Negatives { get; set; }
    }
}
