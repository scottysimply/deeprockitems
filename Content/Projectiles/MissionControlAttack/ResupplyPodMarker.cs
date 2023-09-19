using deeprockitems.Content.NPCs.MissionControl;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

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
            Projectile.hide = true;
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
            Main.NewText("Projectile spawned");
            // The projectile is valid. Let it live.
            if (source is EntitySource_Parent { Entity: NPC { ModNPC: MissionControl missionControl} } && (Main.myPlayer == 255 || Main.netMode == 0))
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active && npc.chaseable && !npc.friendly && npc.Distance(missionControl.NPC.Center) < NPCID.Sets.DangerDetectRange[missionControl.Type])
                    {
                        target = npc; 
                        break;
                    }
                }
                predicted_target = NPC.NewNPCDirect(source, target.position, target.type);
                Main.NewText($"Targetting NPC {predicted_target.TypeName} at position {predicted_target.Center.X}, {predicted_target.Center.Y}");
                predicted_target.active = false;
                for (int i = 0; i < 60; i++)
                {
                    predicted_target.AI();
                }
                Main.NewText($"NPC predicted at position {predicted_target.Center.X}, {predicted_target.Center.Y}");
                Projectile.Center = predicted_target.Center;
                predicted_target.StrikeInstantKill();
                for (int i = 0; i < 30; i++)
                {
                    if (Main.tile[Projectile.Center.ToTileCoordinates().X, Projectile.Center.ToTileCoordinates().Y + i].HasUnactuatedTile)
                    {
                        Projectile.Center = Projectile.Center + new Vector2(0, i * 16);
                        break;
                    }
                }
                Projectile.NewProjectile(source, Projectile.Center + new Vector2(0, Main.screenHeight), new Vector2(0, 10), ModContent.ProjectileType<ResupplyPodDrills>(), 500, 0, ai0: Projectile.Center.Y);
                Projectile.hide = false;
                return;
            }
            Projectile.Kill();
        }
        public override void AI()
        {
            if (Projectile.frameCounter++ % 10 == 0)
            {
                Projectile.frame = ++Projectile.frame % 6;
            }
        }
    }
}
