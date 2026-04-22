using HollowKnight.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Graphics
{
    public class HealthHud
    {
        private readonly Texture2D texture;
        private readonly Rectangle emptyMaskSource;

        public HealthHud(TextureAtlas atlas)
        {
            texture = atlas.Texture;
            emptyMaskSource = atlas.GetRegion("HealthMaskEmpty").SourceRectangle;
        }

        public void Draw(SpriteBatch spriteBatch, int currentHealth, int maxHealth)
        {
            int safeHealth = MathHelper.Clamp(currentHealth, 0, maxHealth);

            for (int i = 0; i < maxHealth; i++)
            {
                int x = HudConstants.HealthPipStartX + i * (HudConstants.HealthPipWidth + HudConstants.HealthPipSpacing);
                Rectangle pipTarget = new(x, HudConstants.HealthPipStartY, HudConstants.HealthPipWidth, HudConstants.HealthPipHeight);
                Color pipColor = i < safeHealth ? Color.White : HudConstants.InactiveHealthPipColor;
                spriteBatch.Draw(texture, pipTarget, emptyMaskSource, pipColor); // layerDepth inert: HUD batch uses default SpriteSortMode.Deferred
            }
        }
    }
}
