using deeprockitems.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Items.Beers
{
    public abstract class BaseBeer : ModItem
    {
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.Ale);
            Item.width = 24;
            Item.height = 18;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.buffTime = 10;
            Item.buffType = 0;
            SetBuff();
        }
        public abstract void SetBuff();
        public abstract class BuffForBeer : InstancedBuff {
            public bool BuffLastsAllDay { get; set; } = true;
            private bool _oldDay = Main.IsItDay();
            public override void Update(Player player, ref int buffIndex) {
                //Main.buffNoTimeDisplay[buffIndex] = true;
                if (BuffLastsAllDay)
                {
                    TimeLeft = 10;
                }
                // If it just became day, remove buff
                bool newDay = Main.IsItDay();
                if (!_oldDay && newDay)
                {
                    player.DelBuff(buffIndex);
                    buffIndex--;
                    return;
                }
                _oldDay = newDay;
            }
        }
    }
}
