using System;
using Terraria.ModLoader.IO;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeStateBinding : TagSerializable
    {
        public bool IsEquipped { get; set; } = false;
        public bool IsUnlocked { get; set; } = false;
        public TagCompound SerializeData() {
            return new TagCompound() {
                ["Equipped"] = IsEquipped,
                ["Unlocked"] = IsUnlocked,
            };
        }
        public static UpgradeStateBinding Load(TagCompound tag) {
            return new UpgradeStateBinding() {
                IsEquipped = tag.GetBool("Equipped"),
                IsUnlocked = tag.GetBool("Unlocked")
            };
        }
        public static Func<TagCompound, UpgradeStateBinding> DESERIALIZER = Load;
    }
}
