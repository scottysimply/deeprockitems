using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Utilities;
using deeprockitems.Content.Items.Upgrades.SludgePump;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class ProjectileTracer : ModProjectile
    {
        SludgePump parentItem;
        bool flag = false;
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse { Item.ModItem: SludgePump item })
            {
                parentItem = item;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (parentItem is null)
            {
                return false;
            }
            DrawBeam(ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/SludgeProjectile/ProjectileTracer").Value, 0, Projectile.owner, Projectile);
            return false;
        }
        public void DrawBeam(Texture2D texture, float direction, int owner, Projectile projectile)
        {
            Color teamColor = DRGHelpers.GetTeamColor(Main.player[owner].team);
            Vector2 start = Main.player[owner].Center - Main.screenPosition;
            Vector2 drawOrigin = new(1, 1);
            Vector2 position = start;
            int MAX_DISTANCE = 1600;
            for (int i = 0; i < MAX_DISTANCE; i++)
            {
                if (((i - parentItem.TIMER) % (SludgePump.MAX_TIMER + 1)) == parentItem.TIMER)
                {
                    flag = !flag;
                }
                position += projectile.wet ? projectile.velocity / 20 : projectile.velocity / 10;

                if (flag)
                {
                    Main.EntitySpriteDraw(texture, position, null, teamColor, direction, drawOrigin, 1f, SpriteEffects.None, 0);
                }
                if (Projectile.ai[1] != ModContent.ItemType<AntiGravOC>())
                {
                    projectile.velocity.Y += projectile.velocity.Y < 30 ? .05f : 0;
                }
                Vector2 adjustedPos = position + Main.screenPosition;
                projectile.wet = false;
                if (Collision.WetCollision(adjustedPos, projectile.width, projectile.height) || Collision.LavaCollision(adjustedPos, projectile.width, projectile.height))
                {
                    projectile.wet = true;
                }

            }
        }
    }
}
