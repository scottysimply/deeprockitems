using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace deeprockitems.Utilities
{
    public static class DrawingDefaults
    {
        public static SpriteSortMode SpriteSortMode => SpriteSortMode.Deferred;
        public static BlendState BlendState => BlendState.AlphaBlend;
        public static SamplerState SamplerState => Main.DefaultSamplerState;
        //SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix
        /// <summary>
        /// Begins spritebatch with Terraria's defaults.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void BeginWithDefaults(this SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void BeginWithDefaultsForUI(this SpriteBatch spriteBatch, SpriteSortMode? sortMode = null, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? stencilState = null, RasterizerState? rasterizer = null, Effect? effect = null, Matrix? matrix = null) {
            spriteBatch.Begin(sortMode ?? SpriteSortMode.Deferred, blendState ?? BlendState.AlphaBlend, samplerState ?? SamplerState.PointClamp, stencilState ?? default, rasterizer ?? Main.Rasterizer, effect, matrix ?? Main.UIScaleMatrix);
        }
    }
}