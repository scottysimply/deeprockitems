/*using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using deeprockitems.Content.Items.Weapons;

namespace deeprockitems.Common.Overides
{
    public class ProjectileTracer : ModPlayer
    {
        private int TIMER;
        private bool flag;
        private Texture2D sprite;

        public override void Initialize()
        {
            sprite = ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/ProjectileTracer").Value;
        }

        public override void PreUpdate()
        {
            if (Player.HeldItem.ModItem is SludgePump modItem)
            {
                if (modItem.Upgrades[0])
                {
                }
                else
                {
                    
                }
            }
        }
        public override 

        void SludgeTracer(Texture2D texture, float direction, Vector2 simulVelocity, bool doGrav)
        {
            Vector2 drawOrigin = new(1, 1);
            Color teamColor = Player.team switch
            {
                1 => new(.9f, .3f, .3f, .85f), // Red
                2 => new(.3f, .9f, .3f, .85f), // Green
                3 => new(.4f, .8f, .8f, .8f), // Blue
                4 => new(.8f, .8f, .25f, .8f), // Yellow
                5 => new(.8f, .25f, .75f, .8f), // Pink
                _ => new(.95f, .95f, .95f, .85f) // No team
            };
            Vector2 start = Player.position + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 position = start;
            for (int i = 0; i < 1600; i++)
            {
                if (i % 30 == 0)
                {
                    flag = !flag;
                }

                position += simulVelocity / 10;
                if (flag)
                {
                    Main.EntitySpriteDraw(texture, position, null, teamColor, direction, drawOrigin, 1f, SpriteEffects.None, 0);
                }
                if (doGrav)
                {
                    simulVelocity.Y += simulVelocity.Y < 30 ? .05f : 0;
                }
            }
        }
    }
}
*/