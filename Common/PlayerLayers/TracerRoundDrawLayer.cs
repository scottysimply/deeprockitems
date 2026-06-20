using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using deeprockitems.Content.Items.Weapons;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Common.PlayerLayers
{
    public class TracerRoundPlayer : ModPlayer
    {
        public bool IsLayerAllowedToDraw { get; set; } = false;
        public override void PreUpdate() {
            base.PreUpdate();
            IsLayerAllowedToDraw = false;
        }
        public override void PostUpdate() {
            //IsLayerAllowedToDraw = false;
        }
    }
    public class TracerRoundDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo) {
            if (drawInfo.drawPlayer.HeldItem.ModItem is not SludgePump pump) return;
            if (drawInfo.drawPlayer.GetModPlayer<TracerRoundPlayer>().IsLayerAllowedToDraw && Main.myPlayer == drawInfo.drawPlayer.whoAmI)
            {
                // Simulate a projectile
                Vector2 velocity = (Main.MouseWorld - drawInfo.drawPlayer.Center);
                velocity.Normalize();
                velocity = drawInfo.drawPlayer.HeldItem.shootSpeed * velocity;
                float gravityStrength = velocity.Y > 16f ? 0f : 0.5f;

                //Projectile.NewProjectile(drawInfo.drawPlayer.GetSource_ItemUse(drawInfo.drawPlayer.HeldItem), drawInfo.drawPlayer.Center, velocity, ModContent.ProjectileType<SludgeBall>(), 0, 0, drawInfo.drawPlayer.whoAmI);
                // Start is the player's center
                Vector2 start = drawInfo.drawPlayer.Center;
                int width = 20;
                int height = 20;
                int tracerWidth = 4;
                int tracerHeight = 4;
                // This is the center of the projecitle. Used for drawing the tracer
                Vector2 computedCenter = start;
                // This is the top left of the projectile. The projectile's center spawns on the player's center
                Vector2 position = new Vector2(start.X - 0.5f * width, start.Y - 0.5f * height);
                // How close together to put the dots
                const float distanceMultiplier = 10f;
                // Base distance
                //float extraDistance = pump.Upgrades.Contains(ModContent.ItemType<OvertunedNozzle>()) ? 30f : 0f;
                float distance = 120;
                // Test for wetness very first
                bool wet = Collision.WetCollision(position, width, height);
                Color playerColor = DRGHelpers.GetTeamColor(drawInfo.drawPlayer.team); // Get a color corresponding to team color

                // Iteration count
                int timer = 0;

                // Total distance of the dots
                for (int i = 0; i < distance; i++)
                {
                    // Simulate AI (gravity)
                    velocity.Y += gravityStrength;

                    // Determine if projectile is wet
                    bool justGotWet;
                    try
                    {
                        justGotWet = Collision.WetCollision(position, width, height);
                    }
                    catch
                    {
                        return;
                    }

                    // This sets wetness on the frame _after_ entering water.
                    if (justGotWet)
                    {
                        wet = true;
                    }
                    else if (wet)
                    {
                        wet = false;
                    }

                    // Set velocities depending on the fluid.
                    Vector2 wetVelocity = new Vector2(0, 0);
                    if (wet)
                    {
                        if (Collision.shimmer)
                        {
                            wetVelocity = velocity * 0.375f;
                        }
                        else if (Collision.honey)
                        {
                            wetVelocity = velocity * 0.25f;
                        }
                        else
                        {
                            wetVelocity = velocity * 0.5f;
                        }
                    }
                    // Interpolation. This yields something slightly different to a parabola, but it looks close enough
                    for (int j = 0; j < distanceMultiplier; j++)
                    {
                        // Use wet velocity if wet
                        if (wet)
                        {
                            position += wetVelocity * (1 / distanceMultiplier);
                        }
                        else
                        {
                            position += velocity * (1 / distanceMultiplier);
                        }
                        computedCenter = new(position.X + 0.5f * width, position.Y + 0.5f * height);
                        // Check visibility
                        if ((30 + timer - ((int)Main.timeForVisualEffects % 30)) % 30 < 15)
                        {
                            // Draw the individual squares
                            drawInfo.DrawDataCache.Add(new DrawData(Assets.WhitePixel.Value, new Rectangle((int)(computedCenter.X - Main.screenPosition.X - 0.5f * tracerWidth), (int)(computedCenter.Y - Main.screenPosition.Y - 0.5f * tracerHeight), tracerWidth, tracerHeight), playerColor * 0.85f));
                        }
                        timer++;

                        // If hit tile, kill and draw X
                        if (Collision.SolidCollision(position, width, height))
                        {
                            DrawHitSomething(ref drawInfo, computedCenter, distance, distanceMultiplier, timer, playerColor);
                            return;
                        }

                        // If hit NPC, kill and draw X
                        foreach (var npc in Main.ActiveNPCs)
                        {
                            if (npc.friendly) continue;
                            if (npc.Hitbox.Intersects(new Rectangle((int)(position.X), (int)(position.Y), width, height)))
                            {
                                DrawHitSomething(ref drawInfo, computedCenter, distance, distanceMultiplier, timer, playerColor);
                                return;
                            }
                        }
                    }

                }
            }
        }
        public void DrawHitSomething(ref PlayerDrawSet drawInfo, Vector2 computedCenter, float distance, float distanceMultiplier, int timer, Color playerColor) {
            int hitWidth = Assets.TracerHit.Value.Width;
            int hitHeight = Assets.TracerHit.Value.Height;
            float rotation = drawInfo.drawPlayer.direction * ((timer) / (distance * distanceMultiplier));
            Vector2 origin = new(hitWidth * 0.5f, hitHeight * 0.5f);

            drawInfo.DrawDataCache.Add(new DrawData(Assets.TracerHit.Value, new Rectangle((int)(computedCenter.X - Main.screenPosition.X), (int)(computedCenter.Y - Main.screenPosition.Y), hitWidth, hitHeight), null, playerColor, rotation, origin, SpriteEffects.None));
            return;
        }
    }
}