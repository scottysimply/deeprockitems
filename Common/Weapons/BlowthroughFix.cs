using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using deeprockitems.Content.Items.Weapons;
using static System.Math;
using deeprockitems.Content.Items.Upgrades;

namespace deeprockitems.Common.Weapons
{
    public class BlowthroughFix : GlobalProjectile
    {
        bool UpgradeEquipped;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            UpgradeEquipped = false;
            if (source is EntitySource_ItemUse_WithAmmo parent && parent.Item.ModItem is JuryShotgun boomstick)
            {
                foreach (int i in boomstick.Upgrades2)
                {
                    if (i == ModContent.ItemType<Blowthrough>())
                    {
                        UpgradeEquipped = true;
                        projectile.penetrate = 5;
                        projectile.usesIDStaticNPCImmunity = true;
                        projectile.idStaticNPCHitCooldown = 1;
                    }
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (UpgradeEquipped)
            {
                projectile.damage = (int)Floor(projectile.damage * .7f);
            }

        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                if (UpgradeEquipped)
                {
                    projectile.damage = (int)Floor(projectile.damage * .7f);
                }
            }
        }
    }
}