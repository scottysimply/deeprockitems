using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Pets.Molly
{
    public class ChunkOfNitra : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SpiderEgg); // Essentially a copy of the spider pet.
            Item.width = 24;
            Item.height = 24;

            Item.value = Item.sellPrice(0, 10, 0, 0);

            Item.shoot = ModContent.ProjectileType<MollyPetProjectile>(); // Spawn molly
            Item.buffType = ModContent.BuffType<MollyPetBuff>(); // Give pet buff
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