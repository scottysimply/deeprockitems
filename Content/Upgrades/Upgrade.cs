using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace deeprockitems.Content.Upgrades
{
    public class Upgrade {
        public Upgrade(string weaponName, string upgradeName, Asset<Texture2D> sprite) {
            UpgradeName = upgradeName;
            WeaponName = weaponName;
            Texture = sprite;
            Behavior = new();
            Recipe = new();
            UpgradeState = new UpgradeStateBinding() {
                IsEquipped = false,
                IsUnlocked = false,
            };
        }
        public readonly string WeaponName;
        public readonly string UpgradeName;
        public Asset<Texture2D> Texture { get; set; }
        public virtual Asset<Texture2D> Background { get => Assets.UI.UpgradeSlot; }
        public string LocalizedKey { get; set; }
        public LocalizedText DisplayName { get => Language.GetOrRegister($"{LocalizedKey}.DisplayName", () => UpgradeName); }
        public LocalizedText HoverText { get => Language.GetOrRegister($"{LocalizedKey}.HoverText", () => "Hover text"); }
        public UpgradeStateBinding UpgradeState { get; set; }
        public UpgradeBehavior Behavior { get; set; }
        public UpgradeRecipe Recipe { get; set; }
        public UpgradeTier Tier { get; set; }
    }
}
