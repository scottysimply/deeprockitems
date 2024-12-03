using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeFactory
    {
        private UpgradeFactory() {

        }
        int _currentTierAddingTo = 0;
        Upgrade _currentUpgradeAddingTo;
        string _internalName;
        Dictionary<int, List<Upgrade>> _innerUpgrades;
        /// <summary>
        /// Creates a factory to construct a new upgrade list. 
        /// </summary>
        /// <param name="name">The internal name of the upgrade list, which will be used for localization</param>
        /// <returns></returns>
        public static UpgradeFactory CreateUpgradeList(string name) {
            return new() { _innerUpgrades = [], _internalName = name};
        }
        public UpgradeFactory WithTier(int tier) {
            _currentTierAddingTo = tier;
            return this;
        }
        public UpgradeFactory WithUpgrade(string name, Texture2D texture) {
            _currentUpgradeAddingTo = new(name, texture);
            return this;
        }
        public UpgradeFactory WithBehavior<T>(T action) where T : Delegate {
            if (_currentUpgradeAddingTo is null) throw new NotSupportedException($"{nameof(WithUpgrade)} must be invoked before invoking {nameof(WithBehavior)}");
            var query = _currentUpgradeAddingTo.Behavior.GetType().GetMembers().Where(info => info.ReflectedType == typeof(T));
            if (!query.Any()) throw new ArgumentException($"{nameof(UpgradeBehavior)} has no delegate with type {nameof(T)}");

            _currentUpgradeAddingTo.Behavior.GetType().GetProperty(query.First().Name, BindingFlags.Public | BindingFlags.Instance).SetMethod.Invoke(_currentUpgradeAddingTo.Behavior, [action]);
            return this;
        }
        public UpgradeFactory SealUpgrade() {
            _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
            _currentUpgradeAddingTo = null;
            return this;
        }
        public Dictionary<int, List<Upgrade>> Seal() {
            var upgrades = _innerUpgrades;
            _innerUpgrades = [];
            return upgrades;
        }
    }
}
