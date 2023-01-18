using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static deeprockitems.MyFunctions;

namespace deeprockitems.Content.Projectiles
{
    public class LaserPointerBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 60;
          //  Projectile.hide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(ModContent.ProjectileType<LaserPointerBeam>());
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int owner = Projectile.owner;
            float direction = (Main.MouseWorld - Main.player[owner].Center).ToRotation();
            DrawLaser(texture, direction, owner, Projectile);
            Main.NewText("Laser should be visible");
            return false;
        }
        public static void DrawLaser(Texture2D texture, float direction, int owner, Projectile projectile)
        {
            Vector2 drawOrigin = new(texture.Width * .5f, projectile.height * .5f);
            Vector2 start = Main.player[owner].Center;
            Vector2 mouse = Main.MouseWorld - Main.player[owner].Center;
            Color c = TeamColor(Main.player[owner]);
            for (int i = 0; i < 1600; i += 16)
            {
                Vector2 position = start + i * Vector2.Normalize(mouse);
                Main.EntitySpriteDraw(texture, position - Main.screenPosition, new(0, 0, 15, 2), c, direction, drawOrigin, 1f, SpriteEffects.None, 0);
            }
        }
        public static Color TeamColor(Player player)
        {
            if (player.team == 1)
            {
                return Color.Red;
            }
            else if (player.team == 2)
            {
                return Color.Green;
            }
            else if (player.team == 3)
            {
                return Color.Blue;
            }
            else if (player.team == 4)
            {
                return Color.Yellow;
            }
            else if (player.team == 5)
            {
                return Color.Pink;
            }
            return Color.White;
        }
    }
}
        