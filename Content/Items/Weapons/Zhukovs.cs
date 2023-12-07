using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradeableItemTemplate
    {
        public override void SafeDefaults()
        {
            Item.width = 56;
            Item.height = 50;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 46;
            Item.knockBack = 0.4f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 5;
            Item.useAmmo = AmmoID.Bullet;

            Item.useStyle = 0;
            Item.useTime = 10;
            Item.useAnimation = 30;


            Item.value = Item.sellPrice(0, 6, 50, 0);

        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int frame = 3;
                player.bodyFrame.Y = player.bodyFrame.Height * frame;
                player.itemLocation = heldItemFrame.Location.ToVector2();
            }
        }
        public override void UseAnimation(Player player)
        {

        }
    }
}
