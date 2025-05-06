using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
                spriteBatch.Draw(_overclock.Background.Value, dims.Center.ToVector2(), Color.White);
                spriteBatch.Draw(_overclock.Texture.Value, dims.Center.ToVector2(), Color.White);
            }
        }
        public void SetOverclock(Overclock overclock) {
            this._overclock = overclock;
        }
    }
}
