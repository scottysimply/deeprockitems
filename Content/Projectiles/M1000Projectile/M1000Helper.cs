using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Upgrades;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Helper : ModProjectile
    {
        public float MAX_CHARGE = 40f; // This is the time it takes to charge the weapon, in ticks.
        public float projTimer = 0f;
        private bool charged = false;
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
            if (Main.player[Projectile.owner].HeldItem.ModItem is Items.Weapons.M1000 modItem)
            {
                foreach (int i in modItem.Upgrades)
                {
                    if (i == ModContent.ItemType<SupercoolOC>())
                    {
                        MAX_CHARGE = (float)(MAX_CHARGE * 1.33);
                    }
                    if (i == ModContent.ItemType<QuickCharge>())
                    {
                        MAX_CHARGE *= .75f;
                    }
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
                    Projectile.NewProjectile(entitysource, Projectile.Center, Vector2.Normalize(Main.MouseWorld - owner.Center) * SHOOT_SPEED, ModContent.ProjectileType<M1000Bullet>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, System.Convert.ToSingle(charged));
                }
                Projectile.Kill();
            }
        }

        

        // So what's going on in this method? Deconstruction time!
        private void HoldItemOut(Player player)
        {
            // So fun fact about the way the game handles rotation: values go from -Pi to +Pi. There is no 0 to 2Pi.
            // For some god awful reason though, itemRotation doesn't match DirectionTo().ToRotation() of the mouse, but only when the itemRotation is in quadrant 4.

            // If cursor is to the right of the player
            if (Main.MouseWorld.X > player.Center.X)
            {
                // See, this is easy!
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
                player.direction = 1; // Make the player face right
                return;
            }
            // If cursor is above the player
            if (Main.MouseWorld.Y < player.Center.Y)
            {
                // Here's where it messes up. If the cursor is in quadrant II, it needs to add PI
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
            }
            // If cursor is below the player
            else
            {
                // But if the cursor is in Quadrant III, it has to subtract. guh??
                player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - (float)PI;
            }
            player.direction = -1; // Make the player face left
        }
    }
}