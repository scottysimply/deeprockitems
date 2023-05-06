using deeprockitems.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static System.Math;
using deeprockitems.Content.Items.Upgrades;
using Terraria.Utilities;

namespace deeprockitems.Content.Items.Weapons
{
    public abstract class UpgradeableItemTemplate : ModItem
    {
        public float DamageScale;
        public virtual BitsByte Upgrades { get; set; }
        public virtual string CurrentOverclock { get; set; } = "";
        public virtual string OverclockPositives { get; set; } = "";
        public virtual string OverclockNegatives { get; set; } = "";
        public virtual int[] ValidUpgrades { get; set; } = new int[8];
        private bool load_flag = false;
        public virtual void SafeDefaults()
        {


        }
        public override void SetDefaults()
        {
            SafeDefaults();

            ValidUpgrades[3] = ModContent.ItemType<DamageUpgrade>();
            ValidUpgrades[5] = ModContent.ItemType<ArmorPierce>();
            ValidUpgrades[7] = ModContent.ItemType<Blowthrough>();

            base.SetDefaults();
        }
        public override void UpdateInventory(Player player)
        {
            if (!Main.playerInventory)
            {
                Close_UI();
            }
        }

        public override void RightClick(Player player)
        {
            Item.stack += 1;
            if (UpgradeUIState.ParentSlot.ItemToDisplay != Item)
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
            tag["WeaponUpgrades"] = (byte)Upgrades;
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("WeaponUpgrades"))
            {
                Upgrades = (byte)tag["WeaponUpgrades"];
            }
            else
            {
                Upgrades = 0;
            }
            if (!load_flag)
            {
                load_flag = true;
                UpdateUpgrades();
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Upgrades[0] || Upgrades[1] || Upgrades[2])
            {
                TooltipLine line = new(Mod, "Upgrades", string.Format("This weapon has the following overclock: [c/4AB1D3:{0}]", CurrentOverclock));
                tooltips.Add(line);
                line = new(Mod, "Positives", OverclockPositives)
                {
                    OverrideColor = new(35, 223, 26),
                };
                tooltips.Add(line);
                if (OverclockNegatives != "")
                {
                    line = new(Mod, "Negatives", OverclockNegatives)
                    {
                        OverrideColor = new(240, 19, 24)
                    };
                    tooltips.Add(line);
                }


            }
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public void UpdateUpgrades()
        {
            DamageScale = 1f;
            IndividualUpgrades();

            if (Upgrades[3])
            {
                Item.damage = (int)Floor(Item.OriginalDamage * DamageScale) + 5;
            }
            else
            {
                Item.damage = (int)Floor(Item.OriginalDamage * DamageScale);
            }





        }
        public virtual void IndividualUpgrades()
        {
            
        }
        private void Open_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(UpgradeUISystem.UpgradeUIPanel);
            UpgradeUIState.ParentSlot.ItemToDisplay = Item;
            UpgradeSlot.ParentItem = this;
            UpgradeUISystem.UpgradeUIPanel.LoadItem_InSlot();
        }
        private void Close_UI()
        {
            UpgradeUISystem.UpgradeUIPanel.ClearItems();
            UpgradeUISystem.Interface.SetState(null);
        }
    }
}
