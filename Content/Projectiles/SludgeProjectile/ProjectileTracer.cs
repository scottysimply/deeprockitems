using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static System.Math;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Content.Items.Upgrades.Overclocks;

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
            DrawBeam(ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/SludgeProjectile/ProjectileTracer").Value, 0, Projectile.owner, Projectile);
            return false;
        }
        public void DrawBeam(Texture2D texture, float direction, int owner, Projectile projectile)
        {
            Color teamColor = Main.player[owner].team switch
            {
                1 => new(.9f, .3f, .3f, .85f), // Red
                2 => new(.3f, .9f, .3f, .85f), // Green
                3 => new(.4f, .8f, .8f, .8f), // Blue
                4 => new(.8f, .8f, .25f, .85f), // Yellow
                5 => new(.8f, .25f, .75f, .85f), // Pink
                _ => new(.95f, .95f, .95f, .8f) // No team
            };
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
