using deeprockitems.Common.EntitySources;
using System;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Projectiles;

namespace deeprockitems.Content.Upgrades
{
    #region Delegates for upgrade behavior
    /// <summary>
    /// Called to modify an item's stats when the upgrade is equipped.
    /// </summary>
    /// <param name="item"></param>
    public delegate void ItemStatChange(Item item);
    /// <summary>
    /// Called to add behavior while an item is held and the upgrade is equipped. <br/>
    /// <inheritdoc cref="Terraria.ModLoader.ModItem.HoldItem"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="player"></param>
    public delegate void ItemHoldItem(Item item, Player player);
    /// <summary>
    /// Called to change every instance of a spawned projectile when the upgrade is equipped. Make sure that the type of the projectile is checked (ie, <c>if (projectile.type === [x])</c>).
    /// </summary>
    /// <param name="projectile"></param>
    public delegate void ProjectileSetDefaults(Projectile projectile);
    /// <summary>
    /// Called to modify a projectile after spawning when the upgrade is equipped. <br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.OnSpawn"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="source"></param>
    public delegate void ProjectileOnSpawn(Projectile projectile, IEntitySource source);
    /// <summary>
    /// Called to modify projectile AI while an upgrade is equipped. <br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.AI"/>
    /// </summary>
    /// <param name="projectile"></param>
    public delegate void ProjectileAI(Projectile projectile);
    /// <summary>
    /// Called to add on hit effects while the upgrade is equipped. <br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.OnHitNPC"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="target"></param>
    /// <param name="hit"></param>
    /// <param name="damageDone"></param>
    public delegate void ProjectileOnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone);
    /// <summary>
    /// Called to modify projectile drawing or cancel it entirely, while the upgrade is equipped. <br/>
    /// Returns will short circuit; if one upgrade hook returns true, they all will.<br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.PreDraw"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="lightColor"></param>
    /// <returns></returns>
    public delegate bool ProjectilePreDraw(Projectile projectile, Color lightColor);
    /// <summary>
    /// Called to modify hits to an NPC when the upgrade is equipped.<br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.ModifyHitNPC"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="target"></param>
    /// <param name="modifiers"></param>
    public delegate void ProjectileModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers);
    /// <summary>
    /// Called to manually shoot projectiles when the upgrade is equipped.<br/>
    /// Returns will short circuit; if one upgrade hook returns false, they all will.<br/>
    /// <inheritdoc cref="Terraria.ModLoader.ModItem.Shoot"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="source"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="type"></param>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    /// <param name="spread"></param>
    /// <returns></returns>
    public delegate bool ItemOnShoot(Item item, Player player, EntitySource_FromUpgradableWeapon source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread);
    /// <summary>
    /// Called right before a projectile would be killed when the upgrade is equipped. <br/>
    /// Returns will short circuit; if one upgrade hook returns false, they all will.<br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.PreKill"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="timeLeft"></param>
    /// <returns></returns>
    public delegate bool ProjectilePreKill(Projectile projectile, int timeLeft);
    /// <summary>
    /// Called right before a tile collision would occur when the upgrade is equipped. <br/>
    /// Returns will short circuit; if one upgrade hook returns false, they all will.<br/>
    /// <inheritdoc cref="Terraria.ModLoader.GlobalProjectile.OnTileCollide"/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="oldVelocity"></param>
    /// <returns></returns>
    public delegate bool ProjectileOnTileCollide(Projectile projectile, Vector2 oldVelocity);
    /// <summary>
    /// Called to modify the shoot parameters when the upgrade is equipped. <br/>
    /// <inheritdoc cref="Terraria.ModLoader.ModItem.ModifyShootStats"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="type"></param>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    /// <param name="spread"></param>
    public delegate void ItemModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread);
    /// <summary>
    /// Called to modify the shoot parameters on the <i>held projectile</i> when the upgrade is equipped. Only called if the original projectile shot by the item inherited from <see cref="HeldProjectileBase"></see>.<br/>
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="type"></param>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    /// <param name="spread"></param>
    public delegate void HeldProjectileModifyShootStats(HeldProjectileBase projectile, Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, ref float spread);
    /// <summary>
    /// Called to manually shoot projectiles on the <i>held projectile</i> when the upgrade is equipped. Only called if the original projectile shot by the item inherited from <see cref="HeldProjectileBase"></see>.<br/>
    /// Returns will short circuit; if one upgrade hook returns false, they all will.
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="source"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="type"></param>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    /// <param name="spread"></param>
    /// <returns></returns>
    public delegate bool HeldProjectileShoot(HeldProjectileBase projectile, Item item, Player player, EntitySource_FromHeldProjectile source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float spread);
    ///<summary>
    /// Called immediately after the projectile is spawned on the <i>held projectile</i> when the upgrade is equipped AND the Shoot() returned true. Only called if the original projectile shot by the item inherited from <see cref="HeldProjectileBase"></see>.<br/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="projectile"></param>
    public delegate void HeldProjectilePostSpawn(Projectile projectile, EntitySource_FromHeldProjectile source);
    
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
        public ItemOnShoot Item_ShootHook { get; set; }
        public ProjectileOnTileCollide Projectile_OnTileCollideHook { get; set; }
        public ItemModifyShootStats Item_ModifyShootStatsHook { get; set; }
        public HeldProjectileModifyShootStats HeldProjectile_ModifyShootStatsHook { get; set; }
        public HeldProjectileShoot HeldProjectile_ShootHook { get; set; }
        public HeldProjectilePostSpawn HeldProjectile_PostSpawnHook { get; set; }
        #endregion
    }
}
