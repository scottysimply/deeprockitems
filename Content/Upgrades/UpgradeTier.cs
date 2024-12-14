using System;
using System.Collections.Generic;
using System.Collections;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeTier : IEnumerable<Upgrade>
    {
        public UpgradeTier(int tier, params Upgrade[] data)
        {
            _innerArray = data;
            Tier = tier;
        }
        public Upgrade this[int i]
        {
            get => _innerArray[i];
        }
        public int Length => _innerArray.Length;
        public int SelectedUpgradeIndex { get; private set; }
        public void SelectUpgrade(int index)
        {
            SelectedUpgradeIndex = index;
        }
        private Upgrade[] _innerArray;
        public int Tier { get; set; }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UpgradeEnumerator(_innerArray);
        }

        IEnumerator<Upgrade> IEnumerable<Upgrade>.GetEnumerator()
        {
            return new UpgradeEnumerator(_innerArray);
        }
    }
    public class UpgradeEnumerator : IEnumerator<Upgrade>
    {
        public UpgradeEnumerator(Upgrade[] upgrades)
        {
            _upgrades = upgrades;
        }
        private Upgrade[] _upgrades;
        private int index = -1;
        public Upgrade Current => _upgrades[index];

        object IEnumerator.Current => Current;
        public bool MoveNext()
        {
            index++;
            return index < _upgrades.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
