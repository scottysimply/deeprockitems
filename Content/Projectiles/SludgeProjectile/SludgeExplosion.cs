using deeprockitems.Content.Buffs;
using deeprockitems.Content.Projectiles.Globals;
using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Projectiles.SludgeProjectile
{
    public class SludgeExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 72;
            Projectile.scale = 2f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            DrawOffsetX = Projectile.height / 2;
            DrawOriginOffsetY = Projectile.height / 2;

        }
        public override bool ShouldUpdatePosition() => false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.Center.X > Projectile.Center.X)
            {
                modifiers.HitDirectionOverride = 1;
            }
            else
            {
                modifiers.HitDirectionOverride = -1;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.AddInstancedBuff(600, out Sludged? buff))
            {
                buff.SlowingSludge = Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("SlowingSludge");
                buff.StrongSludge = Projectile.GetGlobalProjectile<UpgradeGlobalProjectile>().IsUpgradeEquipped("StrongSludge");
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
    }
}
