using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deeprockitems.Common.Quests
{
    public class QuestsTileTracker : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // Convert our player to a ModPlayer
            DRGQuestsModPlayer modPlayer = Main.LocalPlayer.GetModPlayer<DRGQuestsModPlayer>();
            if (modPlayer is null) return; // Return if null.


            // If quest type is mining and player is in interaction range:
            if (modPlayer.CurrentQuestInformation[0] == 1 && modPlayer.Player.IsInTileInteractionRange(i, j, Terraria.DataStructures.TileReachCheckSettings.Pylons))
            {
                // Fixing all gem migraines in this section of code
                int subID = FixGems(i, j, modPlayer.CurrentQuestInformation[1]);
                if (subID > -1 && Main.tile[i, j].TileType == TileID.ExposedGems && Main.tile[i, j].TileFrameX > 16 * subID && Main.tile[i, j].TileFrameX < 16 * (subID + 1) && !effectOnly && !fail)
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    return;
                }
                else if (modPlayer.CurrentQuestInformation[1] == type && !effectOnly && !fail) // Well. It wasn't the gem, or at least, not the correct one. Continue as normal
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    return;
                }
                

            }
            /*// Trying anything I can to make sure this player is the one destroying the tile.
            if (modPlayer.Player.IsInTileInteractionRange(i, j, Terraria.DataStructures.TileReachCheckSettings.Pylons))
            {
                // Check if quest is mining, and pertains to this tile
                if (modPlayer.CurrentQuestInformation[0] == 1 && type2 == modPlayer.CurrentQuestInformation[1] && !effectOnly && !fail)
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                }
            }*/


        }
        private static int FixGems(int i, int j, int questType)
        {
            return (questType) switch
            {
                TileID.Amethyst => 0,
                TileID.Topaz => 1,
                TileID.Sapphire => 2,
                TileID.Emerald=> 3,
                TileID.Ruby => 4,
                TileID.Diamond => 5,
                TileID.AmberStoneBlock => 6,
                _ => -1,
            };
        }
    }
   /* public class MiningDetour : ModSystem
    {
        public override void Load()
        {
            Terraria.Achievements
        }
        private void On_Main_DrawInterface_36_Cursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            if (Main.cursorOverride != 30)
            {
                orig(); // Call original method
            }
            else
            {
                Main.spriteBatch.End();
                Texture2D cursor = ModContent.Request<Texture2D>("deeprockitems/UI/UpgradeCursor").Value;

                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                Main.spriteBatch.Draw(cursor, Main.MouseScreen, null, Color.White, 0, new Vector2(.1f) * cursor.Size(), Main.cursorScale * 1.1f, SpriteEffects.None, 0f);
            }

        }
    }*/
}
