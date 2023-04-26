using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using static System.Math;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using deeprockitems.UI;
using deeprockitems.Content.Items.Upgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : UpgradeableItemTemplate
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jury-Rigged Boomstick");
            Tooltip.SetDefault("'See ya, suckers!'\n");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Boomstick);
            Item.damage = 16;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 39;
            Item.useAnimation = 39;
            Item.value = 150000;
            ValidUpgrades[0] = ModContent.ItemType<PelletAlignmentOC>();
            ValidUpgrades[1] = ModContent.ItemType<SpecialPowderOC>();
            ValidUpgrades[2] = ModContent.ItemType<StuffedShellsOC>();
            ValidUpgrades[3] = ModContent.ItemType<DamageUpgrade>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 3 + Main.rand.Next(1, 3);
            double spread = PI / 13;
            if (((BitsByte)Upgrades)[0]) // Reduced spread
            {
                spread *= .5;
            }
            else if (((BitsByte)Upgrades)[2]) // twice amount of pellets, much more spread and lower firerate
            {
                numberProjectiles *= 2;
                spread *= 2;
            }
            // This block is for the projectile spread.
            float VelocityReducer = ((BitsByte)Upgrades)[0] ? 1f : .8f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(spread) * Main.rand.NextFloat(VelocityReducer, 1.2f); // random velocity effect
                Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            // This block and helper functions below manage the shotgun jump
            Vector2 mousePos = Main.MouseWorld - player.Center;
            if (Main.MouseWorld.X > player.Center.X) // Change player's direction to face the gun.
            {
                player.direction = 1;
            }
            else
            {
                player.direction = -1;
            }
            if (((BitsByte)Upgrades)[1])
            {
                ShotgunJump(player, mousePos);
            }
            return true;
        }
        public static void ShotgunJump(Player player, Vector2 mousePos)
        {
            float speedCap = 15f;
            player.velocity -= Vector2.Normalize(mousePos) * 10;
            if (Abs(player.velocity.X) > speedCap)
            {
                player.velocity.X = speedCap * Sign(player.velocity.X);
            }
            if (player.velocity.Y < 5f)
            {
                player.fallStart = (int)player.position.Y / 16;
            }
        }
        public override void IndividualUpgrades()
        {
            if (((BitsByte)Upgrades)[0])
            {
                DamageScale = 1f;
                Item.useTime = 39;
                Item.useAnimation = 39;
                CurrentOverclock = "Magnetic Pellet Alignment";
                OverclockPositives = "▶Reduced spread";
                OverclockNegatives = "";
            }
            else if (((BitsByte)Upgrades)[1])
            {
                DamageScale = .75f;
                Item.useTime = 39;
                Item.useAnimation = 39;
                CurrentOverclock = "Special Powder";
                OverclockPositives = "▶Launch yourself with each shot";
                OverclockNegatives = "▶Lower damage output";
            }
            else if (((BitsByte)Upgrades)[2])
            {
                DamageScale = 1f;
                Item.useTime = 50;
                Item.useAnimation = 50;
                CurrentOverclock = "Stuffed Shells";
                OverclockPositives = "▶Each shot produces twice as many shells";
                OverclockNegatives = "▶Heavily increased spread\n" +
                                     "▶Lower fire rate";
            }
            else
            {
                DamageScale = 1f;
                Item.useTime = 39;
                Item.useAnimation = 39;
            }

        }
    }
}