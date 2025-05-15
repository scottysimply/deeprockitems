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
            foreach (var upgrade in _innerArray)
            {
                upgrade.Tier = this;
            }
        }
        public Upgrade this[int i]
        {
            get => _innerArray[i];
        }
        public int Length => _innerArray.Length;
        public int SelectedIndex { get; private set; }
        /// <summary>
        /// Obtains the selected upgrade, or null if no upgrade is selected.
        /// </summary>
        public Upgrade? SelectedUpgrade { get => SelectedIndex == -1 ? null : _innerArray[SelectedIndex]; }
        public void SelectUpgrade(int index)
        {
            if (index >= _innerArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The provided index is greater than the length of the tier.");
            }
            else if (index < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"The provided index is less than -1. To specify no selection, {nameof(index)} should be -1.");
            }
            SelectedIndex = index;
            for (int i = 0; i < _innerArray.Length; i++)
            {
                if (i == index)
                {
                    _innerArray[index].UpgradeState.IsEquipped = !_innerArray[index].UpgradeState.IsEquipped;
                    continue;
                }
                _innerArray[i].UpgradeState.IsEquipped = false;
            }
        }
        public void SelectUpgrade(string name) {
            foreach (var upgrade in _innerArray)
            {
                if (upgrade.InternalName == name)
                {
                    upgrade.UpgradeState.IsEquipped = !upgrade.UpgradeState.IsEquipped;
                    continue;
                }
                upgrade.UpgradeState.IsEquipped = false;
            }
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
