using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Upgrades;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradableWeapon : ModItem, IUpgradable
    {
        /// <summary>
        /// Whether the weapon is disabled or not.
        /// </summary>
        public bool IsWeaponEnabledByCooldown { get; private set; } = true;
        /// <summary>
        /// The threshold before the weapon is disabled.
        /// </summary>
        public const float COOLDOWN_THRESHOLD = 100f;
        /// <summary>
        /// How much cooldown is gained per shot. Defaults to 12f.
        /// </summary>
        public float ShotsUntilCooldown { get; set; } = 12f;
        /// <summary>
        /// The initial 
        /// </summary>
        public float OverheatCooldown { get; set; } = 0;
        /// <summary>
        /// The time in ticks it takes for the weapon to cool down.
        /// </summary>
        public float TimeToEndCooldown { get; set; } = 60f;
        /// <summary>
        /// The timer that must finish before the weapon will start cooling down
        /// </summary>
        protected float _passiveCooldownTimer = 0;
        protected float _activeCooldownTimer = 0;
        public override void HoldItem(Player player) {
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier.Value)
                {
                    if (upgrade.UpgradeState.IsEquipped)
                    {
                        upgrade.Behavior.Item_HoldItemHook?.Invoke(Item, player);
                    }
                }
            }
        }
        public override void UpdateInventory(Player player) {
            // Run separate logic if the weapon was overheated or not
            if (IsWeaponEnabledByCooldown)
            {
                // Set active cooldown timer
                _activeCooldownTimer = 0;
                // If cooldown timer is at 0, start passively cooling the weapon
                if (_passiveCooldownTimer <= 0)
                {
                    if (OverheatCooldown > 0)
                    {
                        // Passive cooldown should be the fastest way to cool down the weapon
                        OverheatCooldown -= 1.5f * (COOLDOWN_THRESHOLD / TimeToEndCooldown);
                    }
                    else
                    {
                        OverheatCooldown = 0f;
                    }
                }
                else // Else decrement time since using this weapon
                {
                    _passiveCooldownTimer--;
                }
            }
            else // Else, run slightly different active behavior
            {
                // Set passive cooldown to zero
                _passiveCooldownTimer = 0;
                if (_activeCooldownTimer <= 0)
                {
                    if (OverheatCooldown > 0)
                    {
                        OverheatCooldown -= COOLDOWN_THRESHOLD / TimeToEndCooldown;
                    }
                    else
                    {
                        OverheatCooldown = 0f;
                    }
                }
                else
                {
                    _activeCooldownTimer--;
                }
            }

            if (OverheatCooldown <= 0) // If cooldown is at or below 0, re-enable weapon
            {
                IsWeaponEnabledByCooldown = true;
                OverheatCooldown = 0;
            }

            // Calculate whether cooldown should begin or be added to
            // Make sure the player is holding this weapon
            if (player.HeldItem != Item) return;

            // If player channeling, increment cooldown timer to prevent it from going down
            if (player.channel && _passiveCooldownTimer > 0)
            {
                _passiveCooldownTimer++;
            }

        }
        #region Saving and loading upgrades
        public override void SaveData(TagCompound tag) {
            if (UpgradeMasterList is null) return;
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier.Value)
                {
                    // Of note: instead of serializing everything to do with these upgrades, i'm serializing whether the upgrade is both equipped and unlocked.
                    tag.Add($"{upgrade.InternalName}.{tier.Key}.State", upgrade.UpgradeState);
                }
            }
        }
        public override void LoadData(TagCompound tag) {
            if (UpgradeMasterList is null) return;
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier.Value)
                {
                    if (!tag.ContainsKey($"{upgrade.InternalName}.{tier.Key}.State")) continue;
                    upgrade.UpgradeState = tag.Get<UpgradeStateBinding>($"{upgrade.InternalName}.{tier.Key}.State");
                }
            }
            ApplyStatUpgrades();
        }
        #endregion
