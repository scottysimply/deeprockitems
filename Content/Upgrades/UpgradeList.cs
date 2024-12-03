using System.Collections;
using System.Collections.Generic;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeList : IEnumerable<UpgradeTier>
    {
        public UpgradeList(string parentName, params UpgradeTier[] upgradeTiers)
        {
            _parentName = parentName;
            _innerArray = new UpgradeTier[5];
            for (int i = 0; i < upgradeTiers.Length)
            // Set localization keys
            foreach (var tier in upgradeTiers)
            {
                foreach (var upgrade in tier)
                {
                    upgrade.LocalizedKey = $"Mods.deeprockitems.Upgrades.{parentName}.{upgrade.InternalName}";
                    _ = upgrade.DisplayName;
                    _ = upgrade.HoverText;
                }
            }
        }
        public UpgradeTier this[int i]
        {
            get => _innerArray[i];
            set => _innerArray[i] = value;
        }
        private string _parentName;
        private UpgradeTier[] _innerArray;
        public IEnumerator<UpgradeTier> GetEnumerator()
        {
            return new UpgradeListEnumerator(_innerArray);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UpgradeListEnumerator(_innerArray);
        }
    }
    public class UpgradeListEnumerator : IEnumerator<UpgradeTier>
    {
        private UpgradeTier[] _tiers;
        int _index;
        public UpgradeListEnumerator(UpgradeTier[] tiers)
        {
            _tiers = tiers;
            _index = -1;
        }
        public UpgradeTier Current => _tiers[_index];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            _index++;
            if (_index >= _tiers.Length)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
