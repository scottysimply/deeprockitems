using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Content.Items.Upgrades.PlasmaPistol;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class BigPlasma : ModProjectile // Darn big plasma.. and their exploding!
    {
        private int[] upgrades = new int[4];
        private UpgradeableItemTemplate parentItem = null;
        private bool piercingPlasma = false;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            if (piercingPlasma)
            {
                return;
            }
            Projectile.rotation = 0;
            Projectile collidingProjectile = Projectile.IsCollidingWithProjectile(ModContent.ProjectileType<PlasmaBullet>());

            if (collidingProjectile is not null)
            {
                if (Projectile.owner == collidingProjectile.owner)
                {
                    collidingProjectile.Kill();
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f }); // Sound of the projectile 
                    Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_ItemUse(parentItem.Item), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage, .1f);
                    Projectile.Kill();
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate parentItem })
            {
                upgrades = parentItem.Upgrades;
                this.parentItem = parentItem;
            }
            if (upgrades.Contains(ModContent.ItemType<PiercingPlasmaOC>()))
            {
                Projectile.tileCollide = false;
                Projectile.penetrate = 5;
                Projectile.maxPenetrate = 5;
                piercingPlasma = true;

            }
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                if (upgrades.Contains(ModContent.ItemType<EzBoomOC>()))
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = .5f, Pitch = -.8f });
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage, .1f);
                }
            }
        }

    }
}
