using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;
using deeprockitems.Utilities;
using Microsoft.CodeAnalysis;
using deeprockitems.Content.Items.Upgrades.Overclocks.SludgePump;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
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
            if (parentItem.Upgrades.Contains(ModContent.ItemType<SludgeExplosionOC>()) && Projectile.ai[0] > 900f)
            {
                Projectile.damage = (int)Ceiling(Projectile.damage * .8f);
            }

        }
        public override void AI()
        {
            if (parentItem == null) return;

            if (parentItem.Upgrades.Contains(ModContent.ItemType<GooSpecialOC>()) && Projectile.ai[0] == 1) // Drop goo from the projectile if goo bomber is equipped
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


            if (!(parentItem.Upgrades.Contains(ModContent.ItemType<AntiGravOC>()))) // If nograv is not equipped:
            {
                if (Projectile.velocity.Y <= 30f) // Set gravity cap
                {
                    Projectile.velocity.Y += .5f;
                }
            }

            Projectile.rotation += Projectile.velocity.X / 100;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] <= 900 && Projectile.ai[0] > 0)
            {
                target.AddBuff(BuffID.Venom, 300);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 150);
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                if (Projectile.ai[0] == 1)
                {
                    target.AddBuff(BuffID.Venom, 150);
                }
                else
                {
                    target.AddBuff(BuffID.Venom, 75);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgeBallHit") with { Volume = .3f }, Projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dust.SludgeDust>(), Scale: Main.rand.NextFloat(1.1f, 1.5f));
            }

            if (parentItem == null) return;
            if (parentItem.Upgrades.Contains(ModContent.ItemType<GooSpecialOC>())) return;
            if (parentItem.Upgrades.Contains(ModContent.ItemType<SludgeExplosionOC>()))
            {
                if (Projectile.ai[0] <= 900 && Projectile.ai[0] > 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)Floor(Projectile.damage * .8), Projectile.knockBack, Projectile.owner);
                }
                return;
            }


            if (Projectile.ai[0] <= 900 && Projectile.ai[0] > 0 && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * .5), Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}