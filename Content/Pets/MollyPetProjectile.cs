using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace deeprockitems.Content.Pets
{
    public class MollyPetProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.CanDistortWater[Projectile.type] = false;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Pet;
            Projectile.width = 60;
            Projectile.height = 60;
        }
        /// <summary>
        /// This is the "current state" for molly's animations. This controls whether she's displayed as being on the walls or on the floor.
        /// </summary>
        private int MollyBodyCurrentState { get => (int)Projectile.localAI[1]; set => Projectile.localAI[1] = value; } 
        /// <summary>
        /// This is the the "AFK" timer for molly. We want her to "seek" the player if they're AFK.
        /// </summary>
        private int MollyDistanceTimer { get => (int)Projectile.localAI[0]; set => Projectile.localAI[0] = value; }
        /// <summary>
        /// This is the timer Molly uses while sitting down.
        /// </summary>
        private int MollySittingTimer { get => (int)Projectile.localAI[2]; set => Projectile.localAI[2] = value; }
        bool FastTraveling = false;
        Vector2 LastSignificantPosition;
        public override void OnSpawn(IEntitySource source)
        {
            LastSignificantPosition = Main.player[Projectile.owner].Center;
        }
        public override void AI()
        {
            // Get the player's current position if they're far from where they used to be, or if it has been a little bit and the player is still there.
            if (Vector2.Distance(LastSignificantPosition, Main.player[Projectile.owner].Center) > 240 || MollyDistanceTimer > 600)
            {
                LastSignificantPosition = Main.player[Projectile.owner].Center;
                MollyDistanceTimer = 0; // Reset time spent in this position.
            }
            else if (LastSignificantPosition != Main.player[Projectile.owner].Center)
            {
                MollyDistanceTimer++; // Projectile.localAI[0] is the time the player spent in the same general area. Molly will update the last "important" position if the player is AFK, or in the same general area.
            }

            // See if there are walls around molly...
            int projectileTileCoordX = (int)(Projectile.Center.X / 16f);
            int projectileTileCoordY = (int)(Projectile.Center.Y / 16f);
            Tile blockCenter = Framing.GetTileSafely(projectileTileCoordX, projectileTileCoordY);
            Tile blockLeft = Framing.GetTileSafely(projectileTileCoordX - 1, projectileTileCoordY);
            Tile blockRight = Framing.GetTileSafely(projectileTileCoordX + 1, projectileTileCoordY);

            // ...In order to see if molly should be on the walls or the floor.
            if ((blockCenter.WallType + blockLeft.WallType + blockRight.WallType) > 1)
            {
                // Wall code
                MollyBodyCurrentState = 1;
            }
            else
            {
                // Floor code
                MollyBodyCurrentState = 0;
                // Gravity
                Projectile.velocity.Y = Projectile.velocity.Y > 10 ? 10 : Projectile.velocity.Y + .4f;
            }

            float distance = Vector2.Distance(Projectile.Center, LastSignificantPosition);
            MollyBodyCurrentState += 2;
            if (distance > 240)
            {
                
            }
            else // Molly is close to the player. She will stop walking.
            {
                MollyBodyCurrentState -= 2;
                FastTraveling = false;
                // TODO: Insert ability to use as chester (but for the safe)
            }

            
        }
        public override string Texture => "deeprockitems/Content/Pets/MollyBody";
        private Texture2D molly_body_frames => ModContent.Request<Texture2D>("deeprockitems/Content/Pets/MollyBody").Value;
        private Texture2D front_leg_frames => ModContent.Request<Texture2D>("deeprockitems/Content/Pets/MollyFrontLegs").Value;
        private Texture2D rear_leg_frames => ModContent.Request<Texture2D>("deeprockitems/Content/Pets/MollyHindLegs").Value;
        private Texture2D top_leg_frames => ModContent.Request<Texture2D>("deeprockitems/Content/Pets/MollyTopLegs").Value;
        private Texture2D molly_head => ModContent.Request<Texture2D>("deeprockitems/Content/pets/MollyHead").Value;
        public override bool PreDraw(ref Color lightColor)
        {
            
            Vector2 DrawPosition = Projectile.position - Main.screenPosition;
            Projectile.frameCounter++;
            if (Projectile.frame != -1)
            {
                if (MollyBodyCurrentState % 2 == 0)
                {
                    // Draw hind legs:
                    Main.EntitySpriteDraw(rear_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.None);
                    Projectile.frame = TryToIterateFrame(Projectile.frameCounter, Projectile.frame) % 6;
                    Main.EntitySpriteDraw(rear_leg_frames, DrawPosition - new Vector2(4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.FlipHorizontally);
                    Projectile.frame = TryToIterateFrame(Projectile.frameCounter, Projectile.frame) % 6;

                    // Draw molly's body:
                    Main.EntitySpriteDraw(molly_body_frames, DrawPosition, new Rectangle(0, MollyBodyCurrentState * 60, 60, 60), Color.White, Projectile.rotation, new Vector2(30, 30), 1f, SpriteEffects.None);


                    // Draw molly's front legs:
                    Main.EntitySpriteDraw(front_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.None);
                    Projectile.frame = TryToIterateFrame(Projectile.frameCounter, Projectile.frame) % 6;
                    Main.EntitySpriteDraw(front_leg_frames, DrawPosition - new Vector2(4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.FlipHorizontally);
                    Projectile.frame = TryToIterateFrame(Projectile.frameCounter, Projectile.frame) % 6;
                }
                else
                {
                    // Draw all 4 legs:
                    Main.EntitySpriteDraw(top_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.None);
                    Main.EntitySpriteDraw(top_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.FlipHorizontally);
                    Main.EntitySpriteDraw(top_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.FlipVertically);
                    Main.EntitySpriteDraw(top_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation + MathHelper.Pi, new Vector2(6, 6), 1f, SpriteEffects.None);


                    // Draw body
                    Main.EntitySpriteDraw(molly_body_frames, DrawPosition, new Rectangle(0, MollyBodyCurrentState * 60, 60, 60), Color.White, Projectile.rotation, new Vector2(30, 30), 1f, SpriteEffects.None);
                }
            }
            else
            {
                // Draw molly's front legs:
                Main.EntitySpriteDraw(front_leg_frames, DrawPosition - new Vector2(-4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.None);
                Main.EntitySpriteDraw(front_leg_frames, DrawPosition - new Vector2(4, 4), new Rectangle(0, Projectile.frame * 13, 13, 13), Color.White, Projectile.rotation, new Vector2(6, 6), 1f, SpriteEffects.FlipHorizontally);

                // Draw body
                Main.EntitySpriteDraw(molly_body_frames, DrawPosition, new Rectangle(0, MollyBodyCurrentState * 60, 60, 60), Color.White, Projectile.rotation, new Vector2(30, 30), 1f, SpriteEffects.None);
            }
            
            

            return false;
        }
        
        private static int TryToIterateFrame(int framecounter, int frame)
        {
            if (framecounter % 2 == 0)
            {
                return frame++;
            }
            return frame;
        }
    }
}
