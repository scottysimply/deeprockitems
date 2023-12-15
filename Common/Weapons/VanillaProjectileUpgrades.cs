using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using static System.Math;
using Terraria.ID;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Items.Upgrades.M1000Upgrades;
using deeprockitems.Content.Items.Upgrades.PlasmaPistolUpgrades;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Content.Projectiles.SludgeProjectile;
using deeprockitems.Content.Projectiles.PlasmaProjectiles;

namespace deeprockitems.Common.Weapons
{
    public class VanillaProjectileUpgrades : GlobalProjectile
    {
        private int[] upgrades = new int[4];
        private int thisPlayer = 0;
        private bool fromM1000 = false;
        private bool fromModItem = false;
        private bool? canDamage = null;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate sourceItem })
            {
                fromModItem = true;
                if (sourceItem is M1000)
                {
                    fromM1000 = true;
                }
                upgrades = sourceItem.Upgrades;
            }
            if (source is EntitySource_ItemUse { Player: Player player })
            {
                thisPlayer = player.whoAmI;
            }

            // Upgrades!!
            if (fromModItem)
            {
                // If projectile fully charged
                if (projectile.ai[0] <= 900 && projectile.ai[0] > 0)
                {
                    if (projectile.ai[2] == ModContent.ItemType<SupercoolOC>())
                    {
                        projectile.damage *= 2;
                    }
                    if (projectile.ai[2] == ModContent.ItemType<DiggingRoundsOC>())
                    {
                        projectile.tileCollide = false;
                    }
                    if (fromM1000 && projectile.maxPenetrate < 5)
                    {
                        projectile.penetrate += 2;
                        projectile.maxPenetrate += 2;
                    }

                }
                if (upgrades.Contains(ModContent.ItemType<Blowthrough>()))
                {
                    projectile.penetrate = 5;
                    projectile.maxPenetrate = 5;
                }
                if (projectile.ai[2] == ModContent.ItemType<MountainMoverOC>())
                {
                    canDamage = false;
                }


            }
            base.OnSpawn(projectile, source);
        }
        public override void AI(Projectile projectile)
        {
            if (fromM1000 && projectile.type != ProjectileID.ChlorophyteBullet && projectile.type != ModContent.ProjectileType<M1000Helper>())
            {

                if (projectile.type != ProjectileID.ChlorophyteBullet)
                {
                    projectile.velocity.Y -= 0.1f; // You wouldn't understand the gravity of this situation...
                }
            }
            base.AI(projectile);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fromModItem)
            {
                if (projectile.type != ModContent.ProjectileType<PlasmaExplosion>() && projectile.type != ModContent.ProjectileType<SludgeExplosion>())
                {
                    projectile.damage = (int)Floor(projectile.damage * .7f);
                }

                if (upgrades.Contains(ModContent.ItemType<HotPlasma>()))
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
            }
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
        public override bool? CanDamage(Projectile projectile)
        {
            return canDamage;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (fromModItem)
            {
                Point spawnTile = projectile.Center.ToTileCoordinates();
                // Move projectile left
                if (oldVelocity.X > projectile.velocity.X)
                {
                    spawnTile.X--;
                }
                // Move projectile right
                if (oldVelocity.X < projectile.velocity.X)
                {
                    spawnTile.X++;
                }
                // Move projectile up
                if (oldVelocity.Y > projectile.velocity.Y)
                {
                    spawnTile.Y--;
                }
                // Move projectile down
                if (oldVelocity.Y < projectile.velocity.Y)
                {
                    spawnTile.Y++;
                }
                // Spawn projectile at this new position.
                Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<Content.Projectiles.ZhukovProjectiles.CryoMineletProjectile>(), projectile.damage, 0f, projectile.owner, ai0: 40f, ai1: spawnTile.X, ai2: spawnTile.Y);
            }
            return true;
        }
    }
}
