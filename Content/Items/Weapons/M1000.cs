using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using deeprockitems.Content.Projectiles;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("M1000 Classic");
            Tooltip.SetDefault("'From A to D, skipping B and C!'\n" +
                               "Hold click to fire a focused shot");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 50;
            Item.height = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f;
            Item.rare = ItemRarityID.Pink;
            Item.scale = .7f;
            Item.value = 960000;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<M1000Helper>();
        }
    }
}