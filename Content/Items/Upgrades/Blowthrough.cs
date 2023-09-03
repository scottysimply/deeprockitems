﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades
{
    public class Blowthrough : UpgradeTemplate
    {
        public override string ItemName { get => "Blowthrough Rounds"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Shots pierce enemies"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => false; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
    }
}
