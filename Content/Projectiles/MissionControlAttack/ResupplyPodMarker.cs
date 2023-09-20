using deeprockitems.Content.NPCs.MissionControl;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using deeprockitems.Utilities;
using System;

namespace deeprockitems.Content.Projectiles.MissionControlAttack
{
    public class ResupplyPodMarker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 12;
            Projectile.velocity = Vector2.Zero;
            Projectile.aiStyle = -1;
            Projectile.hide = false;
            Projectile.timeLeft = 360;
            Main.projFrames[Type] = 1;
        }
        private NPC target;
        private NPC predicted_target;
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // The projectile is valid. Let it live.
            if (source is EntitySource_Parent { Entity: NPC { ModNPC: MissionControl missionControl} } && (Main.myPlayer == 255 || Main.netMode == 0))
            {
                // Try to find the closest, valid NPC. If none exists, then this code isn't ran.
                foreach (NPC npc in Main.npc)
                {
                    // If NPC is a valid NPC (in range, hostile, targettable by minions)
                    if (npc.active && npc.chaseable && !npc.friendly && npc.Distance(missionControl.NPC.Center) < NPCID.Sets.DangerDetectRange[missionControl.Type])
                    {
                        target = npc; 
                        break;
                    }
                }
                // This is how we tell if we got our target.
                if (target is not null)
                {

                    // Determine if we can even make a copy. If there's no space in Main.npc, then we fail this check as well.
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        // If there's an empty slot:
                        if (!Main.npc[i].active)
                        {
                            // Clone our NPC to that slot.
                            predicted_target = target.Clone() as NPC; // This is making a copy of our target--we don't want to affect them. Only predict...
                            predicted_target.whoAmI = i;
                            Main.npc[i] = predicted_target;
                            break;
                        }
                        // If no match found:
                        if (i == Main.npc.Length - 1)
                        {
                            Projectile.Kill(); // Kill our projectile.
                        }
                    }

                    // This is the prediction code... Yeah.
                    for (int i = 0; i < 135; i++)
                    {
                        predicted_target.UpdateNPC(predicted_target.whoAmI); // Quite literally just calling its AI 2 seconds into the future. This is also dependent on if the player doesn't move at all.
                    }
                    // Teleport the projectile base onto our target
                    Projectile.Center = predicted_target.Center;
                    Point tilepos = Projectile.Center.ToTileCoordinates();
                    // Try snapping the base downward onto the ground.
                    for (int i = 0; i < 30; i++)
                    {
                        // Convert to tile below
                        if (WorldGen.SolidTile(new Point(tilepos.X, tilepos.Y + i)) && !WorldGen.SolidTile(tilepos))
                        {
                            // Snap tile
                            Projectile.Center = Projectile.Center + new Vector2(0, (i - 1) * 16 + 4);
                            break;
                        }
                    }
                    // Spawn the drill above the NPC.
                    Projectile.NewProjectile(source, predicted_target.Center - new Vector2(0, Main.screenHeight), new Vector2(0, 10), ModContent.ProjectileType<ResupplyPodDrills>(), 500, 0, ai0: Projectile.Center.Y);
                    Projectile.hide = false; // Show projectile if not already shown.
                    Projectile.scale = 1.5f;
                    predicted_target.active = false; // Deactive our "helper" target, they're unneeded.
                    return;
                }
               
            }
            Projectile.Kill();
        }
        public override void AI()
        {
            if (Projectile.frameCounter++ % 20 == 0)
            {
                Projectile.frame = ++Projectile.frame % 6;
            }
            if (Projectile.frameCounter > 180)
            {
                Projectile.Kill();
            }
        }
        private readonly Texture2D arrows = ModContent.Request<Texture2D>("deeprockitems/Content/Projectiles/MissionControlAttack/ResupplyPodMarkerArrows").Value;
        Vector2 drawPos;
        public override bool PreDraw(ref Color lightColor)
        {
            Rectangle frame = new Rectangle(0, Projectile.frame * arrows.Height / 6, arrows.Width, arrows.Height / 6);
            drawPos = new Vector2(Projectile.position.X + 4, Projectile.position.Y - (arrows.Height / 6) + 10) - Main.screenPosition; // Gonna need this
            Main.EntitySpriteDraw(arrows, drawPos, frame, new Color(new Vector4(.8f)), 0, new Vector2(0, 0), 1f, SpriteEffects.None);
            return true;
        }
    }
}
