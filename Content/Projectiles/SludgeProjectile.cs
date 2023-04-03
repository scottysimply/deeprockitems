using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;

namespace deeprockitems.Content.Projectiles
{
    public class SludgeBall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = 0;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y != 10f) // Set gravity cap (we don't want
            {
                Projectile.velocity.Y += .5f;
            }
            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == SludgeHelper.MAX_CHARGE)
            {
                target.AddBuff(BuffID.Venom, 300);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 150);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Projectile.ai[0] == SludgeHelper.MAX_CHARGE)
            {
                target.AddBuff(BuffID.Venom, 150);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 75);
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpHit") with { Volume = .3f }, Projectile.position);
            if (Projectile.ai[0] == SludgeHelper.MAX_CHARGE && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor((float)Projectile.damage * .4), Projectile.knockBack, Main.myPlayer);
                }
            }
        }
    }
    public class SludgeFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.velocity.Y != 10f)
                {
                    Projectile.velocity.Y += .5f;
                }
            }
            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpHit") with { Volume = .2f }, Projectile.position);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 60);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 30);
        }
    }
    public class SludgeHelper : ModProjectile
    {
        public const float MAX_CHARGE = 50f; // This is the time it takes to charge the weapon, in ticks. Due to being a constant, it is in all caps for readability
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Main.MouseWorld - owner.Center) * SHOOT_SPEED, ModContent.ProjectileType<SludgeBall>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, projTimer);
                }
                Projectile.Kill();
            }
        }
    }
}