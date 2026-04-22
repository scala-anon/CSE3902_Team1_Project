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
        private bool _loop;
        private bool _finished;

        public int Width => (int)(_frames[_currentFrame].Width * _scale);
        public int Height => (int)(_frames[_currentFrame].Height * _scale);

        /// <summary>
        /// Create an animated sprite from a sprite sheet.
        /// </summary>
        /// <param name="texture">The sprite sheet texture</param>
        /// <param name="frames">Array of rectangles defining each animation frame</param>
        /// <param name="position">Initial position in world coordinates</param>
        /// <param name="frameInterval">Time in seconds between frames (default 0.15)</param>
        /// <param name="scale">Scale multiplier (default 2.0)</param>
        public bool IsFinished => _finished;

        public AnimatedSprite(Texture2D texture, Rectangle[] frames, Vector2 position, double frameInterval = 0.15, float scale = 2.0f, bool loop = true)
        {
            _texture = texture;
            _frames = frames;
            _position = position;
            _startPosition = position;
            _scale = scale;
            _currentFrame = 0;
            _frameTimer = 0;
            _frameInterval = frameInterval;
            _loop = loop;
            _finished = false;
        }

        public void Update(GameTime gameTime)
        {
            if (_finished) return;

            _frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTimer >= _frameInterval)
            {
                _currentFrame++;
                if (_currentFrame >= _frames.Length)
                {
                    if (_loop)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame = _frames.Length - 1;
                        _finished = true;
                    }
                }
                _frameTimer = 0;
            }
        }

        public void Reset()
        {
            _position = _startPosition;
            _currentFrame = 0;
            _frameTimer = 0;
            _finished = false;
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
                _frames[_currentFrame],
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                effects,
                layerDepth
            );
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public Vector2 GetSize()
        {
            
            Rectangle currentFrame = _frames[_currentFrame];
            return new Vector2(currentFrame.Width * _scale, currentFrame.Height * _scale);
        }
    }
}
