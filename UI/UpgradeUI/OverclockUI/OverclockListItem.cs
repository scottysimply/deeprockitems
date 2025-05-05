using deeprockitems.Content.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace deeprockitems.UI.UpgradeUI.OverclockUI
{
    public class OverclockListItem : UpgradeSelectOption
    {
        private Overclock _internalOverclock;
        public OverclockListItem(UpgradeTier tier, Overclock overclock) : base(tier, overclock) {
            _internalOverclock = overclock;
        }
        public new bool IsMouseHovering { get => base.IsMouseHovering && Parent.Parent.GetDimensions().ToRectangle().Contains((Rectangle)ScaledDimensions); }
        public override void OnInitialize() {
            OverflowHidden = false;
        }
        public override void LeftClick(UIMouseEvent evt) {
            SelectThisUpgrade();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            int frame = _internalOverclock.Type switch {
                Overclock.OverclockType.Balanced => 1,
                Overclock.OverclockType.Unstable => 2,
                _ => 0
            };
            Rectangle bgBounds = new Rectangle(0, frame * 48, 48, 48);
            // draws background
            spriteBatch.Draw(backgroundImage.Value, (Rectangle)ScaledDimensions, bgBounds, Color.White);

            // draws icon
            float scale = _internalOverclock.Type switch {
                Overclock.OverclockType.Clean => 0.75f,
                _ => 1f
            };
            spriteBatch.Draw(icon.Value, (Rectangle)ScaledDimensions.Scale(scale), Color.White);
            HandleTweening();
            if (_internalOverclock.UpgradeState.IsEquipped)
            {
                spriteBatch.Draw(Assets.Upgrades.Overclocks.Outlines.Value, (Rectangle)ScaledDimensions, bgBounds, Color.Yellow);
            }


            if (IsMouseHovering)
            {
                UICommon.TooltipMouseText($"[c/E3B465:{_internalOverclock.DisplayName}]\n" +
                                          $"{_internalOverclock.HoverText}");
            }
        }
    }
}
