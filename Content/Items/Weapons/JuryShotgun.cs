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
        public int oldFireRate = 0;
        public int newFireRate = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jury-Rigged Boomstick");
            Tooltip.SetDefault("'See ya, suckers!'\n");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }

        public override void SafeDefaults()
        {
            Item.CloneDefaults(ItemID.Boomstick);
            Item.damage = 16;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 39;
            Item.useAnimation = 39;
            Item.value = 150000;

            oldFireRate = Item.useTime;

            ValidUpgrades[0] = ModContent.ItemType<PelletAlignmentOC>();
            ValidUpgrades[1] = ModContent.ItemType<SpecialPowderOC>();
            ValidUpgrades[2] = ModContent.ItemType<StuffedShellsOC>();

            ValidUpgrades[4] = ModContent.ItemType<WhitePhosphorous>();
            ValidUpgrades[6] = ModContent.ItemType<BumpFire>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 4 + Main.rand.Next(1, 3);
            double spread = PI / 13;
            if (Upgrades[0]) // Reduced spread
            {
                spread *= .5;
            }
            else if (Upgrades[2]) // twice amount of pellets, much more spread and lower firerate
            {
                numberProjectiles *= 2;
                spread *= 2;
            }
            // This block is for the projectile spread.
            float VelocityReducer = Upgrades[0] ? 1f : .8f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(spread) * Main.rand.NextFloat(VelocityReducer, 1.2f); // random velocity effect
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, numberProjectiles);
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
            if (Upgrades[1])
            {
                ShotgunJump(player, mousePos);
            }
            if (Upgrades[4])
            {
                WPIgnite();
            }
            return false;
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
            if (Upgrades[0])
            {
                DamageScale = 1f;
                newFireRate = 39;
                CurrentOverclock = "Magnetic Pellet Alignment";
                OverclockPositives = "▶Reduced spread";
                OverclockNegatives = "";
            }
            else if (Upgrades[1])
            {
                DamageScale = .75f;
                newFireRate = 39;
                CurrentOverclock = "Special Powder";
                OverclockPositives = "▶Launch yourself with each shot";
                OverclockNegatives = "▶Lower damage output";
            }
            else if (Upgrades[2])
            {
                DamageScale = 1f;
                newFireRate = 50;
                CurrentOverclock = "Stuffed Shells";
                OverclockPositives = "▶Each shot produces twice as many shells";
                OverclockNegatives = "▶Heavily increased spread\n" +
                                     "▶Lower fire rate";
            }
            else
            {
                DamageScale = 1f;
                newFireRate = 39;
            }
            if (Upgrades[6])
            {
                Item.useAnimation = (int)Ceiling(newFireRate * .83f);
                Item.useTime = (int)Ceiling(newFireRate * .83f);
            }
            else
            {
                Item.useAnimation = oldFireRate;
                Item.useTime = oldFireRate;
            }

        }
        private void WPIgnite()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.friendly) { continue; }
                if (Vector2.DistanceSquared(Main.player[Main.myPlayer].position, npc.position) <= 10000)
                {
                    npc.AddBuff(BuffID.OnFire, 360);
                }
            }
        }
    }
}