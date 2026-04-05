using HollowKnight.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Graphics
{
    public class HealthHud
    {
        private readonly Texture2D texture;
        private readonly Rectangle portraitSource;
        private readonly Rectangle emptyMaskSource;
        private readonly Rectangle fullMaskSource;

        private const int FullMaskYOffset = 1;
        private const int PortraitWidth = 102;
        private const int PortraitHeight = 98;
        private const int MaskWidth = 22;
        private const int MaskHeight = 32;
        private const int FullMaskWidth = 21;
        private const int FullMaskHeight = 32;
        private const int PortraitX = 14;
        private const int PortraitY = 22;
        private const int MaskStartX = 118;
        private const int MaskStartY = 28;
        private const int MaskSpacing = 14;

        private static readonly Color HudInactiveMaskColor = new(28, 32, 44, 255);
        private static readonly Color HudZeroHealthPortraitColor = new(28, 32, 44, 255);

        public HealthHud(TextureAtlas atlas)
        {
            texture = atlas.Texture;
            portraitSource = atlas.GetRegion("HudPortrait").SourceRectangle;
            emptyMaskSource = atlas.GetRegion("HealthMaskEmpty").SourceRectangle;
            fullMaskSource = atlas.GetRegion("HealthMaskFull").SourceRectangle;
        }

        public void Draw(SpriteBatch spriteBatch, int currentHealth, int maxHealth)
        {
            int safeHealth = MathHelper.Clamp(currentHealth, 0, maxHealth);
            Color portraitTint = safeHealth == 0 ? HudZeroHealthPortraitColor : Color.White;
            spriteBatch.Draw(
                texture,
                new Rectangle(PortraitX, PortraitY, PortraitWidth, PortraitHeight),
                portraitSource,
                portraitTint);

            for (int i = 0; i < maxHealth; i++)
            {
                int x = MaskStartX + i * (MaskWidth + MaskSpacing);
                if (i < safeHealth)
                {
                    Rectangle fullTarget = new(x, MaskStartY + FullMaskYOffset, FullMaskWidth, FullMaskHeight);
                    spriteBatch.Draw(texture, fullTarget, fullMaskSource, Color.White);
                }
                else
                {
                    Rectangle emptyTarget = new(x, MaskStartY, MaskWidth, MaskHeight);
                    spriteBatch.Draw(texture, emptyTarget, emptyMaskSource, HudInactiveMaskColor);
                }
            }
        }
    }
}
