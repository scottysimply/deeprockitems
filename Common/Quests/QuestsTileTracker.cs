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

            // Fixing all mismatches here
            int type2 = type;
            type2 = FixOres(i, j, type2);


            // Trying anything I can to make sure this player is the one destroying the tile.
            if (modPlayer.Player.IsInTileInteractionRange(i, j, Terraria.DataStructures.TileReachCheckSettings.Pylons))
            {
                // Check if quest is mining, and pertains to this tile
                if (modPlayer.CurrentQuestInformation[0] == 1 && type2 == modPlayer.CurrentQuestInformation[1] && !effectOnly && !fail)
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                }
            }


        }
        public int FixOres(int i, int j, int type)
        {
            switch (type)
            {
                case TileID.Tin:
                    type = TileID.Copper;
                    break;

                case TileID.Lead:
                    type = TileID.Iron;
                    break;

                case TileID.Tungsten:
                    type = TileID.Silver;
                    break;

                case TileID.Platinum:
                    type = TileID.Gold;
                    break;

                case TileID.Palladium:
                    type = TileID.Cobalt;
                    break;

                case TileID.Orichalcum:
                    type = TileID.Mythril;
                    break;

                case TileID.Titanium:
                    type = TileID.Adamantite;
                    break;

                default:
                    break;
            };
            return type;
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
