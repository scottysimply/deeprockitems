using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace deeprockitems.Common.Quests
{
    public class QuestsTileTracker : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

            // Fixing all mismatches here
            int type2 = type;
            type2 = FixOres(i, j, type2);

            // Check if quest is mining, and pertains to this tile
            if (QuestsBase.CurrentQuest[0] == 1 && type2 == QuestsBase.CurrentQuest[1] && !effectOnly && !fail)
            {
                QuestsBase.Progress--;
                QuestsBase.DecrementProgress();
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
}
