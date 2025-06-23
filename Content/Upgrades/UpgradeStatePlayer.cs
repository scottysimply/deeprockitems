using deeprockitems.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace deeprockitems.Content.Upgrades
{
    public class UpgradeStatePlayer : ModPlayer
    {
        public class PlayerUpgrades : Dictionary<string, WeaponUpgradeState>, TagSerializable {
            public TagCompound SerializeData() {
                TagCompound data = new();
                data["Weapons"] = new Dictionary<string, object>();
                foreach ((string weapon, WeaponUpgradeState upgrades) in this)
                {
                    (data["Weapons"] as Dictionary<string, object>)[weapon] = upgrades;
                }
                return data;
            }
            public static PlayerUpgrades Load(TagCompound tag) {
                var data = tag.Get<PlayerUpgrades>("Weapons");
                return data;
            }
            public static Func<TagCompound, PlayerUpgrades> SERIALIZER = Load;
        }
        public PlayerUpgrades MasterPlayerUpgrades { get; set; }
        public override void SaveData(TagCompound tag) {
            tag["DrgUpgrades"] = MasterPlayerUpgrades;
        }
        public override void LoadData(TagCompound tag) {
            // Failsafe if the player contains old upgrade data
            var existingUpgrades = new PlayerUpgrades();
            bool hasOldUpgrades = false;
            foreach (var item in Player.inventory)
            {
                if (item.ModItem is UpgradableWeapon weapon)
                {
                    foreach (var (key, value) in weapon.UpgradeMasterList) {
                        if (value)
                    }
                }
            }

            var loadedNewUpgrades = tag.Get<PlayerUpgrades>("DrgUpgrades");
            if (hasOldUpgrades)
            {

            }
            else
            {

            }
        }
    }
}
