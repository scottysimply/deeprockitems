using deeprockitems.Common.EntitySources;
using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace deeprockitems.Content.Projectiles.Globals
{
    public class UpgradeGlobalProjectile : GlobalProjectile
    {
        Upgrade[] _equippedUpgrades = [];
        bool _cameFromUpgradableWeapon = false;
        public override bool InstancePerEntity => true;
        /// <summary>
        /// Finds if an upgrade is associated with this projectile by searching for its' internal name. If two upgrades share the same internal name, either upgrade has to be equipped.
        /// </summary>
        /// <param name="name">The internal name of the upgrade</param>
        /// <returns></returns>
        public bool IsUpgradeEquipped(string name)
        {
            return _equippedUpgrades.Any(upgrade => upgrade.InternalName == name);
        }
        public override void SetDefaults(Projectile entity)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_SetDefaultsHook?.Invoke(entity);
            }
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // If the projectile was spawned via another projectile, transfer upgrades from parent to child
            if (source is EntitySource_Parent { Entity: Projectile newProj })
            {
                var global = newProj.GetGlobalProjectile<UpgradeGlobalProjectile>();
                _equippedUpgrades = global._equippedUpgrades;
                _cameFromUpgradableWeapon = global._cameFromUpgradableWeapon;
                return;
            }

            if (source is not EntitySource_FromUpgradableWeapon newSource) return;

            _cameFromUpgradableWeapon = true;

            // Get list of equipped upgrades
            _equippedUpgrades = newSource.Item.GetEquippedUpgrades();

            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_OnSpawnHook?.Invoke(projectile, source);
            }
        }
        public override void AI(Projectile projectile)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_AIHook?.Invoke(projectile);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var upgrade in _equippedUpgrades)
            {
                upgrade.Behavior.Projectile_OnHitNPCHook?.Invoke(projectile, target, hit, damageDone);
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            // If the projectile has any equipped upgrades, disable any damage variation
            if (_cameFromUpgradableWeapon)
            {
                modifiers.DamageVariationScale *= 0f;
                modifiers.DisableCrit();
            }

            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_ModifyHitNPCHook == null) continue;

                upgrade.Behavior.Projectile_ModifyHitNPCHook.Invoke(projectile, target, ref modifiers);
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            bool callBase = true;
            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_PreDrawHook == null) continue;

                // If the hook was found, predraw and cache the return value. 
                bool result = upgrade.Behavior.Projectile_PreDrawHook.Invoke(projectile, lightColor);
                if (!result)
                {
                    callBase = false;
                }
            }
            if (!callBase) return false;
            return base.PreDraw(projectile, ref lightColor);
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            bool callBase = true;
            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_PreKillHook == null) continue;

                // If the hook was found, prekill and return base 
                bool result = upgrade.Behavior.Projectile_PreKillHook.Invoke(projectile, timeLeft);
                if (!result)
                {
                    callBase = false;
                }
            }
            if (!callBase) return false;
            return base.PreKill(projectile, timeLeft);
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            bool callBase = true;
            foreach (var upgrade in _equippedUpgrades)
            {
                if (upgrade.Behavior.Projectile_OnTileCollideHook == null) continue;

                // If the hook was found, collide and return base 
                bool result = upgrade.Behavior.Projectile_OnTileCollideHook.Invoke(projectile, oldVelocity);
                if (!result)
                {
                    callBase = false;
                }
            }
            if (!callBase) return false;
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(projectile, bitWriter, binaryWriter);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            base.ReceiveExtraAI(projectile, bitReader, binaryReader);
        }
    }
    public static class UpgradeProjectileExtension {
        public static bool IsUpgradeEquipped(this Projectile projectile, string upgradeName) {
            return projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped(upgradeName);
        }
    }
}
