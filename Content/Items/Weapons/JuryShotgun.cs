using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using static deeprockitems.MyFunctions;
using static System.Math;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jury-Rigged Boomstick");
            Tooltip.SetDefault("'See ya, suckers!'\n" +
                               "Uses evil powder as propellant");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Boomstick);
            Item.damage = 16;
            Item.width = 40;
            Item.height = 16;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.value = 150000;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This block is for the projectile spread.
            int numberProjectiles = 3 + Main.rand.Next(3);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(PI/7); // around 25 degrees
                Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            } 

            // This block and helper functions below manage the shotgun jump
            Vector2 mousePos = MouseRel(Main.MouseWorld, player);
            if (Main.MouseWorld.X > player.Center.X) // Change player's direction to face the gun.
            {
                player.direction = 1;
            }
            else
            {
                player.direction = -1;
            }
            ShotgunJump(player, mousePos);
            return true;
        }
        public static void ShotgunJump(Player player, Vector2 mousePos)
        {
            float speedCap = 15f;
            int powderTaken = 0;
            for (int i = 0; i < 49; i++) // iterating through player's inventory. slots 0-49 start in top left of hotbar to bottom right, above trash can.
            {
                if ((player.inventory[i].type == ItemID.VilePowder) || (player.inventory[i].type == ItemID.ViciousPowder)) // Look for either vicious or vile powder.
                {
                    powderTaken = ConsumePowder(player.inventory[i], powderTaken);
                    if (powderTaken == -1)
                    {
                        powderTaken = 3;
                        break;
                    }
                }
            }
            if (powderTaken > 0)
            {
                player.velocity -= Vector2.Normalize(mousePos) * ((10 / 3) * powderTaken);
                if (Abs(player.velocity.X) > speedCap)
                {
                    player.velocity.X = speedCap * Sign(player.velocity.X);
                }
                if (player.velocity.Y < 5f)
                {
                    player.fallStart = (int)player.position.Y / 16;
                }
            }
        }
        public static int ConsumePowder(Item currentItem, int powder)
        {
            for (int x = 0; x < 3; x++) // takes up to 3 powder
            {
                if (powder == 3)
                {
                    return -1;
                }
                currentItem.stack -= 1;
                powder++;
                if (currentItem.stack <= 0)
                {
                    currentItem.maxStack = 0; // this is a bug in 1.4, TurnToAir() doesn't reset maxStack, which determines whether right click should swap an item. this prevents that skissue
                    currentItem.TurnToAir();
                    return powder;
                }
            }
            return powder;
        }
    }
}