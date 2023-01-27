using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Net;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;

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
            if (Projectile.ai[0] == M1000Helper.MAX_CHARGE)
            {
                Projectile.damage *= 3;
                Projectile.penetrate = 5;
                Projectile.ai[0] = -1;
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
        public const float MAX_CHARGE = 40f; // This is the time it takes to charge the weapon, in ticks. Due to being a constant, it is in all caps for readability
        public float projTimer = 0f;
        private bool charged = false;
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
                SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/M1000Fire") with { Volume = .4f }, owner.Center);
                if (Main.myPlayer == Projectile.owner)
                {
                    float projRotation = Projectile.rotation = owner.Center.DirectionTo(Main.MouseWorld).ToRotation();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Main.MouseWorld - owner.Center) * SHOOT_SPEED, ModContent.ProjectileType<M1000Bullet>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, projTimer, projRotation);
                }
                Projectile.Kill();
            }
        }
    }
}