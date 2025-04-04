using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace deeprockitems.Content.Upgrades
{
    public class Overclock : Upgrade
    {
        public Overclock(string internalName, Asset<Texture2D> sprite) : base(internalName, sprite) {
            
        }
        public LocalizedText Positives { get => Language.GetOrRegister($"{LocalizedKey}.Positives", () => "Positives"); }
        public LocalizedText Negatives { get => Language.GetOrRegister($"{LocalizedKey}.Negatives", () => "Negatives"); }
    }
}
