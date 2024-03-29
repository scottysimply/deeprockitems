﻿using Terraria;
using deeprockitems.Content.Items.Upgrades;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;
using static Humanizer.In;
using deeprockitems.Assets.Sounds;
using Terraria.ID;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeHelper : HeldProjectileBase
    {
        public override float CHARGE_TIME { get; set; } = 50f;
        public override SoundStyle Charge_Sound => DRGSoundIDs.SludgePumpFocus with { Volume = .8f, PitchVariance = 1f};
        public override SoundStyle Fire_Sound => DRGSoundIDs.SludgePumpFire with { Volume = .5f, PitchVariance = .75f};

        public SludgeHelper()
        {
            ProjectileToSpawn = ModContent.ProjectileType<SludgeBall>();
        }

        public override void AtFullCharge()
        {
            Projectile.damage = (int)Floor(Projectile.damage * 1.35f);
        }
    }
}