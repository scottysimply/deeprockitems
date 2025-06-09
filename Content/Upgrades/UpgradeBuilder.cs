using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeBuilder
    {
        private UpgradeBuilder() {

        }
        public const int OVERCLOCK_TIER = 127;
        int _currentTierAddingTo = 0;
        Upgrade _currentUpgradeAddingTo;
        string _internalName;
        Dictionary<int, List<Upgrade>> _innerUpgrades;
        /// <summary>
        /// Creates a builder to construct a new upgrade list. 
        /// </summary>
        /// <param name="name">The internal name of the upgrade list, which will be used for localization</param>
        /// <returns></returns>
        public static UpgradeBuilder CreateUpgradeList(string name) {
            return new() { _innerUpgrades = [], _internalName = name};
        }
        /// <summary>
        /// Defines a new upgrade tier for this upgrade list with a specified tier.
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        public UpgradeBuilder WithTier(int tier) {
            if (_currentUpgradeAddingTo != null)
            {
                _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
                _currentUpgradeAddingTo = null;
            }
            _currentTierAddingTo = tier;
            _innerUpgrades.Add(_currentTierAddingTo, []);
            return this;
        }
        /// <summary>
        /// Defines a new upgrade tier for this upgrade list without a specified tier.
        /// </summary>
        /// <returns></returns>
        public UpgradeBuilder WithTier() {
            if (_currentUpgradeAddingTo != null)
            {
                _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
                _currentUpgradeAddingTo = null;
            }
            _currentTierAddingTo++;
            _innerUpgrades.Add(_currentTierAddingTo, []);
            return this;
        }
        /// <summary>
        /// Defines a new overclock for this weapon.
        /// </summary>
        /// <returns></returns>
        public UpgradeBuilder WithOverclock(string name, Asset<Texture2D> texture, Overclock.OverclockType type) {
            if (_currentTierAddingTo != OVERCLOCK_TIER)
            {
                WithTier(OVERCLOCK_TIER);
            }
            if (_currentUpgradeAddingTo != null)
            {
                _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
            }
            _currentUpgradeAddingTo = new Overclock(_internalName, name, texture, type) { LocalizedKey = $"Mods.deeprockitems.Upgrades.{_internalName}.{name}" };
            return this;
        }
        /// <summary>
        /// Defines a new upgrade for the currently selected upgrade tier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public UpgradeBuilder WithUpgrade(string name, Asset<Texture2D> texture) {
            if (_currentUpgradeAddingTo != null)
            {
                _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
            }
            _currentUpgradeAddingTo = new Upgrade(_internalName, name, texture) { LocalizedKey = $"Mods.deeprockitems.Upgrades.{_internalName}.{name}" };
            return this;
        }
        /// <summary>
        /// Adds behavior to the currently selected upgrade.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public UpgradeBuilder WithBehavior<T>(T action) where T : Delegate {
            if (_currentUpgradeAddingTo is null) throw new NotSupportedException($"{nameof(WithUpgrade)} must be invoked before invoking {nameof(WithBehavior)}");
            var query = _currentUpgradeAddingTo.Behavior.GetType().GetProperties().Where(info => info.PropertyType == typeof(T));

            if (!query.Any()) throw new ArgumentException($"{nameof(UpgradeBehavior)} has no delegate with type {nameof(T)}");
            var property = _currentUpgradeAddingTo.Behavior.GetType().GetProperty(query.First().Name, BindingFlags.Public | BindingFlags.Instance);
            if (property.GetMethod.Invoke(_currentUpgradeAddingTo.Behavior, null) != null) throw new InvalidOperationException($"{_internalName + "." + _currentUpgradeAddingTo.UpgradeName} already has behavior defined for type {nameof(T)}.");

            _currentUpgradeAddingTo.Behavior.GetType().GetProperty(query.First().Name, BindingFlags.Public | BindingFlags.Instance).SetMethod.Invoke(_currentUpgradeAddingTo.Behavior, [action]);
            return this;
        }
        /// <summary>
        /// Adds an ingredient (or candidacy of ingredients) to the recipe of the current upgrade.
        /// </summary>
        /// <param name="candidateItemIDs"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public UpgradeBuilder WithIngredient(int[] candidateItemIDs, int stack = 1) {
            if (_currentUpgradeAddingTo is null) throw new NotSupportedException($"{nameof(WithUpgrade)} must be invoked before adding a recipe.");
            _currentUpgradeAddingTo.Recipe ??= new();

            _currentUpgradeAddingTo.Recipe.AddCandidateIngredient(candidateItemIDs, stack);
            return this;
        }
        public UpgradeBuilder WithIngredient(int itemID, int stack = 1) {
            if (_currentUpgradeAddingTo is null) throw new NotSupportedException($"{nameof(WithUpgrade)} must be invoked before adding a recipe.");
            _currentUpgradeAddingTo.Recipe ??= new();

            _currentUpgradeAddingTo.Recipe.AddIngredient(itemID, stack);
            return this;
        }
        /// <summary>
        /// Finishes defining this upgrade list and removes references to this builder.
        /// </summary>
        /// <returns></returns>
        public UpgradeList Seal() {
            if (_currentUpgradeAddingTo != null)
            {
                _innerUpgrades[_currentTierAddingTo].Add(_currentUpgradeAddingTo);
                _currentUpgradeAddingTo = null;
            }
            UpgradeList upgrades = new(_internalName);
            foreach (var kvp in _innerUpgrades)
            {
                foreach (var upgrade in kvp.Value)
                {
                    string tierText = upgrade is Overclock ? "Overclocks" : $"Tier{kvp.Key}";
                    upgrade.LocalizedKey = $"Mods.deeprockitems.Upgrades.{_internalName}.{tierText}.{upgrade.UpgradeName}";
                    _ = upgrade.DisplayName;
                    _ = upgrade.HoverText;
                }
                UpgradeTier upgradeTier = new(kvp.Key, [..kvp.Value]);
                upgrades.Add(upgradeTier.Tier, upgradeTier);
            }
            return upgrades;
        }
    }
}
