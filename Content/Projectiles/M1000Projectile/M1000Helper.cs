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
        public override float ChargeTime { get; set; } = 37.5f;
        public override int ProjectileToSpawn { get; set; } = ProjectileID.BulletHighVelocity;
        public override SoundStyle? ChargeSound => DRGSoundIDs.M1000Focus;
        public override SoundStyle? FireSound => DRGSoundIDs.M1000Fire;

        public override void WhenReachedFullCharge()
        {
            Projectile.damage *= 2;
        }
    }
}