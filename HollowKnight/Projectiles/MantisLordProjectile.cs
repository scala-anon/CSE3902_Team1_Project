using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using System.Formats.Tar;
using System;

namespace HollowKnight.Projectiles
{
    public class MantisLordsProjectile : Projectile
    {
        private enum State { Moving }
        private State _state = State.Moving;
        private ISprite _currentSprite;
        private Direction _facing;
        public override bool PiercesEnemies => true;
        private float _distanceX = 0f;
        //y = a(x - h)^2 + k
        private float _a = .2f;
        private float _h = 200f;
        private float _k = -100f;
        public float _speed = EnemyConstants.EnemyProjectileSpeed;
        public Vector2 StartPosition;
        public Vector2 CurrentPosition;

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        public MantisLordsProjectile(Vector2 position, Vector2 velocity, ProjectileFaction faction)
            : base(position, velocity, faction)
        {
            _facing = velocity.X >= 0 ? Direction.Right : Direction.Left;
            _currentSprite = SpriteFactory.Instance.CreateMantisProjectile(position);

            Vector2 size = _currentSprite.GetSize();
            Width = (int)size.X;
            Height = (int)size.Y;

           
            Position.Y -= Height / 2f;
            _currentSprite.SetPosition(Position);

            StartPosition = position;
            CurrentPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _distanceX += _speed * dt;

            float relativeY = _a * MathF.Pow(_distanceX - _h, 2) + _k;

            float initialY = _a * MathF.Pow(0 - _h, 2) + _k;
            float finalYOffset = relativeY - initialY;

            CurrentPosition.X = StartPosition.X + _distanceX;
            CurrentPosition.Y = StartPosition.Y + finalYOffset;
            
            _currentSprite.SetPosition(Position);
            _currentSprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Direction facing, float layerDepth = 0f)
        {
            if (!Alive) return;
            if (_state == State.Moving && !HasMoved) return;

            SpriteEffects effects = SpriteEffects.None;
            //if (_state == State.Moving)
            //{
            //    effects = _facing == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //}
            //else
            //{
            //    // If moving left, use right-facing explosion. If moving right, use left-facing explosion.
            //    effects = _facing == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //}

            _currentSprite.Draw(spriteBatch, effects, layerDepth);
        }

    }
}
