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
        public enum OverclockType {
            Unset = 0,
            Clean = 1,
            Balanced = 2,
            Unstable = 3,
        }
        public Overclock(string internalName, Asset<Texture2D> sprite, OverclockType overclockType) : base(internalName, sprite) {
            _type = overclockType;
        }
        public OverclockType Type { get => _type; }
        private readonly OverclockType _type;
        public override Asset<Texture2D> Background
        {
            get
            {
                return _type switch {
                    OverclockType.Clean => Assets.Upgrades.Backgrounds.Clean,
                    OverclockType.Balanced => Assets.Upgrades.Backgrounds.Balanced,
                    OverclockType.Unstable => Assets.Upgrades.Backgrounds.Unstable,
                    _ => Assets.WhitePixel,
                };
            }
        }
        public LocalizedText Positives { get => Language.GetOrRegister($"{LocalizedKey}.Positives", () => "Positives"); }
        public LocalizedText Negatives { get => Language.GetOrRegister($"{LocalizedKey}.Negatives", () => "Negatives"); }
    }
}
