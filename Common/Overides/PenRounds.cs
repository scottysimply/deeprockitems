using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using deeprockitems.Content.Items.Weapons;
using static System.Math;

namespace deeprockitems.Common.Overides
{
    public class PenRounds : GlobalProjectile
    {
        bool UpgradeEquipped;
        bool Shotgun;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            UpgradeEquipped = false;
            Shotgun = false;
            if (source is EntitySource_ItemUse parent && parent.Item.ModItem is UpgradeableItemTemplate item)
            {
                if (item.Upgrades[5])
                {
                    UpgradeEquipped = true;
                }
                if (item is JuryShotgun)
                {
                    Shotgun = true;
                }
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (UpgradeEquipped)
            {
                int defense = target.defense;
                int calc_damage = (int)Floor(damage - defense * .5);

                if (defense <= 10)
                {
                    damage += defense;
                }
                else
                {
                    if (Shotgun)
                    {
                        damage = (int)(Floor(defense * .5 + 10) / projectile.ai[0]);
                        return;
                    }
                    damage = (int)Floor(defense * .5 + 10);
                    
                }
            }

        }
    }
}
