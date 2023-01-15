using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static deeprockitems.MyFunctions;

namespace deeprockitems.Content.Projectiles
{
    public class SludgeHelper : ModProjectile
    {
        bool charged = true;
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 3600;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;

        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner]; // projectile owner aka player
            Vector2 RelativeMouse = MouseRel(Main.MouseWorld, owner);
            Projectile.position = owner.Center;
            float maxCharge = 60f;
            int projDamage = Projectile.damage;
            if (!charged)
            {
                projDamage *= 2;

            }
            if (owner.channel)
            {
                RenderItem(owner, RelativeMouse, Main.MouseWorld);
                if (Projectile.ai[0] < maxCharge) // weapon is charging right now
                {
                    Projectile.ai[0]++;
                }
                else
                {
                    if (charged)
                    {
                        SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Items/SludgePumpFocus") with { Volume = .8f }, owner.position);
                        charged = false;
                    }
                }
            }
            else
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    SoundEngine.PlaySound(new SoundStyle("deeprockitems/Assets/Sounds/Projectiles/SludgePumpFire") with { Volume = .5f }, owner.position);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Normalize(RelativeMouse) * 15f, ModContent.ProjectileType<SludgeBall>(), projDamage, Projectile.knockBack, Main.myPlayer, Projectile.ai[0], maxCharge);
                }
                Projectile.Kill();
            }
        }
    }
}