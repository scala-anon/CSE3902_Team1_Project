using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using System;
using HollowKnight.Audio;
using Microsoft.Xna.Framework.Audio;


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

        public float initialXVelocity = EnemyConstants.MantisProjectile2XVelocity;
        public float xAcceleration = EnemyConstants.MantisProjectile2XAcceleration;
        public float ySpeed = EnemyConstants.MantisProjectile2YSpeed;

        public float AudioCount = 0;
        public bool AudioPlaying = false;
        public SoundEffectInstance _soundEffect;
        

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

            if (_facing == Direction.Left)
            {
                Position = new Vector2(StartPosition.X + -localX, StartPosition.Y + localY);
                
            } else
            {
                Position = new Vector2(StartPosition.X + localX, StartPosition.Y + localY);
            }

            

            _currentSprite.SetPosition(Position);
            _currentSprite.Update(gameTime);

            if (_timer >= duration) Alive = false;


            AudioCount++;
            if (AudioPlaying == false)
            {
                _soundEffect = AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Mantis_Projectile());
                AudioPlaying = true;
            } else if (AudioCount % 120 == 0)
            {
                _soundEffect = AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Mantis_Projectile());
            }

            if (Alive == false) {
                AudioManager.Instance.StopSoundEffect(_soundEffect);
                AudioPlaying = false;
            }

        }

        public override void Draw(SpriteBatch spriteBatch, Direction facing, float layerDepth = 0f)
        {
            if (!Alive) return;
            if (_state == State.Moving && !HasMoved) return;

            SpriteEffects effects = SpriteEffects.None;
            

            _currentSprite.Draw(spriteBatch, effects, layerDepth);
        }

    }
}
