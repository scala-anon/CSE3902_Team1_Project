using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
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
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public float duration;
        public float _timer;

        float initialXVelocity = EnemyConstants.MantisProjectileXVelocity;
        float xAcceleration = EnemyConstants.MantisProjectileXAcceleration;
        float ySpeed = EnemyConstants.MantisProjectileYSpeed;
        

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
            EndPosition = new Vector2(position.X, position.Y + 100);
            

            duration = EnemyConstants.MantisProjectileDuration;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;        

            float localX = (initialXVelocity * _timer) + (EnemyConstants.half * xAcceleration * MathF.Pow(_timer,EnemyConstants.Power2));

            float localY = ySpeed * _timer;

            Position = new Vector2(StartPosition.X + localX, StartPosition.Y + localY);

            _currentSprite.SetPosition(Position);
            _currentSprite.Update(gameTime);

            if (_timer >= duration) Alive = false;
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
