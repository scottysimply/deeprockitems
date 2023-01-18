using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static deeprockitems.MyFunctions;

namespace deeprockitems.Content.Projectiles
{
    public class M1000Helper : ModProjectile
    {
        bool charged = true;
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 3600;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner]; // projectile owner aka player
            Vector2 RelativeMouse = MouseRel(Main.MouseWorld, owner);
            float maxCharge = 40f; // CHANGE THIS TO SET CHARGE TIME. 60 ticks = 1 second
            Projectile.position = owner.Center;
            int projDamage = Projectile.damage;
            if (!charged) // one time stat change
            {
                projDamage *= 3;
            }
            if (owner.channel) // is owner still holding down click?
            {
                RenderItem(owner, RelativeMouse, Main.MouseWorld);
                if (Projectile.ai[0] < maxCharge) // weapon is charging right now
                {
                    Projectile.ai[0]++;
                }
                else
                {
                    if (charged) // this is a one time flag
                    {
                        SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Items/M1000Focus") with { Volume = .6f }, owner.position); // SoundID.Item149
                        charged = false;
                    }
                }
            }
            else // owner has stopped holding, spawn bullet
            {
                if (Main.myPlayer == Projectile.owner) // spawning the actual projectile
                {
                    SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/M1000Fire") with { Volume = .5f }, owner.position);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Normalize(RelativeMouse) * 20f, ModContent.ProjectileType<M1000Proj>(), projDamage, Projectile.knockBack, Main.myPlayer, Projectile.ai[0], maxCharge);
                }
                Projectile.Kill();
            }
        }
    }
}