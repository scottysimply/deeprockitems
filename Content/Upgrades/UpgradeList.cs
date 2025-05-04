using System.Collections.Generic;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeList : Dictionary<int, UpgradeTier>
    {
        public UpgradeList(string parentName) {
            _parentName = parentName;
        }
        private string _parentName;
    }
}
