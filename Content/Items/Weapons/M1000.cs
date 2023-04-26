using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using deeprockitems.Content.Projectiles;
using deeprockitems.Content.Items.Upgrades;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : UpgradeableItemTemplate
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
            ValidUpgrades[0] = ModContent.ItemType<HipsterOC>();
            ValidUpgrades[1] = ModContent.ItemType<DiggingRoundsOC>();
            ValidUpgrades[2] = ModContent.ItemType<SupercoolOC>();
            ValidUpgrades[3] = ModContent.ItemType<DamageUpgrade>();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override void IndividualUpgrades()
        {
            if (((BitsByte)Upgrades)[0])
            {
                Item.channel = false;
                Item.useTime = 9;
                Item.useAnimation = 9;
                CurrentOverclock = "Hipster";
                OverclockPositives = "▶Increased fire rate for normal shots";
                OverclockNegatives = "▶You can no longer fire focus shots";
            }
            else if (((BitsByte)Upgrades)[1])
            {
                Item.channel = true;
                Item.useTime = 15;
                Item.useAnimation = 15;
                CurrentOverclock = "Digging Rounds";
                OverclockPositives = "▶Focus shots pierce through blocks";
                OverclockNegatives = "";
            }
            else if (((BitsByte)Upgrades)[2])
            {
                Item.channel = true;
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
        }
    }
}