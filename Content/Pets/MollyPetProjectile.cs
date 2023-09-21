using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Pets
{
    public class MollyPetProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabySpider);
            Projectile.width = 
        }
    }
}
