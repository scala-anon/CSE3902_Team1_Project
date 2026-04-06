using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Graphics
{
    public class SoulHud
    {
        private readonly Texture2D texture;
        private readonly Rectangle gaugeSource;

        private const int GaugeWidth = 34;
        private const int GaugeHeight = 34;
        private const int GaugeX = 78;
        private const int GaugeY = 72;

        private static readonly Color SoulEmptyColor = new(28, 32, 44, 255);

        public SoulHud(TextureAtlas atlas)
        {
            texture = atlas.Texture;
            gaugeSource = atlas.GetRegion("HudPortrait").SourceRectangle;
        }

        public void Draw(SpriteBatch spriteBatch, int currentSoul, int maxSoul)
        {
            float fillRatio = maxSoul <= 0 ? 0f : MathHelper.Clamp(currentSoul / (float)maxSoul, 0f, 1f);
            Rectangle gaugeTarget = new(GaugeX, GaugeY, GaugeWidth, GaugeHeight);

            spriteBatch.Draw(texture, gaugeTarget, gaugeSource, SoulEmptyColor);
            DrawSoulFill(spriteBatch, gaugeTarget, fillRatio);
        }

        private void DrawSoulFill(SpriteBatch spriteBatch, Rectangle gaugeTarget, float fillRatio)
        {
            int fillPixels = (int)Math.Round(gaugeTarget.Height * fillRatio);
            if (fillPixels <= 0 || gaugeTarget.Height <= 0)
            {
                return;
            }

            int sourceFillHeight = (int)Math.Round(gaugeSource.Height * fillRatio);
            Rectangle filledSource = new(
                gaugeSource.X,
                gaugeSource.Bottom - sourceFillHeight,
                gaugeSource.Width,
                sourceFillHeight);
            Rectangle filledTarget = new(
                gaugeTarget.X,
                gaugeTarget.Bottom - fillPixels,
                gaugeTarget.Width,
                fillPixels);

            spriteBatch.Draw(texture, filledTarget, filledSource, Color.White);
        }
    }
}
