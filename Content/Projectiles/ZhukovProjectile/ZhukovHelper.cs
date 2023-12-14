using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using deeprockitems.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;

namespace deeprockitems.Content.Projectiles.ZhukovProjectile
{
    public class ZhukovHelper : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.hide = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            _projectileOwner = Main.player[Projectile.owner];
            if (source is EntitySource_ItemUse_WithAmmo ammoSource)
            {
                _ammo = ammoSource.AmmoItemIdUsed;
            }
        }
        public override string Texture => "Terraria/Images/MagicPixel";
        public override bool? CanDamage() => false;
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override bool ShouldUpdatePosition() => false;
        public int ProjectileToSpawn { get; set; } = ProjectileID.Bullet;
        private float bulletTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        private Player _projectileOwner { get => Main.player[Projectile.owner]; set => Main.player[Projectile.owner] = value; }
        private int _ammo;

        private int _timeBetweenShots = 25;
        public override void AI()
        {
            // Adjust spawning location
            Projectile.Center = _projectileOwner.Center;
            Projectile.position.Y -= 8f;

            if (!_projectileOwner.channel)
            {
                Projectile.Kill();
            }
            // Spawnlocation fixing
            Vector2 spawn_location = Projectile.Center;
            // spawn_location = new()

            if (bulletTimer % _timeBetweenShots == 0)
            {
                // Play shoot sound
                SoundEngine.PlaySound(SoundID.Item41);

                // Fix velocity
                float shoot_speed = Projectile.velocity.Distance(new(0, 0)); // This is the magnitude of the velocity
                Projectile.velocity = shoot_speed * _projectileOwner.Center.DirectionTo(Main.MouseWorld); // A vector is just magnitude and direction

                // Spawn projectile
                Projectile proj = Projectile.NewProjectileDirect(_projectileOwner.GetSource_ItemUse_WithPotentialAmmo(_projectileOwner.HeldItem, _ammo), Projectile.Center, Projectile.velocity, ProjectileToSpawn, Projectile.damage, Projectile.knockBack);
                proj.rotation = new Vector2(0, 0).DirectionTo(proj.velocity).ToRotation() - MathHelper.Pi / 2; // No sideways projectiles!
            }
            if (bulletTimer % _timeBetweenShots == 5)
            {
                // Play shoot sound
                SoundEngine.PlaySound(SoundID.Item41);

                // Fix velocity
                float shoot_speed = Projectile.velocity.Distance(new(0, 0)); // This is the magnitude of the velocity
                Projectile.velocity = shoot_speed * _projectileOwner.Center.DirectionTo(Main.MouseWorld); // A vector is just magnitude and direction

                // Get spawn offset
                Vector2 offset = _projectileOwner.itemLocation - _projectileOwner.GetModPlayer<Common.PlayerLayers.DualWieldPlayer>().OffHandItemLocation;
                
                // Spawn with offset
                Projectile proj = Projectile.NewProjectileDirect(_projectileOwner.GetSource_ItemUse_WithPotentialAmmo(_projectileOwner.HeldItem, _ammo), Projectile.Center + offset, Projectile.velocity, ProjectileToSpawn, Projectile.damage, Projectile.knockBack);
                proj.rotation = new Vector2(0, 0).DirectionTo(proj.velocity).ToRotation() - MathHelper.Pi / 2; // No sideways projectiles!
            }
            bulletTimer++;
            HoldItemOut(_projectileOwner);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        private void HoldItemOut(Player player)
        {
            // So fun fact about the way the game handles rotation: values go from -Pi to +Pi. There is no 0 to 2Pi.
            // For some god awful reason though, when the mouse is in Quadrant II, itemRotation doesn't match DirectionTo().ToRotation() of the mouse.

            // Make sure the player appears to actually hold the projectile.
            player.itemTime = player.itemAnimation = _timeBetweenShots;

            // If cursor is to the right of the player
            if (Main.MouseWorld.X > player.Center.X)
            {
                // See, this is easy!
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
                player.ChangeDir(1); // Make the player face right
                return;
            }
            // If cursor is above the player
            if (Main.MouseWorld.Y < player.Center.Y)
            {
                // Here's where it messes up. If the cursor is in quadrant II, it needs to add PI
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.Pi;
            }
            // If cursor is below the player
            else
            {
                // But if the cursor is in Quadrant III, it has to subtract. guh??
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.Pi;
            }
            // Make the player face left
            player.ChangeDir(-1);

        }
    }
}
