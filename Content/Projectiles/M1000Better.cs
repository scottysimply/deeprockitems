using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static deeprockitems.MyFunctions;
using Terraria.Audio;
using static System.Math;
using static Humanizer.In;

namespace deeprockitems.Content.Projectiles
{
    public class M1000Better : ModProjectile
    {
        private bool RUN_ONCE_1 = true;
        private bool RUN_ONCE_2 = true;
        private const float MAX_CHARGE = 40f;
        
        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public bool IsFullyCharged => Charge == MAX_CHARGE;
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }
        public override bool? CanDamage()
        {
            if (Main.player[Projectile.owner].channel)
            {
                return false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.height, Projectile.width);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.channel) // for when the player has stopped charging
            {
                Projectile.extraUpdates = 7;
                Projectile.hide = false;
                if (RUN_ONCE_2)
                {
                    SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/M1000Fire") with { Volume = .5f }, owner.position);
                    RUN_ONCE_2 = false;
                }
                return;
            }
            else
            {
                WeaponCharge(owner);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (int)Floor(Projectile.damage *.7);
        }
        public void WeaponCharge(Player player)
        {
            Projectile.timeLeft = 300;
            Projectile.position = player.Center;
            //Projectile.rotation = ;
            RenderItem(player, Main.MouseWorld - player.Center, Main.MouseWorld);
            if (!IsFullyCharged)
            {
                Charge++;
            }
        }
        public void ChargedStats(Projectile Projectile, Player player) // changing stats when the projectile is fully charged
        {
            if (RUN_ONCE_1)
            {
                SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Items/M1000Focus") with { Volume = .6f }, player.position);
                Projectile.damage *= 3;
                Projectile.penetrate = 5;
                RUN_ONCE_1 = false;
            }
        }
    }
}