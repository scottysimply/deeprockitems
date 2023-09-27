using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using deeprockitems.Utilities;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.Audio;
using deeprockitems.Common.Items;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace deeprockitems.Content.Pets.Molly
{
    public class MollyPetProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 14;
            Main.projPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(313);
            Projectile.aiStyle = -1;
            Projectile.width = 36;
            Projectile.height = 36;

            DrawOffsetX = -10;
            DrawOriginOffsetY = -14;
            DrawOriginOffsetX = 0;
        }

        private float CurrentState { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
        private float SitTimer { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        private float CurrentFrame { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public override void AI()
        {
            Main.CurrentFrameFlags.HadAnActiveInteractibleProjectile = true; // This is required to make the projectile be considered "interactible"
            // Kill other instaces of Molly owned by this player
            if (Main.myPlayer == Projectile.owner)
            {
                foreach (Projectile proj in Main.projectile)
                {
                    // If this player has another instance of molly (which they shouldn't, but just in case they spawn her in), KILL IT!!! BUGG!!!
                    if (proj.type == Projectile.type && Projectile.whoAmI != proj.whoAmI && proj.active && proj.owner == Projectile.owner)
                    {
                        if (Projectile.timeLeft >= proj.timeLeft)
                        {
                            proj.Kill();
                        }
                        else
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }








            // Kill projectile if the buff isn't equipped
            if (!Main.player[Projectile.owner].buffType.Contains(ModContent.BuffType<MollyPetBuff>()))
            {
                Projectile.Kill();
                return;
            }
            if (!Main.player[Projectile.owner].dead)
            {
                Projectile.timeLeft = 2;
            }

            Vector2 deltaPlayerPosition = Main.player[Projectile.owner].Center - Projectile.Center;
            float distance = deltaPlayerPosition.Length();
            bool isLeftOfPlayer = false;
            bool isRightOfPlayer = false;

            // These parameters are user defined, and control the projectile's constraints. Change these to make the projectile behave differently
            float flying_acceleration = .2f;
            float teleport_threshold = 2000f;
            float fly_threshold = 250f;
            float running_acceleration = 0.2f;
            float max_running_velocity = 6f;
            float follow_distance = 85f;
            float fly_variable_2 = 200f;

            if (Main.player[Projectile.owner].Center.X < Projectile.Center.X - follow_distance)
            {
                isRightOfPlayer = true;
            }
            else if (Main.player[Projectile.owner].Center.X > Projectile.Center.X + follow_distance)
            {
                isLeftOfPlayer = true;
            }
            if (CurrentState == 0f) // Misc state
            {
                if (distance > teleport_threshold)
                {
                    Projectile.tileCollide = true;
                    Projectile.Center = Main.player[Projectile.owner].Center;
                }
                else if (distance > fly_threshold)
                {
                    Projectile.tileCollide = false;
                    CurrentState = 1f;
                }
            }
            if (CurrentState > 0) // Flying to player
            {
                if (distance < fly_variable_2 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.BottomRight.Y <= Main.player[Projectile.owner].BottomRight.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    CurrentState = 0f;
                    if (Projectile.velocity.Y < -6f)
                    {
                        Projectile.velocity.Y = -6f;
                    }
                }
                if (distance < 60f)
                {
                    deltaPlayerPosition = Projectile.velocity;
                }
                else
                {
                    distance = 10f / distance;
                    deltaPlayerPosition *= distance;
                }

                if (Projectile.velocity.X < deltaPlayerPosition.X)
                {
                    Projectile.velocity.X += flying_acceleration;
                    if (Projectile.velocity.X < 0f)
                    {
                        Projectile.velocity.X += flying_acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.X > deltaPlayerPosition.X)
                {
                    Projectile.velocity.X -= flying_acceleration;
                    if (Projectile.velocity.X > 0f)
                    {
                        Projectile.velocity.X -= flying_acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.Y < deltaPlayerPosition.Y)
                {
                    Projectile.velocity.Y += flying_acceleration;
                    if (Projectile.velocity.Y < 0f)
                    {
                        Projectile.velocity.Y += flying_acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.Y > deltaPlayerPosition.Y)
                {
                    Projectile.velocity.Y -= flying_acceleration;
                    if (Projectile.velocity.Y > 0f)
                    {
                        Projectile.velocity.Y -= flying_acceleration * 1.5f;
                    }
                }
                // Manage animations, etc.
                Projectile.rotation = Projectile.velocity.X * .15f;
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.direction = Projectile.spriteDirection = -1;
                }
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.direction = Projectile.spriteDirection = 1;
                }
                Projectile.frameCounter = 0;
                Projectile.frame = 11;
                // Spawn the dust
                Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4f, Projectile.Bottom.Y).RotatedBy(Projectile.rotation, Projectile.Center), 8, 8, 16, (0f - Projectile.velocity.X) * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f);
                smoke.velocity *= .2f; // Slow the dust down
                smoke.noGravity = true; // nograv

            }
            else // Walking or stationary
            {
                Projectile.tileCollide = true;
                bool solidTile = false;
                bool abovePlayer = false;

                if (isRightOfPlayer)
                {
                    if (Projectile.velocity.X > -3.5f)
                    {
                        Projectile.velocity.X -= running_acceleration;
                    }
                    else
                    {
                        Projectile.velocity.X -= running_acceleration * .25f;
                    }
                }
                else if (isLeftOfPlayer)
                {
                    if (Projectile.velocity.X < 3.5f)
                    {
                        Projectile.velocity.X += running_acceleration;
                    }
                    else
                    {
                        Projectile.velocity.X += running_acceleration * .25f;
                    }
                }
                else
                {
                    Projectile.velocity.X *= .9f;
                    if (Projectile.velocity.X >= 0f - running_acceleration && Projectile.velocity.X <= running_acceleration)
                    {
                        Projectile.velocity.X = 0;
                    }
                }
                if (isRightOfPlayer || isLeftOfPlayer)
                {
                    int tileX = Projectile.Center.ToTileCoordinates().X;
                    int tileY = Projectile.Center.ToTileCoordinates().Y;
                    if (isRightOfPlayer)
                    {
                        tileX--;
                    }
                    if (isLeftOfPlayer)
                    {
                        tileX++;
                    }
                    tileX += (int)Projectile.velocity.X;
                    if (WorldGen.SolidTile(tileX, tileY))
                    {
                        solidTile = true;
                    }
                }
                if (Main.player[Projectile.owner].BottomRight.Y - 8f > Projectile.BottomRight.Y)
                {
                    abovePlayer = true;
                }
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);

                if (Projectile.velocity.Y == 0f)
                {
                    if (!abovePlayer && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
                    {
                        int tileX2 = Projectile.Center.ToTileCoordinates().X;
                        int tileY2 = Projectile.Center.ToTileCoordinates().Y + 1;
                        if (isRightOfPlayer)
                        {
                            tileX2--;
                        }
                        if (isLeftOfPlayer)
                        {
                            tileX2++;
                        }
                        WorldGen.SolidTile(tileX2, tileY2);
                    }
                    if (solidTile)
                    {
                        int tileX3 = Projectile.Center.ToTileCoordinates().X;
                        int tileY3 = Projectile.BottomRight.ToTileCoordinates().Y;
                        if (WorldGen.SolidTileAllowBottomSlope(tileX3, tileY3) || Main.tile[tileX3, tileY3].IsHalfBlock || Main.tile[tileX3, tileY3].Slope > 0)
                        {
                            try
                            {
                                tileX3 = Projectile.Center.ToTileCoordinates().X;
                                tileY3 = Projectile.Center.ToTileCoordinates().Y;
                                if (isRightOfPlayer)
                                {
                                    tileX3--;
                                }
                                if (isLeftOfPlayer)
                                {
                                    tileX3++;
                                }
                                tileX3 += (int)Projectile.velocity.X;
                                if (!WorldGen.SolidTile(tileX3, tileY3 - 1) && !WorldGen.SolidTile(tileX3, tileY3 - 2))
                                {
                                    Projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(tileX3, tileY3 - 2))
                                {
                                    Projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(tileX3, tileY3 - 5))
                                {
                                    Projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(tileX3, tileY3 - 4))
                                {
                                    Projectile.velocity.Y = -10.1f;
                                }
                                else
                                {
                                    Projectile.velocity.Y = -9.1f;
                                }
                            }
                            catch
                            {
                                Projectile.velocity.Y = -9.1f;
                            }
                        }
                    }
                }
                if (Projectile.velocity.X > max_running_velocity)
                {
                    Projectile.velocity.X = max_running_velocity;
                }
                if (Projectile.velocity.X < 0f - max_running_velocity)
                {
                    Projectile.velocity.X = 0f - max_running_velocity;
                }
                



                // Vanilla spider egg code, since this pet behaves identically aside from functioning as a safe.
                Spider_egg_AI();
            }
        }
        // Just the vanilla code
        private void Spider_egg_AI()
        {
            bool stationary = Projectile.position.X - Projectile.oldPosition.X == 0f;
            int tileX = (int)(Projectile.Center.X / 16f);
            int tileY = (int)(Projectile.Center.Y / 16f);
            int backgroundWallCounter = 0;
            Tile tileBelow = Framing.GetTileSafely(tileX, tileY);
            Tile tileLeft = Framing.GetTileSafely(tileX, tileY - 1);
            Tile tileRight = Framing.GetTileSafely(tileX, tileY + 1);
            if (tileBelow.WallType > 0)
            {
                backgroundWallCounter++;
            }
            if (tileLeft.WallType > 0)
            {
                backgroundWallCounter++;
            }
            if (tileRight.WallType > 0)
            {
                backgroundWallCounter++;
            }
            if (backgroundWallCounter > 1)
            {
                Vector2 vector5 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num101 = Main.player[Projectile.owner].Center.X - vector5.X;
                float num102 = Main.player[Projectile.owner].Center.Y - vector5.Y;
                float num103 = Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center);
                float num104 = 4f / num103;
                num101 *= num104;
                num102 *= num104;
                if (num103 < 120f)
                {
                    Projectile.velocity.X *= 0.9f;
                    Projectile.velocity.Y *= 0.9f;
                    if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < 0.1)
                    {
                        Projectile.velocity *= 0f;
                    }
                }
                else
                {
                    Projectile.velocity.X = (Projectile.velocity.X * 9f + num101) / 10f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 9f + num102) / 10f;
                }
                if (num103 >= 120f)
                {
                    Projectile.spriteDirection = Projectile.direction;
                    Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y * (float)(-Projectile.direction), Projectile.velocity.X * (float)(-Projectile.direction));
                }
                float velocity_strength = Projectile.velocity.Length();
                if (velocity_strength > 0)
                {
                    Projectile.frameCounter++;
                }
                if (velocity_strength > .8f)
                {
                    Projectile.frameCounter++;
                }
                if (velocity_strength > 1.6f)
                {
                    Projectile.frameCounter++;
                }
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 10)
                {
                    Projectile.frame = 5;
                }
                if (Projectile.frame < 5)
                {
                    Projectile.frame = 10;
                }
            }
            else
            {
                Projectile.rotation = 0f;
                if (!stationary)
                {
                    if (Projectile.direction == -1)
                    {
                        Projectile.spriteDirection = 1;
                    }
                    if (Projectile.direction == 1)
                    {
                        Projectile.spriteDirection = -1;
                    }
                }
                
                if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
                {
                    if (stationary)
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }
                    else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
                    {
                        Projectile.frameCounter++;
                        float abs = Math.Abs(Projectile.velocity.X);
                        if (abs > .4f)
                        {
                            Projectile.frameCounter++;
                        }
                        if (abs > .8f)
                        {
                            Projectile.frameCounter++;
                        }
                        if (Projectile.frameCounter > 6)
                        {
                            Projectile.frame++;
                            Projectile.frameCounter = 0;
                        }
                        if (Projectile.frame > 3)
                        {
                            Projectile.frame = 0;
                        }
                    }
                    else
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }

                }
                else
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 4;
                }
                Projectile.velocity.Y += .4f;
                if (Projectile.velocity.Y > 10f)
                {
                    Projectile.velocity.Y = 10f;
                }

            }

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
    public class MollyDetours : ModSystem
    {
        public override void Load()
        {
            On_Projectile.TryGetContainerIndex += On_Projectile_TryGetContainerIndex;
            On_Player.HandleBeingInChestRange += On_Player_HandleBeingInChestRange;
            On_Main.DrawProj_Inner_DoDrawProj += DrawProjectiles;
            On_Player.clientClone += On_Player_clientClone;
            On_Player.IsProjectileInteractibleAndInInteractionRange += IsInteractible;
        }
        public bool IsInteractible(On_Player.orig_IsProjectileInteractibleAndInInteractionRange orig, Player self, Projectile proj, ref Vector2 compareSpot)
        {
            if (!proj.active)
            {
                return false;
            }
            if (!proj.IsInteractible() && proj.type != ModContent.ProjectileType<MollyPetProjectile>())
            {
                return false;
            }
            Point point = proj.Hitbox.ClosestPointInRect(compareSpot).ToTileCoordinates();
            if (!self.IsInTileInteractionRange(point.X, point.Y, TileReachCheckSettings.Simple))
            {
                return false;
            }
            return true;
        }

        /*private void PlayerConstructor(On_Player.orig_ctor orig, Player self)
        {
            if (self is null)
            {
                orig(self);
            }
            if (self.TryGetModPlayer(out MollyModPlayer modPlayer))
            {
                modPlayer.MULEInventoryProjTracker.Clear();
            }
            orig(self);
            return;
        }*/

        private Player On_Player_clientClone(On_Player.orig_clientClone orig, Player self)
        {
            Player player = orig(self);
            if (self.TryGetModPlayer(out MollyModPlayer modPlayerParent) && player.TryGetModPlayer(out MollyModPlayer modPlayerClone))
            {
                modPlayerClone.MULEInventoryProjTracker = modPlayerParent.MULEInventoryProjTracker;
                return modPlayerClone.Player;
            }
            return player;
        }

        private void DrawProjectiles(On_Main.orig_DrawProj_Inner_DoDrawProj orig, Main main, Projectile proj, Vector2 mountedCenter, float polePosX, float polePosY)
        {
            if (proj.type != ModContent.ProjectileType<MollyPetProjectile>())
            {
                orig(main, proj, mountedCenter, polePosX, polePosY);
                return;
            }
            else
            {
                Color lightColor = Lighting.GetColor(proj.Center.ToTileCoordinates());
                if (ProjectileLoader.PreDraw(proj, ref lightColor))
                {
                    Texture2D texture = ModContent.Request<Texture2D>("deeprockitems/Content/Pets/Molly/MollyPetProjectile").Value;



                    // manually draw projectile

                    int xOffset = 0;
                    int yOffset = 0;

                    float xOrigin = (float)(texture.Width - proj.width) * 0.5f + (float)proj.width * 0.5f;


                    ProjectileLoader.DrawOffset(proj, ref xOffset, ref yOffset, ref xOrigin);

                    int frameSize = texture.Height / Main.projFrames[proj.type];
                    int frameY = frameSize * proj.frame;

                    SpriteEffects spriteEffects = SpriteEffects.None;
                    if (proj.spriteDirection == -1)
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }

                    Vector2 DrawPosition = new()
                    {
                        X = proj.position.X - Main.screenPosition.X + xOffset + xOrigin,
                        Y = proj.position.Y - Main.screenPosition.Y + proj.gfxOffY + proj.height/2,
                    };
                    Vector2 DrawOrigin = new()
                    {
                        X = xOrigin,
                        Y = proj.height/2 + yOffset,
                    };
                    Rectangle Frame = new()
                    {
                        X = 0,
                        Y = frameY,
                        Width = texture.Width,
                        Height = frameSize - 1,
                    };


                    // Draw the projectile itself
                    Color colorAffectcedByLight = proj.GetAlpha(lightColor);
                    Main.EntitySpriteDraw(texture, DrawPosition, Frame, colorAffectcedByLight, proj.rotation, DrawOrigin, proj.scale, spriteEffects, 0);

                    int trackedResult = TryInteractingWithMULE(proj);
                    if (trackedResult == 0)
                    {
                        return;
                    }

                    int colorResult = (lightColor.R + lightColor.G + lightColor.B) / 3;
                    if (colorResult > 10)
                    {
                        Texture2D outline = ModContent.Request<Texture2D>("deeprockitems/Content/Pets/Molly/MollyPetOutline").Value;
                        Color selectionGlowColor = Colors.GetSelectionGlowColor(trackedResult == 2, colorResult);
                        Main.EntitySpriteDraw(outline, DrawPosition, Frame, selectionGlowColor, proj.rotation, DrawOrigin, proj.scale, spriteEffects, 0);
                    }
                }
                ProjectileLoader.PostDraw(proj, lightColor);
            }
        }

        private static int TryInteractingWithMULE(Projectile proj)
        {
            if (Main.gamePaused || Main.gameMenu)
            {
                return 0;
            }
            bool flag = !Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad;
            Player localPlayer = Main.LocalPlayer;
            if (!localPlayer.TryGetModPlayer(out MollyModPlayer modPlayer)) return 0;
            Microsoft.Xna.Framework.Point point = proj.Center.ToTileCoordinates();
            Vector2 compareSpot = localPlayer.Center;
            if (!localPlayer.IsProjectileInteractibleAndInInteractionRange(proj, ref compareSpot))
            {
                return 0;
            }
            /*Matrix matrix = Matrix.Invert(Main.GameViewMatrix.ZoomMatrix);
            Vector2 position = Main.ReverseGravitySupport(Main.MouseScreen);
            Vector2.Transform(Main.screenPosition, matrix);
            Vector2 v = Vector2.Transform(position, matrix) + Main.screenPosition;*/
            bool flag2 = proj.Hitbox.Contains(Main.MouseWorld.ToPoint());
            if (!((flag2 || Main.SmartInteractProj == proj.whoAmI) & !localPlayer.lastMouseInterface))
            {
                if (!flag)
                {
                    return 1;
                }
                return 0;
            }
            Main.HasInteractibleObjectThatIsNotATile = true;
            if (flag2)
            {
                localPlayer.noThrow = 2;
                localPlayer.cursorItemIconEnabled = true;
                localPlayer.cursorItemIconID = ModContent.ItemType<ChunkOfNitra>();
                if (proj.type == ModContent.ProjectileType<MollyPetProjectile>())
                {
                    localPlayer.cursorItemIconID = ModContent.ItemType<ChunkOfNitra>();
                }
            }
            if (PlayerInput.UsingGamepad)
            {
                localPlayer.GamepadEnableGrappleCooldown();
            }
            if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
            {
                Main.mouseRightRelease = false;
                localPlayer.tileInteractAttempted = true;
                localPlayer.tileInteractionHappened = true;
                localPlayer.releaseUseTile = false;
                if (localPlayer.chest == -3)
                {
                    localPlayer.chest = -1;
                    SoundEngine.PlaySound(in SoundID.MenuClose);
                    Recipe.FindRecipes();
                }
                else
                {
                    localPlayer.chest = -3;
                    for (int i = 0; i < 40; i++)
                    {
                        ItemSlot.SetGlow(i, -1f, chest: true);
                    }
                    modPlayer.MULEInventoryProjTracker.Set(proj);
                    localPlayer.chestX = point.X;
                    localPlayer.chestY = point.Y;
                    localPlayer.SetTalkNPC(-1);
                    Main.SetNPCShopIndex(0);
                    Main.playerInventory = true;
                    SoundEngine.PlaySound(in SoundID.MenuOpen);
                    Recipe.FindRecipes();
                    proj.ai[2] = 13;
                }
            }
            if (!Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad)
            {
                return 0;
            }
            if (!flag)
            {
                return 2;
            }
            return 0;
        }


        private void On_Player_HandleBeingInChestRange(On_Player.orig_HandleBeingInChestRange orig, Player self)
        {
            if (!self.TryGetModPlayer(out MollyModPlayer modPlayer))
            {
                orig(self);
                return;
            }
            if (self.chest != -1)
            {
                if (self.chest != -2)
                {
                    self.piggyBankProjTracker.Clear();
                }
                if (self.chest != -3)
                {
                    modPlayer.MULEInventoryProjTracker.Clear();
                }
                if (self.chest != -5)
                {
                    self.voidLensChest.Clear();
                }
                bool flag = false;
                int projectileLocalIndex = self.piggyBankProjTracker.ProjectileLocalIndex;
                if (projectileLocalIndex >= 0)
                {
                    flag = true;
                    if (!Main.projectile[projectileLocalIndex].active || (Main.projectile[projectileLocalIndex].type != 525 && Main.projectile[projectileLocalIndex].type != 960))
                    {
                        Main.PlayInteractiveProjectileOpenCloseSound(Main.projectile[projectileLocalIndex].type, open: false);
                        self.chest = -1;
                        Recipe.FindRecipes();
                    }
                    else
                    {
                        int num = (int)(((double)self.position.X + (double)self.width * 0.5) / 16.0);
                        int num2 = (int)(((double)self.position.Y + (double)self.height * 0.5) / 16.0);
                        Vector2 vector = Main.projectile[projectileLocalIndex].Hitbox.ClosestPointInRect(self.Center);
                        self.chestX = (int)vector.X / 16;
                        self.chestY = (int)vector.Y / 16;
                        if (num < self.chestX - Player.tileRangeX || num > self.chestX + Player.tileRangeX + 1 || num2 < self.chestY - Player.tileRangeY || num2 > self.chestY + Player.tileRangeY + 1)
                        {
                            if (self.chest != -1)
                            {
                                Main.PlayInteractiveProjectileOpenCloseSound(Main.projectile[projectileLocalIndex].type, open: false);
                            }
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                    }
                }
                int projectileLocalIndex2 = self.voidLensChest.ProjectileLocalIndex;
                if (projectileLocalIndex2 >= 0)
                {
                    flag = true;
                    if (!Main.projectile[projectileLocalIndex2].active || Main.projectile[projectileLocalIndex2].type != 734)
                    {
                        SoundEngine.PlaySound(in SoundID.Item130);
                        self.chest = -1;
                        Recipe.FindRecipes();
                    }
                    else
                    {
                        int num3 = (int)(((double)self.position.X + (double)self.width * 0.5) / 16.0);
                        int num4 = (int)(((double)self.position.Y + (double)self.height * 0.5) / 16.0);
                        Vector2 vector2 = Main.projectile[projectileLocalIndex2].Hitbox.ClosestPointInRect(self.Center);
                        self.chestX = (int)vector2.X / 16;
                        self.chestY = (int)vector2.Y / 16;
                        if (num3 < self.chestX - Player.tileRangeX || num3 > self.chestX + Player.tileRangeX + 1 || num4 < self.chestY - Player.tileRangeY || num4 > self.chestY + Player.tileRangeY + 1)
                        {
                            if (self.chest != -1)
                            {
                                SoundEngine.PlaySound(in SoundID.Item130);
                            }
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                    }
                }
                int MuleIndex = modPlayer.MULEInventoryProjTracker.ProjectileLocalIndex;
                if (MuleIndex >= 0)
                {
                    flag = true;
                    if (!Main.projectile[MuleIndex].active || Main.projectile[MuleIndex].type != ModContent.ProjectileType<MollyPetProjectile>())
                    {
                        SoundEngine.PlaySound(in SoundID.MenuClose);
                        self.chest = -1;
                        Recipe.FindRecipes();
                    }
                    else
                    {
                        int num3 = (int)(((double)self.position.X + (double)self.width * 0.5) / 16.0);
                        int num4 = (int)(((double)self.position.Y + (double)self.height * 0.5) / 16.0);
                        Vector2 vector2 = Main.projectile[MuleIndex].Hitbox.ClosestPointInRect(self.Center);
                        self.chestX = (int)vector2.X / 16;
                        self.chestY = (int)vector2.Y / 16;
                        if (num3 < self.chestX - Player.tileRangeX || num3 > self.chestX + Player.tileRangeX + 1 || num4 < self.chestY - Player.tileRangeY || num4 > self.chestY + Player.tileRangeY + 1)
                        {
                            if (self.chest != -1)
                            {
                                SoundEngine.PlaySound(in SoundID.MenuClose);
                            }
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                    }
                }
                if (flag)
                {
                    return;
                }
                if (!self.IsInInteractionRangeToMultiTileHitbox(self.chestX, self.chestY))
                {
                    if (self.chest != -1)
                    {
                        SoundEngine.PlaySound(in SoundID.MenuClose);
                    }
                    self.chest = -1;
                    Recipe.FindRecipes();
                }
                else if (!Main.tile[self.chestX, self.chestY].HasTile)
                {
                    SoundEngine.PlaySound(in SoundID.MenuClose);
                    self.chest = -1;
                    Recipe.FindRecipes();
                }
            }
            else
            {
                self.piggyBankProjTracker.Clear();
                self.voidLensChest.Clear();
            }
        }

        private bool On_Projectile_TryGetContainerIndex(On_Projectile.orig_TryGetContainerIndex orig, Projectile self, out int containerIndex)
        {
            return orig(self, out containerIndex);
        }
    }
}
