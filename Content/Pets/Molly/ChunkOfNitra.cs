using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Pets.Molly
{
    public class ChunkOfNitra : ModItem
    {
        // Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SpiderEgg); // Copy the Defaults of the Zephyr Fish Item.
            Item.width = 24;
            Item.height = 24;

            Item.shoot = ModContent.ProjectileType<MollyPetProjectile>(); // "Shoot" your pet projectile.
            Item.buffType = ModContent.BuffType<MollyPetBuff>(); // Apply buff upon usage of the Item.
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }

        
    }
}