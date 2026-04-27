using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Graphics
{
    public class ScreenFader
    {
        private enum State { None, FadingOut, FadingIn }
        private State _state = State.None;
        private float _alpha = 0f;
        private const float Speed = 3f;
        private Action _onBlack;

        public bool IsActive => _state != State.None;

        public void StartFadeOut(Action onBlack)
        {
            _onBlack = onBlack;
            _state = State.FadingOut;
            _alpha = 0f;
        }

        public void Update(float dt)
        {
            if (_state == State.FadingOut)
            {
                _alpha = Math.Min(1f, _alpha + Speed * dt);
                if (_alpha >= 1f)
                {
                    _onBlack?.Invoke();
                    _onBlack = null;
                    _state = State.FadingIn;
                }
            }
            else if (_state == State.FadingIn)
            {
                _alpha = Math.Max(0f, _alpha - Speed * dt);
                if (_alpha <= 0f)
                    _state = State.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, Rectangle bounds)
        {
            if (_alpha <= 0f) return;
            spriteBatch.Draw(pixel, bounds, Color.Black * _alpha);
        }

        public void Reset()
        {
            _state = State.None;
            _alpha = 0f;
            _onBlack = null;
        }
    }
}
