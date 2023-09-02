using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static System.Math;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Projectiles.M1000Projectile;
using deeprockitems.Utilities;
using Terraria.DataStructures;
using deeprockitems.Content.Projectiles;
using System;
using Microsoft.CodeAnalysis;
using deeprockitems.Content.Items.Upgrades.Overclocks.M1000;

namespace deeprockitems.Content.Items.Weapons
{
    public class M1000 : UpgradeableItemTemplate
    {
        public int oldFireRate = 0;
        public int newFireRate = 0;
        private int original_projectile;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SafeDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 7.75f;
            Item.crit = 17;
            Item.width = 60;
            Item.height = 12;
            Item.useAmmo = AmmoID.Bullet;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f;
            Item.rare = ItemRarityID.Pink;
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
            // Store the projectile that would've been shot.
            original_projectile = type;

            // Set type to be the "helper" projectile.
            type = ModContent.ProjectileType<M1000Helper>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            if (proj.ModProjectile is HeldProjectileBase modProj)
            {
                // Make the helper spawn the original projectile when it despawns/dies, to make it look like the original projectile was shot.
                modProj.ProjectileToSpawn = original_projectile;
                // Replace musket balls with high-velocity bullet
                if (original_projectile == ProjectileID.Bullet)
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }

                // Sorry, until this weird gravity issue gets fixed: No modded bullets!
                if (!ModInformation.IsProjectileVanilla(original_projectile) && !ModInformation.IsProjectileMyMod(original_projectile))
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }
            }
            return false;
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
            if (Upgrades.Contains(ModContent.ItemType<BumpFire>()))
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