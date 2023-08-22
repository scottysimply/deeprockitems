using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades;
using static System.Math;

namespace deeprockitems.Common.Weapons
{
    public class PenRounds : GlobalProjectile
    {
        bool UpgradeEquipped;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            UpgradeEquipped = false;
            if (source is EntitySource_ItemUse parent && parent.Item.ModItem is UpgradeableItemTemplate item)
            {
                foreach (int i in item.Upgrades)
                {
                    if (i == ModContent.ItemType<ArmorPierce>())
                    {
                        UpgradeEquipped = true;
                    }
                }
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (UpgradeEquipped)
            {
                if (target.defense < 10)
                {
                    modifiers.FinalDamage.Flat += target.defense;
                    return;
                }
                modifiers.FinalDamage.Flat += 10;
            }
        }
    }
}
