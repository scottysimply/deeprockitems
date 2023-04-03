using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using static deeprockitems.MyFunctions;
using static System.Math;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace deeprockitems.Content.Items.Weapons
{
    public class JuryShotgun : ModItem
    {
        public byte Upgrades;
        public BitsByte ByteHelper;
        private string CurrentOverclock;
        private string OverclockPositives;
        private string OverclockNegatives;
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
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 3 + Main.rand.Next(1, 4);
            double spread = PI / 7;
            int pelletDamage = damage;
            if (ByteHelper[6] && !ByteHelper[7]) // Reduced spread, but lower damage per pellet
            {
                spread *= .5;
            }
            else if (!ByteHelper[6] && ByteHelper[7]) // twice amount of pellets, much more spread and lower firerate
            {
                numberProjectiles *= 2;
                spread *= 2;
            }
            // This block is for the projectile spread.

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(spread) * Main.rand.NextFloat(.8f, 1.2f); // around 25 degrees
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
            if (ByteHelper[6] && ByteHelper[7])
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
            /*int powderTaken = 0;
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
            }*/
        }
        /*public static int ConsumePowder(Item currentItem, int powder)
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
        }*/
        public override void RightClick(Player player)
        {
            ByteHelper = Upgrades;
            Item.stack += 1;
            if (player.HeldItem.type == ModContent.ItemType<Overclocks.PelletAlignmentOC>())
            {
                Main.mouseItem.stack -= 1;
                Main.mouseItem.maxStack = 0;
                ByteHelper[6] = true;
                ByteHelper[7] = false;
            }
            else if (player.HeldItem.type == ModContent.ItemType<Overclocks.SpecialPowderOC>())
            {
                Main.mouseItem.stack -= 1;
                Main.mouseItem.maxStack = 0;
                ByteHelper[6] = true;
                ByteHelper[7] = true;
            }
            else if ((player.HeldItem.type == ModContent.ItemType<Overclocks.StuffedShellsOC>()))
            {
                Main.mouseItem.stack -= 1;
                Main.mouseItem.maxStack = 0;
                ByteHelper[6] = false;
                ByteHelper[7] = true;
            }
            else
            {
                ByteHelper[6] = false;
                ByteHelper[7] = false;
            }
            UpdateUpgrades();
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void SaveData(TagCompound tag)
        {
            Upgrades = ByteHelper;
            tag["WeaponUpgrades"] = Upgrades;
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("WeaponUpgrades"))
            {
                Upgrades = (byte)tag["WeaponUpgrades"];
            }
            else
            {
                Upgrades = 0;
            }
            ByteHelper = Upgrades;
            UpdateUpgrades();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line;
            if (ByteHelper[6] | ByteHelper[7])
            {
                line = new(Mod, "Upgrades", string.Format("This weapon has the following overclock: [c/4AB1D3:{0}]", CurrentOverclock));
                tooltips.Add(line);
                line = new(Mod, "Positives", OverclockPositives)
                {
                    OverrideColor = new(35, 223, 26),
                };
                tooltips.Add(line);
                if (OverclockNegatives != "")
                {
                    line = new(Mod, "Negatives", OverclockNegatives)
                    {
                        OverrideColor = new(240, 19, 24)
                    };
                    tooltips.Add(line);
                }

                line = new(Mod, "RemoveOC", "Right click without an item to remove the overclock");
                tooltips.Add(line);

            }



        }
        private void UpdateUpgrades()
        {
            if (ByteHelper[6] && !ByteHelper[7])
            {
                Item.damage = (int)Floor(.75f * Item.damage);
                Item.useTime = 39;
                Item.useAnimation = 39;
                CurrentOverclock = "Magnetic Pellet Alignment";
                OverclockPositives = "▶Reduced spread";
                OverclockNegatives = "▶Lower damage output";
            }
            else if (ByteHelper[6] && ByteHelper[7])
            {
                Item.useTime = 39;
                Item.useAnimation = 39;
                CurrentOverclock = "Special Powder";
                OverclockPositives = "▶Launch yourself with each shot";
                OverclockNegatives = "";
            }
            else if (!ByteHelper[6] && ByteHelper[7])
            {
                Item.useTime = 50;
                Item.useAnimation = 50;
                CurrentOverclock = "Stuffed Shells";
                OverclockPositives = "▶Each shot produces twice as many shells";
                OverclockNegatives = "▶Heavily increased spread\n" +
                                     "▶Lower fire rate";
            }
            else
            {
                Item.useTime = 39;
                Item.useAnimation = 39;
            }
        }
    }
}