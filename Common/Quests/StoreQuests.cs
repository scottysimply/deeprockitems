using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static deeprockitems.Common.Quests.QuestsBase;


namespace deeprockitems.Common.Quests
{
    public class StoreQuests : ModSystem
    {
        public override void SaveWorldData(TagCompound tag)
        {
            tag["CurrentQuest"] = CurrentQuest;
            tag["QuestProgress"] = Progress;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            // THIS IS IMPORTANT! This allows the mod to load worlds that don't contain the saved key!
            // If this wasn't here, the world would be unloadable if the key wasn't found (eg. install mod and load existing world)
            if (tag.ContainsKey("CurrentQuest") && tag.ContainsKey("QuestProgress"))
            {
                CurrentQuest = (int[])tag["CurrentQuest"];
                Progress = (int)tag["QuestProgress"];
            }
        }
        public override void SetStaticDefaults()
        {
            InitializeQuests();
        }
        public override void PostUpdateWorld()
        {
            // If it is morning, allow quest to be reset. Can only be reset at NPC.
            if (Main.dayTime && Main.time == 0)
            {
                CurrentQuest[0] = 0;
            }
            UpdateQuests();
        }
    }

}
