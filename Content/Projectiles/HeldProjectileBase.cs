﻿using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static System.Math;

namespace deeprockitems.Content.Projectiles;
/// <summary>
/// This class handles the spawning of held projectiles. Override 
/// </summary>
public abstract class HeldProjectileBase : ModProjectile
{
    protected int ProjectileToSpawn { get; set; }
    protected float TOTAL_TIME { get; set; } // Not constant, but screaming snake case highlights that this field will almost never change.
    protected SoundStyle Charge_Sound { get; set; }
    protected SoundStyle Fire_Sound { get; set; }
    private float timer;
    private Player projectileOwner;
    public override void SetDefaults()
    {
        Projectile.height = 2;
        Projectile.width = 2;

    }
    public override void OnSpawn(IEntitySource source)
    {
        projectileOwner = Main.player[Projectile.owner];

        // Check for upgrades. Due to my new backend, we don't have to check if this upgrade is valid! So we can run this on any item.
        if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate parent_weapon})
        {
            if (parent_weapon.Upgrades.Contains(ModContent.ItemType<QuickCharge>()))
            {
                TOTAL_TIME *= .75f;
            }
            if (parent_weapon.Upgrades.Contains(ModContent.ItemType<SupercoolOC>()))
            {
                TOTAL_TIME *= 1.33f;
            }
        }
        SpecialOnSpawn(source);
    }
    public virtual void SpecialOnSpawn(IEntitySource source) { }

    public override void AI()
    {
        if (!SpecialAI()) return; // Exit if SpecialAI() returns false!!! Very important!!

        // Continue as normal

    }
    public virtual bool SpecialAI() { return true; }

    // This is for when the projectile is killed. Spawn the new projectile, play sound, etc.
    public override void Kill(int timeLeft)
    {
        SpecialKill(timeLeft);
    }
    public virtual void SpecialKill(int timeLeft) { }

    public override bool? CanDamage()
    {
        return false; // This weapon is invisible and intangible. We don't want the player to know it exists.
    }
    public override bool ShouldUpdatePosition()
    {
        return false; // We want this projectile to stay on the player, which will be done manually.
    }



    // So what's going on in this method? Deconstruction time!
    private void HoldItemOut(Player player)
    {
        // So fun fact about the way the game handles rotation: values go from -Pi to +Pi. There is no 0 to 2Pi.
        // For some god awful reason though, itemRotation doesn't match DirectionTo().ToRotation() of the mouse, but only when the itemRotation is in quadrant 4.

        // If cursor is to the right of the player
        if (Main.MouseWorld.X > player.Center.X)
        {
            // See, this is easy!
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            player.direction = 1; // Make the player face right
            return;
        }
        // If cursor is above the player
        if (Main.MouseWorld.Y < player.Center.Y)
        {
            // Here's where it messes up. If the cursor is in quadrant II, it needs to add PI
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
        }
        // If cursor is below the player
        else
        {
            // But if the cursor is in Quadrant III, it has to subtract. guh??
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - (float)PI;
        }
        player.direction = -1; // Make the player face left
    }
}