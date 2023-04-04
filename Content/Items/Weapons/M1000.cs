using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using deeprockitems.Content.Projectiles;
using Terraria.ModLoader.IO;
using Humanizer;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : ModItem
    {
        public byte Upgrades;
        public BitsByte ByteHelper;
        private string CurrentOverclock;
        private string OverclockPositives;
        private string OverclockNegatives;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("M1000 Classic");
            Tooltip.SetDefault("'From A to D, skipping B and C!'\n" +
                               "Hold click to fire a focused shot");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 50;
            Item.height = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f;
            Item.rare = ItemRarityID.Pink;
            Item.scale = .7f;
            Item.value = 640000;
            Item.consumable = false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override void RightClick(Player player)
        {
            ByteHelper = Upgrades;
            Item.stack += 1;
            if (player.HeldItem.type == ModContent.ItemType<Overclocks.HipsterOC>())
            {
                Main.mouseItem.stack -= 1;
                Main.mouseItem.maxStack = 0;
                ByteHelper[6] = true;
                ByteHelper[7] = false;
            }
            else if (player.HeldItem.type == ModContent.ItemType<Overclocks.DiggingRoundsOC>())
            {
                Main.mouseItem.stack -= 1;
                Main.mouseItem.maxStack = 0;
                ByteHelper[6] = true;
                ByteHelper[7] = true;
            }
            else if (player.HeldItem.type == ModContent.ItemType<Overclocks.SupercoolOC>())
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
                Item.channel = false;
                Item.useTime = 9;
                Item.useAnimation = 9;
                CurrentOverclock = "Hipster";
                OverclockPositives = "▶Increased fire rate for normal shots";
                OverclockNegatives = "▶You can no longer fire focus shots";
            }
            else if (ByteHelper[6] && ByteHelper[7])
            {
                Item.channel = true;
                Item.useTime = 15;
                Item.useAnimation = 15;
                CurrentOverclock = "Digging Rounds";
                OverclockPositives = "▶Focus shots pierce through blocks";
                OverclockNegatives = "";
            }
            else if (!ByteHelper[6] && ByteHelper[7])
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
                CurrentOverclock = "Supercooling Chamber";
                OverclockPositives = "▶Focus shots deal 2x more damage";
                OverclockNegatives = "▶Focus shots take 33% longer to charge\n" +
                                     "▶Decreased fire rate for normal shots";
            }
            else
            {
                Item.channel = true;
                Item.useTime = 15;
                Item.useAnimation = 15;
            }
            Upgrades = ByteHelper;
        }
    }
}