using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;

namespace deeprockitems.Content.Projectiles
{
    public class SludgeBall : ModProjectile
    {
        SludgePump parentItem;
        float GooTimer = 5f;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = 0;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            
        }
        public override void OnSpawn(IEntitySource source)
        {
            parentItem = Main.player[Projectile.owner].HeldItem.ModItem as SludgePump;
        }
        public override void AI()
        {
            if (parentItem == null) return;

            if (((BitsByte)parentItem.Upgrades)[2] && Projectile.ai[0] == SludgeHelper.MAX_CHARGE) // Drop goo from the projectile if goo bomber is equipped
            {
                if (GooTimer > 0)
                {
                    GooTimer--;
                }
                else
                {
                    GooTimer = 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * .8f), Projectile.knockBack, Main.myPlayer);
                }
            }


            if (!((BitsByte)parentItem.Upgrades)[0]) // If nograv is not equipped:
            {
                if (Projectile.velocity.Y <= 30f) // Set gravity cap
                {
                    Projectile.velocity.Y += .5f;
                }
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

            if (parentItem == null) return;
            if (((BitsByte)parentItem.Upgrades)[2]) return;
            if (((BitsByte)parentItem.Upgrades)[1])
            {
                if (Projectile.ai[0] == SludgeHelper.MAX_CHARGE)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)Floor(Projectile.damage * .8), Projectile.knockBack, Projectile.owner);
                }
                return;
            }


            if (Projectile.ai[0] == SludgeHelper.MAX_CHARGE && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * .5), Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
    public class SludgeFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y <= 30f) // Set gravity cap
            {
                Projectile.velocity.Y += .5f;
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
    public class SludgeExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 72;
            Projectile.scale = 2f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            DrawOffsetX = Projectile.height / 2;
            DrawOriginOffsetY = Projectile.height / 2;

        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 150);
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Main.MouseWorld - owner.Center) * SHOOT_SPEED, ModContent.ProjectileType<SludgeBall>(), Projectile.damage, Projectile.knockBack, Projectile.owner, projTimer);
                }
                Projectile.Kill();
            }
        }
    }
}