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
        private Vector2 _scale;
        private readonly SpriteEffects _defaultEffects;

        public int Width => (int)(_sourceRect.Width * _scale.X);
        public int Height => (int)(_sourceRect.Height * _scale.Y);
        public bool IsFinished => true;

        public StaticSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, float scale = 2.0f, SpriteEffects defaultEffects = SpriteEffects.None)
            : this(texture, sourceRect, position, new Vector2(scale, scale), defaultEffects) { }

        public StaticSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, Vector2 scale, SpriteEffects defaultEffects = SpriteEffects.None)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _position = position;
            _startPosition = position;
            _scale = scale;
            _defaultEffects = defaultEffects;
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

        public void Draw(SpriteBatch spriteBatch, SpriteEffects effects, float layerDepth = 0f)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                _sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                effects | _defaultEffects,
                layerDepth
            );
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public Vector2 GetSize()
        {
            return new Vector2(_sourceRect.Width * _scale.X, _sourceRect.Height * _scale.Y);
        }
    }
}
