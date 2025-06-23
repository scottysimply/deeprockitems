using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace deeprockitems.Content.Upgrades
{
    public class WeaponUpgradeState : Dictionary<string, UpgradeStateBinding>, TagSerializable
    {
        public TagCompound SerializeData() {
            TagCompound data = new();
            data["Upgrades"] = new Dictionary<string, object>();
            foreach ((string tier, UpgradeStateBinding entries) in this)
            {
                (data["Upgrades"] as Dictionary<string, object>)[tier] = entries;
            }
            return data;
        }
        public static WeaponUpgradeState Load(TagCompound tag) {
            var data = tag.Get<WeaponUpgradeState>("Upgrades");
            return data;
        }
        public static Func<TagCompound, WeaponUpgradeState> DESERIALIZER = Load;
    }
}
