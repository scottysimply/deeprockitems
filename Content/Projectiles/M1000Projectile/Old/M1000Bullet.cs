/*using Terraria;
using Terraria.ModLoader;
using static System.Math;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Upgrades;
using deeprockitems.Content.Items.Weapons;
using System.Linq;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Bullet : ModProjectile
    {
        public UpgradeableItemTemplate modItem;
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
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate weaponSource } )
            {
                if (weaponSource.Upgrades.Contains(ModContent.ItemType<SupercoolOC>()))
                {
                    Projectile.damage *= 3;
                }
                if (weaponSource.Upgrades.Contains(ModContent.ItemType<Blowthrough>()))
                {
                    Projectile.penetrate = 5;
                }
                modItem = weaponSource;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (modItem.Upgrades.Contains(ModContent.ItemType<Blowthrough>()))
            {
                Projectile.damage = (int)(Projectile.damage * .5f);
                Projectile.penetrate -= 1;
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
*/