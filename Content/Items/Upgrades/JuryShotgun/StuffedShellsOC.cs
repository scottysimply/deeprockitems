using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace deeprockitems.Content.Items.Upgrades.JuryShotgun
{
    public class StuffedShellsOC : UpgradeTemplate
    {
        public override string ItemName { get => "Stuffed Shells"; set => base.ItemName = value; }
        public override string ItemTooltip { get => "Doubled pellet count, but lower fire rate and heavily increased spread"; set => base.ItemTooltip = value; }
        public override bool IsOverclock { get => true; set => base.IsOverclock = value; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red;
        }
    }
}
