using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;

namespace HollowKnight.Sprites
{
    /// <summary>
    /// A sprite that renders text using a SpriteFont.
    /// Use for: UI labels, damage numbers, dialogue, credits.
    /// </summary>
    public class TextSprite : ISprite
    {
        private SpriteFont _font;
        private string _text;
        private Vector2 _position;
        private Vector2 _startPosition;
        private Color _color;

        /// <summary>
        /// Create a text sprite.
        /// </summary>
        /// <param name="font">The SpriteFont to use for rendering</param>
        /// <param name="text">The text to display</param>
        /// <param name="position">Position in screen coordinates</param>
        /// <param name="color">Text color</param>
        public TextSprite(SpriteFont font, string text, Vector2 position, Color color)
        {
            _font = font;
            _text = text;
            _position = position;
            _startPosition = position;
            _color = color;
        }

        public void Update(GameTime gameTime)
        {
            // No update needed for static text
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, _position, _color);
        }

        public void Reset()
        {
            _position = _startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}
