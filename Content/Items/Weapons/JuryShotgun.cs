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
using deeprockitems.Utilities;
using deeprockitems.Content.Items.Upgrades.JuryShotgunUpgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : UpgradeableItemTemplate
    {
        public int oldFireRate = 0;
        public int newFireRate = 0;
        public override void SetStaticDefaults()
        {
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

            ValidUpgrades.Add(ModContent.ItemType<PelletAlignmentOC>());
            ValidUpgrades.Add(ModContent.ItemType<SpecialPowderOC>());
            ValidUpgrades.Add(ModContent.ItemType<StuffedShellsOC>());

            ValidUpgrades.Add(ModContent.ItemType<WhitePhosphorus>());
            ValidUpgrades.Add(ModContent.ItemType<BumpFire>());
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 4 + Main.rand.Next(1, 3);
            double spread = PI / 13;
            if (Upgrades.Contains(ModContent.ItemType<PelletAlignmentOC>())) // Reduced spread
            {
                spread *= .5;
            }
            else if (Upgrades.Contains(ModContent.ItemType<StuffedShellsOC>())) // twice amount of pellets, much more spread and lower firerate
            {
                numberProjectiles *= 2;
                spread *= 2;
            }
            // This block is for the projectile spread.
            float VelocityReducer = (Upgrades.Contains(ModContent.ItemType<PelletAlignmentOC>())) ? 1f : .8f;
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
            if (Upgrades.Contains(ModContent.ItemType<SpecialPowderOC>()))
            {
                ShotgunJump(player, mousePos);
            }
            if (Upgrades.Contains(ModContent.ItemType<WhitePhosphorus>()))
            {
                WPIgnite();
            }

            return false;
        }
        public override void AddRecipes()
        {
            Recipe JuryShotgun = Recipe.Create(ModContent.ItemType<JuryShotgun>());
            JuryShotgun.AddIngredient(ItemID.Boomstick, 1);
            JuryShotgun.AddIngredient(ItemID.IllegalGunParts);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.DemoniteBar), 8);
            JuryShotgun.AddRecipeGroup(nameof(ItemID.VilePowder), 10);
            JuryShotgun.AddTile(TileID.Anvils);
            JuryShotgun.Register();
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
        public override void UniqueUpgrades()
        {
            if (Overclock == ModContent.ItemType<PelletAlignmentOC>())
            {
                DamageScale = 1f;
                newFireRate = 39;
            }
            else if (Overclock == ModContent.ItemType<SpecialPowderOC>())
            {
                DamageScale = .75f;
                newFireRate = 39;
            }
            else if (Overclock == ModContent.ItemType<StuffedShellsOC>())
            {
                DamageScale = 1f;
                newFireRate = 50;
            }
            else
            {
                DamageScale = 1f;
                newFireRate = 39;
            }
            foreach (int i in Upgrades)
            {
                if (i == ModContent.ItemType<BumpFire>())
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