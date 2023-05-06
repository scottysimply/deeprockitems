using Terraria;
using Terraria.ModLoader;
using static System.Math;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Bullet : ModProjectile
    {
        public Items.Weapons.M1000 modItem;
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
            modItem = Main.player[Projectile.owner].HeldItem.ModItem as Items.Weapons.M1000;
            if (Projectile.ai[0] == 1)
            {
                Projectile.damage *= 3;
                if (modItem is not null)
                {
                    if (modItem.Upgrades[1])
                    {
                        Projectile.tileCollide = false;
                    }
                    else if (modItem.Upgrades[2])
                    {
                        Projectile.damage *= 2;
                    }
                }
            }
            if (modItem is not null)
            {
                if (modItem.Upgrades[7])
                {
                    Projectile.penetrate = 5;
                }
            }
            Projectile.rotation = Projectile.ai[1];
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (modItem.Upgrades[7])
            {
                Projectile.damage = (int)Floor(Projectile.damage * .7f);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (modItem.Upgrades[7])
            {
                Projectile.damage = (int)Floor(Projectile.damage * .7f);
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
