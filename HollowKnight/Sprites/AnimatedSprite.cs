using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;

namespace HollowKnight.Sprites
{
    /// <summary>
    /// A sprite that cycles through animation frames.
    /// Use for: walking animations, attack animations, environmental effects.
    /// </summary>
    public class AnimatedSprite : ISprite
    {
        private Texture2D _texture;
        private Rectangle[] _frames;
        private Vector2 _position;
        private Vector2 _startPosition;
        private float _scale;
        private int _currentFrame;
        private double _frameTimer;
        private double _frameInterval;

        /// <summary>
        /// Create an animated sprite from a sprite sheet.
        /// </summary>
        /// <param name="texture">The sprite sheet texture</param>
        /// <param name="frames">Array of rectangles defining each animation frame</param>
        /// <param name="position">Initial position in world coordinates</param>
        /// <param name="frameInterval">Time in seconds between frames (default 0.15)</param>
        /// <param name="scale">Scale multiplier (default 2.0)</param>
        public AnimatedSprite(Texture2D texture, Rectangle[] frames, Vector2 position, double frameInterval = 0.15, float scale = 2.0f)
        {
            _texture = texture;
            _frames = frames;
            _position = position;
            _startPosition = position;
            _scale = scale;
            _currentFrame = 0;
            _frameTimer = 0;
            _frameInterval = frameInterval;
        }

        public void Update(GameTime gameTime)
        {
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

        public void Reset()
        {
            _position = _startPosition;
            _currentFrame = 0;
            _frameTimer = 0;
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
                _frames[_currentFrame],
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