/*        public override bool? CanAutoReuseItem(Player player) {
            if (Item.channel && player.autoReuseAllWeapons) return true;
            return null;
        }*/
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            // Remove the crit line, add "upgradable"
            tooltips.Find(tl => tl.FullName == "Terraria/CritChance")?.Hide();
            int desiredIndex = tooltips.FindLastIndex(tooltip => tooltip.Name.StartsWith("Tooltip"));
            if (desiredIndex == -1)
            {
                desiredIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Knockback");
            }
            tooltips.Insert(desiredIndex + 1, new TooltipLine(Mod, "Upgradable", Language.GetTextValue("Mods.deeprockitems.Misc.UsefulWords.Upgradable")));
        }
        public override bool CanUseItem(Player player) {
            return IsWeaponEnabledByCooldown;
        }
        protected int _oldUseTime;
        protected int _oldUseAnimation;
        public UpgradeList UpgradeMasterList { get; set; }
        public Dictionary<string, UpgradeStateBinding> Upgrades = new();
        public virtual void NewSetDefaults() { }
        public sealed override void SetDefaults() {
            ResetStats();
            NewSetDefaults();
            _oldUseTime = Item.useTime;
            _oldUseAnimation = Item.useAnimation;
            base.SetDefaults();
            // Load upgrades and apply parent name to upgrade
            UpgradeMasterList = InitializeUpgrades();
        }
        public virtual void ApplyStatUpgrades() {
            // Reset global stats
            Item.useTime = _oldUseTime;
            Item.useAnimation = _oldUseAnimation;
            ResetStats();
            NewSetDefaults();
            foreach (var tier in UpgradeMasterList)
            {
                foreach (var upgrade in tier.Value)
                {
                    if (upgrade.UpgradeState.IsEquipped)
                    {
                        upgrade.Behavior.Item_ModifyStats?.Invoke(Item);
                    }
                }
            }
        }
        public Upgrade[] GetEquippedUpgrades() {
            List<Upgrade> upgrades = [];
            foreach (var values in UpgradeMasterList.Values)
            {
                foreach (var upgrade in values)
                {
                    if (!upgrade.UpgradeState.IsEquipped) continue;
                    upgrades.Add(upgrade);
                }
            }
            return [..upgrades];
        }
        public override void Load() {
            _ = InitializeUpgrades();
        }
        public virtual void ResetStats() { }
        /// <summary>
        /// Initializes the master list of upgrades this weapon will use.
        /// </summary>
        /// <returns></returns>
        public virtual UpgradeList InitializeUpgrades() => new("notSet");
        public override sealed void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            NewSetStaticDefaults();
        }
        public virtual void NewSetStaticDefaults() { }
        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            // Start modifying the shoot stats of each upgrade
            float spread = 0f;
            // Create duplicates of each parameter to use as ref
            Vector2 position2 = position;
            Vector2 spreadVelocity = velocity;
            int type2 = type;
            int damage2 = damage;
            float knockback2 = knockback;
            NewModifyShootStats(player, ref position2, ref spreadVelocity, ref type2, ref damage2, ref knockback2, ref spread);
            foreach (var upgrade in GetEquippedUpgrades())
            {
                upgrade.Behavior.Item_ModifyShootStatsHook?.Invoke(Item, player, ref position2, ref spreadVelocity, ref type2, ref damage2, ref knockback2, ref spread);
            }
            var newSource = new EntitySource_FromUpgradableWeapon(player, this, source.AmmoItemIdUsed, source.Context);
            if (!Item.channel)
            {
                AddCooldownOnShoot();
            }
            bool upgradeReturn = true;
            foreach (var upgrade in GetEquippedUpgrades())
            {
                upgradeReturn &= upgrade.Behavior.Item_ShootHook?.Invoke(Item, player, newSource, position2, spreadVelocity, type2, damage2, knockback2, spread) ?? true;
            }
            if (upgradeReturn && NewShoot(player, newSource, position2, spreadVelocity, type2, damage2, knockback2, spread))
            {
                spreadVelocity = spreadVelocity.RotatedByRandom(spread);
                Projectile spawnedProj = Projectile.NewProjectileDirect(newSource, position2, spreadVelocity, type2, damage2, knockback2, owner: player.whoAmI);
                if (spawnedProj.ModProjectile is HeldProjectileBase helper)
                {
                    helper.Spread = spread;
                }
            }
            return false;
        }
        public void AddCooldownOnShoot(float multiplier = 1f) {
            OverheatCooldown += multiplier * COOLDOWN_THRESHOLD / ShotsUntilCooldown;
            _passiveCooldownTimer = 30 + Item.useTime;
            // Disable weapon if cooldown gets too high
            if (IsWeaponEnabledByCooldown && OverheatCooldown >= COOLDOWN_THRESHOLD)
            {
                IsWeaponEnabledByCooldown = false;
                _activeCooldownTimer = Item.useTime;
                OverheatCooldown = COOLDOWN_THRESHOLD;
            }
        }

        public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

        }
        public virtual void NewModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread) { }
        public virtual bool NewShoot(Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread) => true;
        public override bool? PrefixChance(int pre, UnifiedRandom rand) {
            return false;
        }
    }
}
