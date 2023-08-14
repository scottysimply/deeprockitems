using Terraria;
using Terraria.ModLoader;
using static System.Math;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Upgrades;

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
            DrawOriginOffsetX = -38;
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
                    if (modItem.Overclock == ModContent.ItemType<DiggingRoundsOC>())
                    {
                        Projectile.tileCollide = false;
                    }
                    else if (modItem.Overclock == ModContent.ItemType<SupercoolOC>())
                    {
                        Projectile.damage *= 2;
                    }
                }
            }
            if (modItem is not null)
            {
                foreach (int i in modItem.Upgrades2)
                {
                    if (i == ModContent.ItemType<Blowthrough>())
                    {
                        Projectile.penetrate = 5;
                    }
                }
            }
            Projectile.rotation = new Vector2(0, 0).DirectionTo(-Projectile.velocity).ToRotation();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (int i in modItem.Upgrades2)
            {
                if (i == ModContent.ItemType<Blowthrough>())
                {
                    Projectile.damage = (int)Floor(Projectile.damage * .7f);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
