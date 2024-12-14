using System.Collections;
using System.Collections.Generic;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeList : Dictionary<int, UpgradeTier>
    {
        /* I want to change the way the upgrade list behaves internally. It will be a collection of upgrade tiers, indexed by tier number
         * 
         */
        public UpgradeList(string parentName) {
            _parentName = parentName;
        }
        private string _parentName;
    }
}
