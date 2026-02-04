using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;

namespace HollowKnight.Sprites
{
    /// <summary>
    /// A non-animated sprite that displays a single frame from a texture.
    /// Use for: static objects, UI elements, backgrounds, idle states.
    /// </summary>
    public class StaticSprite : ISprite
    {
        private Texture2D _texture;
        private Rectangle _sourceRect;
        private Vector2 _position;
        private Vector2 _startPosition;
        private float _scale;

        /// <summary>
        /// Create a static sprite from a sprite sheet.
        /// </summary>
        /// <param name="texture">The sprite sheet texture</param>
        /// <param name="sourceRect">Rectangle defining which part of the texture to draw</param>
        /// <param name="position">Initial position in world coordinates</param>
        /// <param name="scale">Scale multiplier (default 2.0)</param>
        public StaticSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, float scale = 2.0f)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _position = position;
            _startPosition = position;
            _scale = scale;
        }

        public void Update(GameTime gameTime)
        {
            // No update needed for static sprite
        }

        public void Reset()
        {
            _position = _startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                _sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                0f
            );
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}
