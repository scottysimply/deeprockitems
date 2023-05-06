using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeHelper : ModProjectile
    {
        public float MAX_CHARGE = 50f; // This is the time it takes to charge the weapon, in ticks.
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
            if (Main.player[Projectile.owner].HeldItem.ModItem is SludgePump modItem)
            {
                if (modItem.Upgrades[4])
                {
                    MAX_CHARGE = (float)(MAX_CHARGE * .75f);
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
                    Projectile.damage = (int)Floor(1.5 * Projectile.damage);
                    SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpFocus") with { Volume = .8f }, owner.Center);
                }
                // Rest of this section of code is rendering of the held item.
                if (Main.myPlayer == Projectile.owner)
                {
                    if (owner.itemTime == 1)
                    {
                        owner.itemTime = 2;
                        owner.itemAnimation = 2;
                    }
                    if (Main.MouseWorld.X < owner.Center.X) // cursor is to the left of the player
                    {
                        if (Main.MouseWorld.Y < owner.Center.Y) // Mouse is above the player
                        {
                            owner.itemRotation = owner.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
                        }
                        else
                        {
                            owner.itemRotation = owner.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI;
                        }
                        owner.direction = -1; // make player face left
                    }
                    else // cursor is to the right of the player
                    {
                        owner.itemRotation = owner.DirectionTo(Main.MouseWorld).ToRotation();
                        owner.direction = 1; // make player face right
                    }
                }
            }
            else
            {
                Projectile.position = owner.Center;
                SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpFire") with { Volume = .5f }, owner.Center);
                if (Main.myPlayer == Projectile.owner)
                {
                    float flag = charged ? 1 : 0;
                    Projectile.NewProjectile(entitysource, Projectile.Center, Vector2.Normalize(Main.MouseWorld - owner.Center) * SHOOT_SPEED, ModContent.ProjectileType<SludgeBall>(), Projectile.damage, Projectile.knockBack, Projectile.owner, flag);
                }
                Projectile.Kill();
            }
        }
    }
}