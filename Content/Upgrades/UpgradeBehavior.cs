using deeprockitems.Common.EntitySources;
using System;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Upgrades
{
    #region Delegates for upgrade behavior
    public delegate void ItemStatChange(Item item);
    public delegate void ItemHoldItem(Item item, Player player);
    public delegate void ProjectileSetDefaults(Projectile projectile);
    public delegate void ProjectileOnSpawn(Projectile projectile, IEntitySource source);
    public delegate void ProjectileAI(Projectile projectile);
    public delegate void ProjectileOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone);
    public delegate bool ProjectilePreDraw(Projectile projectile, Color lightColor);
    public delegate void ProjectileModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers);
    public delegate void ItemOnShoot(Item item, Player player, EntitySource_FromUpgradableWeapon source, Projectile projectile);
    public delegate bool ProjectilePreKill(Projectile projectile, int timeLeft);
    public delegate bool ProjectileOnTileCollide(Projectile projectile, Vector2 oldVelocity);
    public delegate void ItemModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread);
    #endregion
    public class UpgradeBehavior
    {
        #region Handlers for upgrade behavior
        public ItemStatChange Item_ModifyStats { get; set; }
        public ItemHoldItem Item_HoldItemHook { get; set; }
        public ProjectileSetDefaults Projectile_SetDefaultsHook { get; set; }
        public ProjectileOnSpawn Projectile_OnSpawnHook { get; set; }
        public ProjectileAI Projectile_AIHook { get; set; }
        public ProjectileOnHitNPC Projectile_OnHitNPCHook { get; set; }
        public ProjectilePreDraw Projectile_PreDrawHook { get; set; }
        public ProjectileModifyHitNPC Projectile_ModifyHitNPCHook { get; set; }
        public ProjectilePreKill Projectile_PreKillHook { get; set; }
        public ItemOnShoot Item_OnShootHook { get; set; }
        public ProjectileOnTileCollide Projectile_OnTileCollideHook { get; set; }
        public ItemModifyShootStats Item_ModifyShootStatsHook { get; set; }
        #endregion
    }
}
