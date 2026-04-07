using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Graphics
{
    public class SoulHud
    {
        private readonly Texture2D texture;
        private readonly Rectangle gaugeSource;

        public SoulHud(TextureAtlas atlas)
        {
            texture = atlas.Texture;
            gaugeSource = atlas.GetRegion("HudPortrait").SourceRectangle;
        }

        public void Draw(SpriteBatch spriteBatch, int currentSoul, int maxSoul)
        {
            float fillRatio = maxSoul <= 0 ? 0f : MathHelper.Clamp(currentSoul / (float)maxSoul, 0f, 1f);
            Rectangle gaugeTarget = new(HudConstants.SoulGaugeX, HudConstants.SoulGaugeY, HudConstants.SoulGaugeWidth, HudConstants.SoulGaugeHeight);
            Color soulColor = Color.Lerp(HudConstants.SoulGaugeMinColor, Color.White, fillRatio);
            spriteBatch.Draw(texture, gaugeTarget, gaugeSource, soulColor);
        }
    }
}
