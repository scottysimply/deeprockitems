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
using deeprockitems.Content.Items.Upgrades.Overclocks;

namespace deeprockitems.Common.Weapons
{
    public class VanillaProjectileUpgrades : GlobalProjectile
    {
        private int[] upgrades = new int[4];
        private int thisPlayer = 0;
        private bool fromM1000 = false;
        private bool fromModItem = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile entity)
        {
            entity.arrow = false;
        }
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
                    if (upgrades.Contains(ModContent.ItemType<SupercoolOC>()))
                    {
                        projectile.damage *= 2;
                    }
                }
                if (upgrades.Contains(ModContent.ItemType<Blowthrough>()))
                {
                    projectile.penetrate = 5;
                }
                if (upgrades.Contains(ModContent.ItemType<DiggingRoundsOC>()))
                {
                    projectile.tileCollide = false;
                }
            }
        }
        public override void AI(Projectile projectile)
        {
            if (fromM1000)
            {

                if (projectile.type != ProjectileID.ChlorophyteBullet)
                {
                    projectile.velocity.Y -= 0.1f; // You wouldn't understand the gravity of this situation...
                }

            }
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (fromModItem)
            {
                
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fromModItem)
            {
                projectile.damage = (int)Floor(projectile.damage * .7f);
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (fromModItem)
            {
                projectile.damage = (int)Floor(projectile.damage * .7f);
            }
        }
    }
}
