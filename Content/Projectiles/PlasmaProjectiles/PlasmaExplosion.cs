using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Upgrades.PlasmaPistol;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;

namespace deeprockitems.Content.Projectiles.PlasmaProjectiles
{
    public class PlasmaExplosion : ModProjectile
    {
        private int[] upgrades = new int[4];
        private bool? canDamage = null;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 240;
            Projectile.frame = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 3 == 0 && Projectile.frame < 2)
            {
                Projectile.frame++;
            }
            if (Projectile.frameCounter > 10)
            {
                Projectile.Kill();
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: UpgradeableItemTemplate parentItem })
            {
                upgrades = parentItem.Upgrades;
            }
            if (upgrades.Contains(ModContent.ItemType<MountainMoverOC>()))
            {
                canDamage = false;
                if (Projectile.owner == Main.myPlayer)
                {

                    int explosionRadius = (int)(Projectile.height / 32f);
                    int minTileX = (int)(Projectile.Center.X / 16f - explosionRadius);
                    int maxTileX = (int)(Projectile.Center.X / 16f + explosionRadius);
                    int minTileY = (int)(Projectile.Center.Y / 16f - explosionRadius);
                    int maxTileY = (int)(Projectile.Center.Y / 16f + explosionRadius);

                    // Ensure that all tile coordinates are within the world bounds
                    Utils.ClampWithinWorld(ref minTileX, ref minTileY, ref maxTileX, ref maxTileY);

                    // These 2 methods handle actually mining the tiles and walls while honoring tile explosion conditions
                    bool explodeWalls = Projectile.ShouldWallExplode(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY);
                    Projectile.ExplodeTiles(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY, explodeWalls);
                }
            }
        }
        public override bool? CanDamage()
        {
            return canDamage;
        }
    }
}
