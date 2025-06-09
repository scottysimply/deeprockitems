using Microsoft.Xna.Framework;
using Terraria;

namespace deeprockitems.Localization
{
    public static class DRGText
    {
        public static Color PositiveText { get => new(0x4E, 0xC7, 0x26); }
        public static Color NegativeText { get => new(0xF1, 0x30, 0x10); }
        public static string TextColor(this string str, Color color) {
            string begin = $"[c/{color.Hex3()}:";
            string msg = begin;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ']')
                {
                    msg += str[i];
                    continue;
                }
                msg += $"]{begin}]";
            }
            msg += ']';
            return msg;
        }
    }
}
