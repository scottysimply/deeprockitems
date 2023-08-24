using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Weapons
{
    public class PlasmaPistol : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.useTime = 5;
            Item.useAnimation = 5;
        }
    }
}