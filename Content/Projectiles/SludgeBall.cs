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
        private const float MAX_CHARGE = 40f; // This is the time it takes to charge the weapon, in ticks. Due to being a constant, it is in all caps for readability
        private readonly float SHOOT_SPEED = Main.player[Main.myPlayer].HeldItem.shootSpeed;
        private bool fired = false;
        private bool one_time = false;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.rotation = Main.rand.NextFloat(0f, 6.28f);
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;

            // these are defaults that are used while the projectile is charging. these will change in the code
            Projectile.hide = true;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {
            if (fired)
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawOffsetX = -16;
            return true;
        }
        public override void AI()
        {
            if (!fired)
            {
                if (Charging(Main.player[Projectile.owner])) // If this returns false, the player has stopped charging the weapon. Therefore, we can update the projectile's stats.
                {
                    return; // Halt AI here, the player is still charging.
                }
                fired = true;
                FiredStats(Main.player[Projectile.owner]);
            }
            if (Projectile.velocity.Y != 10f) // cap gravity
            {
                Projectile.velocity.Y += .5f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] == MAX_CHARGE)
            {
                target.AddBuff(BuffID.Venom, 600);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 150);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            if (!fired)
            {
                return false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (fired)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpHit") with { Volume = .3f }, Projectile.position);
            }
            if (Projectile.ai[0] == MAX_CHARGE)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor((float)Projectile.damage * .3), Projectile.knockBack, Main.myPlayer);
                }
            }
        }
        public bool Charging(Player player)
        {
            RenderItem(player);
            if (player.channel) // projectile owner is still channeling the weapon
            {
                Projectile.timeLeft = 300;
                Projectile.Center = player.Center;
                if (Projectile.ai[0] < MAX_CHARGE)
                {
                    Projectile.ai[0]++;
                }
                else if (!one_time) // This is a one time flag
                {
                    ChargedStats(player);
                    one_time = true; // Flag will never run again.
                }
                return true;
            }
            return false;
        }
        public void FiredStats(Player player) // This changes stats for when the player stops channeling (to clean up the code)
        {
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.rotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            Projectile.velocity = player.DirectionTo(Main.MouseWorld) * SHOOT_SPEED; // Makes the projectile shoot toward the mouse.
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpFire") with { Volume = .5f }, player.Center);
        }
        public void ChargedStats(Player player)
        {
            Projectile.damage *= 2;
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Items/SludgePumpFocus") with { Volume = .8f }, player.Center); // This is the sound effect to indicate the weapon has charged.
        }
        public static void RenderItem(Player player) // This will make the player continue to hold out the item while the weapon is charging. It will also aim toward the cursor.
        {
            float myRotation;
            if (player.itemTime == 1)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            if (Main.MouseWorld.X < player.Center.X) // Mouse is to the left of the player
            {
                if (Main.MouseWorld.Y < player.Center.Y) // Mouse is above the player
                {
                    myRotation = player.DirectionTo(Main.MouseWorld).ToRotation() + (float)PI; // For some reason, the rotation of the player's held item and the mouse position is not the same. I have no clue why. This fixes it.
                }
                else
                {
                    myRotation = player.DirectionTo(Main.MouseWorld).ToRotation() - (float)PI;
                }
                player.direction = -1; // Set player to face left.
            }
            else // Cursor is to the right of the player
            {
                myRotation = player.DirectionTo(Main.MouseWorld).ToRotation(); // Thing is, the player's item rotation and mouse position *are* the same while facing to the right. This game is weird.
                player.direction = 1; // Set player to face right.
            }
            player.itemRotation = myRotation;
        }
    }
}