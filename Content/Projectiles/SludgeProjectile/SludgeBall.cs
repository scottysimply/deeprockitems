﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;

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
            if (parentItem.Upgrades[2] && Projectile.ai[0] != 1)
            {
                Projectile.damage = (int)Ceiling(Projectile.damage * .8f);
            }
            if (parentItem.Upgrades[7])
            {
                Projectile.penetrate = 5;
            }
        }
        public override void AI()
        {
            if (parentItem == null) return;

            if (parentItem.Upgrades[2] && Projectile.ai[0] == 1) // Drop goo from the projectile if goo bomber is equipped
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


            if (!parentItem.Upgrades[0]) // If nograv is not equipped:
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
            if (Projectile.ai[0] == 1)
            {
                target.AddBuff(BuffID.Venom, 300);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 150);
            }
            if (parentItem.Upgrades[7])
            {
                Projectile.damage = (int)Floor(Projectile.damage * .7f);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Projectile.ai[0] == 1)
            {
                target.AddBuff(BuffID.Venom, 150);
            }
            else
            {
                target.AddBuff(BuffID.Venom, 75);
            }
            if (parentItem.Upgrades[7])
            {
                Projectile.damage = (int)Floor(Projectile.damage * .7f);
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpHit") with { Volume = .3f }, Projectile.position);

            if (parentItem == null) return;
            if (parentItem.Upgrades[2]) return;
            if (parentItem.Upgrades[1])
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SludgeExplosion>(), (int)Floor(Projectile.damage * .8), Projectile.knockBack, Projectile.owner);
                }
                return;
            }


            if (Projectile.ai[0] == 1 && Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * 8f, ModContent.ProjectileType<SludgeFragment>(), (int)Floor(Projectile.damage * .5), Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}