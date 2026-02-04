using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;

namespace HollowKnight.Sprites
{
    /// <summary>
    /// A static sprite that moves vertically and bounces off screen edges.
    /// Use for: simple moving obstacles, floating platforms, patrolling enemies.
    /// </summary>
    public class MovingSprite : ISprite
    {
        private Texture2D _texture;
        private Rectangle _sourceRect;
        private Vector2 _position;
        private Vector2 _startPosition;
        private float _scale;
        private float _speed;
        private int _direction;
        private int _screenHeight;
        private int _spriteHeight;

        /// <summary>
        /// Create a vertically moving sprite.
        /// </summary>
        /// <param name="texture">The sprite sheet texture</param>
        /// <param name="sourceRect">Rectangle defining which part of the texture to draw</param>
        /// <param name="position">Initial position in world coordinates</param>
        /// <param name="screenHeight">Screen height for bounce detection</param>
        /// <param name="speed">Movement speed in pixels per second (default 150)</param>
        /// <param name="scale">Scale multiplier (default 2.0)</param>
        public MovingSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, int screenHeight, float speed = 150f, float scale = 2.0f)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _position = position;
            _startPosition = position;
            _speed = speed;
            _scale = scale;
            _direction = 1;
            _screenHeight = screenHeight;
            _spriteHeight = (int)(sourceRect.Height * scale);
        }

        public void Update(GameTime gameTime)
        {
            float delta = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position.Y += delta * _direction;

            // Bounce off top and bottom
            if (_position.Y + _spriteHeight >= _screenHeight)
            {
                _position.Y = _screenHeight - _spriteHeight;
                _direction = -1;
            }
            else if (_position.Y <= 0)
            {
                _position.Y = 0;
                _direction = 1;
            }
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

        public void Reset()
        {
            _position = _startPosition;
            _direction = 1;
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
