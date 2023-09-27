using deeprockitems.Content.Items.Upgrades;
using deeprockitems.UI.UpgradeItem;
using deeprockitems.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using static System.Math;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradeableItemTemplate : ModItem
    {
        protected float DamageScale;
        protected float useTimeModifier;
        /// <summary>
        /// The item useTime before being affected by useTimeModifier.
        /// </summary>
        protected int oldUseTime { get; set; }
        private int[] saved_upgrades;
        public int[] Upgrades { get; set; }
        public int Overclock { get => Upgrades[^1]; }
        public virtual string CurrentOverclock { get; set; } = "";
        public virtual string OverclockPositives { get; set; } = "";
        public virtual string OverclockNegatives { get; set; } = "";
        public List<int> ValidUpgrades { get; set; }
        // private bool load_flag = false;
        public virtual void SafeDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            saved_upgrades = new int[4];
            Upgrades = new int[4];
            useTimeModifier = 1f;
            ValidUpgrades = new()
            {
                ModContent.ItemType<DamageUpgrade>(),
                ModContent.ItemType<ArmorPierce>(),
                ModContent.ItemType<Blowthrough>()
            
            };
            SafeDefaults();
            oldUseTime = Item.useTime;
            base.SetDefaults();
        }
        public override void UpdateInventory(Player player)
        {
            if (!Main.playerInventory)
            {
                Close_UI();
            }
            UpdateUpgrades();
        }

        public override void RightClick(Player player)
        {
            Item.stack += 1;
            if (UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay != Item)
            {
                Open_UI();
            }
            else
            {
                Close_UI();
            }
            UpdateUpgrades();
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void SaveData(TagCompound tag)
        {
            for (int upgrade_index = 0; upgrade_index < Upgrades.Length; upgrade_index++)
            {
                if (Upgrades[upgrade_index] == 0)
                {
                    saved_upgrades[upgrade_index] = -1;
                    continue;
                }
                for (int valid_index = 0; valid_index < ValidUpgrades.Count; valid_index++)
                {
                    if (ValidUpgrades[valid_index] == Upgrades[upgrade_index])
                    {
                        saved_upgrades[upgrade_index] = valid_index;
                    }
                }
            }
            tag["Upgrades"] = saved_upgrades;
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("Upgrades"))
            {
                saved_upgrades = (int[])tag["Upgrades"];
                for (int upgrade_index = 0; upgrade_index < saved_upgrades.Length; upgrade_index++)
                {
                    if (saved_upgrades[upgrade_index] == -1)
                    {
                        Upgrades[upgrade_index] = 0;
                        continue;
                    }
                    for (int valid_index = 0; valid_index < ValidUpgrades.Count; valid_index++)
                    {
                        if (saved_upgrades[upgrade_index] == valid_index)
                        {
                            Upgrades[upgrade_index] = ValidUpgrades[valid_index];
                        }
                    }
                }
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ItemSlot.ShiftInUse)
            {
                string upgrades_text = "";
                for (int u = 0; u < Upgrades.Length; u++)
                {
                    UpgradeTemplate upgrade_item = new Item(Upgrades[u]).ModItem as UpgradeTemplate;
                    if (upgrade_item is null)
                    {
                        continue;
                    }
                    if (Upgrades[u] == Overclock && Overclock != 0) // If this is the overclock && overclock exists
                    {
                        upgrades_text = upgrades_text + Language.GetTextValue("Mods.deeprockitems.Misc.DefaultOverclockText", upgrade_item.DisplayName) + "\n";
                        upgrades_text = upgrades_text + upgrade_item.GetLocalizedValue("Positives") + "\n";
                        upgrades_text = upgrades_text + upgrade_item.GetLocalizedValue("Negatives") + "\n";
                        break;
                    }
                    if (Upgrades[u] != 0)
                    {
                        upgrades_text = upgrades_text + string.Format("[c/23DF24:▲{0}]", upgrade_item.Tooltip) + "\n";
                    }
                }
                upgrades_text = upgrades_text.TrimEnd('\n');
                TooltipLine line = new(Mod, "Upgrades", upgrades_text);
                tooltips.Add(line);
            }
            else if (Upgrades.Count(0) < 4)
            {
                TooltipLine line = new(Mod, "ShiftToView", "Hold shift to view upgrades")
                {
                    OverrideColor = new(74, 177, 211),
                };
                tooltips.Add(line);
            }

        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public void UpdateUpgrades()
        {
            DamageScale = 1f;
            useTimeModifier = 1f;
            UniqueUpgrades();

            if (Upgrades.Contains(ModContent.ItemType<DamageUpgrade>()))
            {
                Item.damage = (int)Floor(Item.OriginalDamage * DamageScale) + 5;
            }
            else
            {
                Item.damage = (int)Floor(Item.OriginalDamage * DamageScale);
            }
            if (Upgrades.Contains(ModContent.ItemType<BumpFire>()))
            {
                useTimeModifier *= .83f;
            }
            Item.useTime = Item.useAnimation = (int)Ceiling(oldUseTime * useTimeModifier);
        }
        public virtual void UniqueUpgrades()
        {
            
        }
        private void Open_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(UpgradeUISystem.UpgradeUIPanel);
            UpgradeUISystem.UpgradeUIPanel.ParentSlot.ItemToDisplay = Item;
            UpgradeUISystem.UpgradeUIPanel.ShowItems();
        }
        private void Close_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(null);
        }
    }
}
