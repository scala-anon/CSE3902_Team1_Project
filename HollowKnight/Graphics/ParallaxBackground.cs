using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Graphics
{
    public class ParallaxBackground
    {
        private readonly Texture2D _texture;
        private const float ParallaxFactor = 0.2f; // 5x slower than camera
        private const float Scale = 4.0f;           // large enough to cover screen at max camera travel

        public ParallaxBackground(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, int screenWidth, int screenHeight, Color tint)
        {
            int w = (int)(_texture.Width  * Scale);
            int h = (int)(_texture.Height * Scale);

            int drawX = (screenWidth  - w) / 2 - (int)(cameraPosition.X * ParallaxFactor);
            int drawY = (screenHeight - h) / 2 - (int)(cameraPosition.Y * ParallaxFactor);

            drawX = Math.Min(drawX, 0);
            drawX = Math.Max(drawX, screenWidth  - w);
            drawY = Math.Min(drawY, 0);
            drawY = Math.Max(drawY, screenHeight - h);

            spriteBatch.Draw(_texture, new Rectangle(drawX, drawY, w, h), tint);
        }
    }
}
