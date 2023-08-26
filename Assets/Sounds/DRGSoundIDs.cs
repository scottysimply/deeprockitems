﻿using Terraria.Audio;

namespace deeprockitems.Assets.Sounds
{
    public static class DRGSoundIDs
    {
        public static SoundStyle M1000Focus { get; private set; }
        public static SoundStyle M1000Fire { get; private set; }
        public static SoundStyle SludgePumpFocus { get; private set; }
        public static SoundStyle SludgePumpFire { get; private set; }
        public static SoundStyle SludgeBallHit { get; private set; }
        

        public static void InitializeSounds()
        {
            string dir = "deeprockitems/Assets/Sounds/";
            M1000Focus = new SoundStyle(dir + "Items/M1000Focus");
            M1000Fire = new SoundStyle(dir + "Items/M1000Fire");
            SludgePumpFocus = new SoundStyle(dir + "Items/SludgePumpFocus");
            SludgePumpFire = new SoundStyle(dir + "Items/SludgePumpFire");
            SludgeBallHit = new SoundStyle(dir + "Projectiles/SludgeBallHit");

        }
    }

}
