using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using deeprockitems.Common.Items;

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
                // Essentially, test for gems
                if (subID > -1 && Main.tile[i, j].TileType == TileID.ExposedGems && Main.tile[i, j].TileFrameX > 16 * subID && Main.tile[i, j].TileFrameX < 16 * (subID + 1) && !effectOnly && !fail)
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    noItem = true;
                    return;
                }
                else if (modPlayer.CurrentQuestInformation[1] == type && !effectOnly && !fail) // If the current quest tile is this tile, and the tile was mined, and the tile was killed.
                {
                    modPlayer.CurrentQuestInformation[3]--;
                    QuestsBase.DecrementProgress(modPlayer);
                    noItem = true;
                    return;
                }
            }
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
}
