using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using deeprockitems.Content.Items.Weapons;
using static System.Math;

namespace deeprockitems.Common.Overides
{
    public class BlowthroughFix : GlobalProjectile
    {
        bool UpgradeEquipped;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            UpgradeEquipped = false;
            if (source is EntitySource_ItemUse_WithAmmo parent && parent.Item.ModItem is JuryShotgun boomstick && boomstick.Upgrades[7])
            {
                UpgradeEquipped = true;
                projectile.penetrate = 5;
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 1;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (UpgradeEquipped)
            {
                projectile.damage = (int)Floor(projectile.damage * .7f);
            }

        }
        public override void OnHitPvp(Projectile projectile, Player target, int damage, bool crit)
        {
            if (UpgradeEquipped)
            {
                projectile.damage = (int)Floor(projectile.damage * .7f);
            }
        }
    }
}