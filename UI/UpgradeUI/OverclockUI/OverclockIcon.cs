using deeprockitems.Content.Upgrades;
using deeprockitems.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    /// <summary>
    /// This UIElement's only purpose is to draw the background and icon of the overclock
    /// </summary>
    public class OverclockIcon : UIElement
    {
        Overclock _overclock;
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (_overclock is not null)
            {
                var dims = GetDimensions().ToRectangle();
                var overclockFrame = _overclock.Background.Frame(verticalFrames: 3, frameY: (int)_overclock.Type - 1);
                spriteBatch.Draw(_overclock.Background.Value, dims.Center.ToVector2() - 0.5f * overclockFrame.Size(), overclockFrame, Color.White, 0f, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(_overclock.Texture.Value, dims.Center.ToVector2() - 0.5f * _overclock.Texture.Size(), null, Color.White, 0f, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
                if (GetDimensions().ToRectangle().Contains(Main.MouseScreen.ToPoint()))
                {
                    string mouseText = $"[c/E3B465:{Language.GetTextValue("Mods.deeprockitems.Misc.UsefulWords.Overclock")}: {_overclock.DisplayName}]\n" +
                                       $"{_overclock.HoverText}";
                    if (_overclock.Positives.Value != _overclock.Positives.Key)
                    {
                        foreach (var line in _overclock.Positives.Value.Split('\n'))
                        {
                            mouseText += $"\n▲ {line}".TextColor(DRGText.PositiveText);
                        }
                    }
                    if (_overclock.Negatives.Value != _overclock.Negatives.Key)
                    {
                        foreach (var line in _overclock.Negatives.Value.Split('\n'))
                        {
                            mouseText += $"\n▼ {line}".TextColor(DRGText.NegativeText);
                        }
                    }
                    UICommon.TooltipMouseText(mouseText);
                }
            }
        }
        public void SetOverclock(Overclock overclock) {
            this._overclock = overclock;
        }
    }
}