using Terraria;
using Terraria.ModLoader;

namespace deeprockitems.Content.Dusts
{
    public class SludgeDust : ModDust
    {
        public override void OnSpawn(Terraria.Dust dust)
        {
            dust.noGravity = true;
            dust.velocity = Main.rand.NextVector2Unit() * 4f;
        }
        public override bool Update(Terraria.Dust dust)
        {
            // Update dust's position
            dust.position += dust.velocity; 

            // Make dust rotation be based on velocity.
            dust.rotation += dust.velocity.X / 10;

            // Make the dust shrink as it travels.
            dust.scale *= .99f;
            // If dust is too small, kill it.
            if (dust.scale < .4f)
            {
                dust.active = false;
            }

            // Parabolic dust trajectory
            dust.velocity.Y += .2f;

            // Dust gravity cap
            if (dust.velocity.Y > 5f)
            {
                dust.velocity.Y = 5f;
            }

            // Return false to stop vanilla dust code from running.
            return false;
        }
    }
}
