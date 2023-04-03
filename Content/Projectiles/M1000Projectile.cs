using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.IO;
using static Humanizer.In;

namespace deeprockitems.Content.Projectiles
{
    public class M1000Bullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 7;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOriginOffsetX = -40;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, .5f, .45f, .05f);
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.damage *= 3;
                Projectile.penetrate = 5;
                if (Main.player[Projectile.owner].HeldItem.ModItem is Items.Weapons.M1000 modItem)
                {
                    BitsByte helper = modItem.Upgrades;
                    if (helper[6] && helper[7])
                    {
                        Projectile.tileCollide = false;
                    }
                    else if (!helper[6] && helper[7])
                    {
                        Projectile.damage *= 2;
                    }
                }
            }
            Projectile.rotation = Projectile.ai[1];
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)Floor(Projectile.damage * .7f);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Projectile.damage = (int)Floor(Projectile.damage * .7f);
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
    public class M1000Helper : ModProjectile
    {
        public float MAX_CHARGE = 40f; // This is the time it takes to charge the weapon, in ticks. This is mostly constant.
        public float projTimer = 0f;
        private bool charged = false;
        private float projRotation;
        private Vector2 projDirection;
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
            projDirection = Vector2.Normalize(Main.MouseWorld - Main.player[Projectile.owner].Center);
            projRotation = Main.player[Projectile.owner].Center.DirectionTo(Main.MouseWorld).ToRotation();
            if (Main.player[Projectile.owner].HeldItem.ModItem is Items.Weapons.M1000 modItem)
            {
                BitsByte helper = modItem.Upgrades;
                if (!helper[6] && helper[7])
                {
                    MAX_CHARGE = 60f;
                }
            }
        }
        public override string GlowTexture => "deeprockitems/Content/Projectiles/M1000Projectile";
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projDirection * SHOOT_SPEED, ModContent.ProjectileType<M1000Bullet>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, System.Convert.ToSingle(charged), projRotation);
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