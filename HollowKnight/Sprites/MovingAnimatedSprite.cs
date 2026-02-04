using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;

namespace HollowKnight.Sprites
{
    /// <summary>
    /// A sprite that animates while moving horizontally and bouncing off screen edges.
    /// Use for: walking enemies, patrolling NPCs, moving platforms with effects.
    /// </summary>
    public class MovingAnimatedSprite : ISprite
    {
        private Texture2D _texture;
        private Rectangle[] _frames;
        private Vector2 _position;
        private Vector2 _startPosition;
        private float _scale;
        private float _speed;
        private int _direction;
        private int _screenWidth;
        private int _spriteWidth;
        private int _currentFrame;
        private double _frameTimer;
        private double _frameInterval;

        /// <summary>
        /// Create a horizontally moving animated sprite.
        /// </summary>
        /// <param name="texture">The sprite sheet texture</param>
        /// <param name="frames">Array of rectangles defining each animation frame</param>
        /// <param name="position">Initial position in world coordinates</param>
        /// <param name="screenWidth">Screen width for bounce detection</param>
        /// <param name="speed">Movement speed in pixels per second (default 150)</param>
        /// <param name="frameInterval">Time in seconds between frames (default 0.15)</param>
        /// <param name="scale">Scale multiplier (default 2.0)</param>
        public MovingAnimatedSprite(Texture2D texture, Rectangle[] frames, Vector2 position, int screenWidth, float speed = 150f, double frameInterval = 0.15, float scale = 2.0f)
        {
            _texture = texture;
            _frames = frames;
            _position = position;
            _startPosition = position;
            _speed = speed;
            _scale = scale;
            _direction = 1;
            _screenWidth = screenWidth;
            _spriteWidth = (int)(frames[0].Width * scale);
            _currentFrame = 0;
            _frameTimer = 0;
            _frameInterval = frameInterval;
        }

        public void Update(GameTime gameTime)
        {
            float delta = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position.X += delta * _direction;

            // Bounce off left and right
            if (_position.X + _spriteWidth >= _screenWidth)
            {
                _position.X = _screenWidth - _spriteWidth;
                _direction = -1;
            }
            else if (_position.X <= 0)
            {
                _position.X = 0;
                _direction = 1;
            }

            // Animation
            _frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTimer >= _frameInterval)
            {
                _currentFrame++;
                if (_currentFrame >= _frames.Length)
                {
                    _currentFrame = 0;
                }
                _frameTimer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                _frames[_currentFrame],
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
            _currentFrame = 0;
            _frameTimer = 0;
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
