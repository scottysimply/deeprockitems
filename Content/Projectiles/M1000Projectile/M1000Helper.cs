using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Helper : ModProjectile
    {
        public float MAX_CHARGE = 40f; // This is the time it takes to charge the weapon, in ticks.
        public float projTimer = 0f;
        private bool charged = false;
        private float projRotation;
        private Vector2 projDirection;
        IEntitySource entitysource;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.hide = true;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            entitysource = Main.player[Projectile.owner].GetSource_ItemUse(Main.player[Projectile.owner].HeldItem);
            projDirection = Vector2.Normalize(Main.MouseWorld - Main.player[Projectile.owner].Center);
            projRotation = Main.player[Projectile.owner].Center.DirectionTo(Main.MouseWorld).ToRotation();
            if (Main.player[Projectile.owner].HeldItem.ModItem is Items.Weapons.M1000 modItem)
            {
                if (modItem.Upgrades[2])
                {
                    MAX_CHARGE = (float)(MAX_CHARGE * 1.33);
                }
                if (modItem.Upgrades[4])
                {
                    MAX_CHARGE = MAX_CHARGE * .75f;
                }
            }
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            float SHOOT_SPEED = Projectile.velocity.Length();
            if (owner.channel)
            {
                if (projTimer < MAX_CHARGE)
                {
                    projTimer++;
                }
                else if (!charged)
                {
                    charged = true;
                    SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/M1000Focus") with { Volume = .5f }, owner.Center);
                }
                // Rest of this section of code is rendering of the held item.
                if (Main.myPlayer == Projectile.owner)
                {
                    if (owner.itemTime == 1)
                    {
                        owner.itemTime = 2;
                        owner.itemAnimation = 2;
                    }
                    HoldItemOut(owner); // This method makes the weapon stay held out in front of the player

                }
            }
            else
            {
                Projectile.position = owner.Center;
                SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/M1000Fire") with { Volume = .4f }, owner.Center);
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(entitysource, Projectile.Center, projDirection * SHOOT_SPEED, ModContent.ProjectileType<M1000Bullet>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, System.Convert.ToSingle(charged), projRotation);
                }
                Projectile.Kill();
            }
        }


        private void HoldItemOut(Player player)
        {
            if (Main.MouseWorld.X < player.Center.X) // cursor is to the left of the player
            {
                if (Main.MouseWorld.Y < player.Center.Y) // Mouse is above the player
                {
                    player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
                }
                else
                {
                    player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
                }
                player.direction = -1; // make player face left
            }
            else // cursor is to the right of the player
            {
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
                player.direction = 1; // make player face right
            }
            projRotation = player.itemRotation;
            projDirection = Vector2.Normalize(Main.MouseWorld - player.Center);
        }
    }
}