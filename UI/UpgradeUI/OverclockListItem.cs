using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI
{
    public class OverclockListItem : UpgradeSelectOption
    {
        private Overclock _internalOverclock;
        public OverclockListItem(UpgradeTier tier, Overclock overclock) : base(tier, overclock) {
            _internalOverclock = overclock;
        }
        public override void OnInitialize() {
        }
        public override void LeftClick(UIMouseEvent evt) {
            base.LeftClick(evt);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            float bgScale = _internalOverclock.Type switch {
                Overclock.OverclockType.Clean => 1.15f,
                _ => 1f
            };
            spriteBatch.Draw(backgroundImage.Value, (Rectangle)ScaledDimensions.Scale(bgScale), Color.White);

            //float scale = 1f;
            //Vector2 destination = new(ScaledDimensions.Center.X - icon.Value.Width * 0.5f, ScaledDimensions.Center.Y - icon.Value.Height * 0.5f);
            //spriteBatch.Draw(icon.Value, null, Color.White, 0f, ScaledDimensions.Center, 1f, SpriteEffects.None, 0);
            float scale = 1f / bgScale;
            spriteBatch.Draw(icon.Value, (Rectangle)ScaledDimensions.Scale(scale), Color.White);
            HandleTweening();

            if (ContainsPoint(Main.MouseScreen)) {
                UICommon.TooltipMouseText($"[c/E3B465:{_internalOverclock.DisplayName}]\n" +
                                          $"{_internalOverclock.HoverText}");
            }
        }
    }
}
