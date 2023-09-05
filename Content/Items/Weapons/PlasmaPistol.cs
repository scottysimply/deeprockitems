using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Items.Upgrades.PlasmaPistol;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class PlasmaPistol : UpgradeableItemTemplate
    {
        public override void SafeDefaults()
        {
            Item.damage = 10;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 18f;
            Item.channel = true;
            Item.noMelee = true;

            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Remove(ModContent.ItemType<Blowthrough>());
            ValidUpgrades.Add(ModContent.ItemType<PiercingPlasmaOC>());
            ValidUpgrades.Add(ModContent.ItemType<EzBoomOC>());
            ValidUpgrades.Add(ModContent.ItemType<MountainMoverOC>());
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<Projectiles.PlasmaProjectiles.PlasmaPistolHelper>();

        }
    }
}