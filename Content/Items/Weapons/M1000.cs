using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static System.Math;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Projectiles.M1000Projectile;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : UpgradeableItemTemplate
    {
        public int oldFireRate = 0;
        public int newFireRate = 0;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SafeDefaults()
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
            Item.scale = .5f;
            Item.value = 640000;
            Item.consumable = false;

            oldFireRate = Item.useTime;

            ValidUpgrades.Add(ModContent.ItemType<HipsterOC>());
            ValidUpgrades.Add(ModContent.ItemType<DiggingRoundsOC>());
            ValidUpgrades.Add(ModContent.ItemType<SupercoolOC>());

            ValidUpgrades.Add(ModContent.ItemType<QuickCharge>());
            ValidUpgrades.Add(ModContent.ItemType<BumpFire>());


            
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override void IndividualUpgrades()
        {
            if (Overclock == ModContent.ItemType<HipsterOC>())
            {
                Item.channel = false;
                newFireRate = 15;
                CurrentOverclock = "Hipster";
                OverclockPositives = "▶Increased fire rate for normal shots";
                OverclockNegatives = "▶You can no longer fire focus shots";
            }
            else if (Overclock == ModContent.ItemType<DiggingRoundsOC>())
            {
                Item.channel = true;
                newFireRate = 15;
                CurrentOverclock = "Digging Rounds";
                OverclockPositives = "▶Focus shots pierce through blocks";
                OverclockNegatives = "";
            }
            else if (Overclock == ModContent.ItemType<SupercoolOC>())
            {
                Item.channel = true;
                newFireRate = 20;
                CurrentOverclock = "Supercooling Chamber";
                OverclockPositives = "▶Focus shots deal 2x more damage";
                OverclockNegatives = "▶Focus shots take 33% longer to charge\n" +
                                     "▶Decreased fire rate for normal shots";
            }
            else
            {
                Item.channel = true;
                newFireRate = 15;
            }
            foreach (int i in Upgrades2)
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
    }
}