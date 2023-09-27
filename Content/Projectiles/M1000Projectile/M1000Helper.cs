using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Assets.Sounds;

namespace deeprockitems.Content.Projectiles.M1000Projectile
{
    public class M1000Helper : HeldProjectileBase
    {
        public override float CHARGE_TIME { get; set; } = 37.5f;
        public override SoundStyle Charge_Sound => DRGSoundIDs.M1000Focus;
        public override SoundStyle Fire_Sound => DRGSoundIDs.M1000Fire;

        public M1000Helper()
        {
            ProjectileToSpawn = ProjectileID.BulletHighVelocity;
        }

        public override void AtFullCharge()
        {
            Projectile.velocity *= 2f;
            Projectile.damage *= 2;
        }
    }
}