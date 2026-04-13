using System;
using System.Collections.Generic;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Enemies
{
    public abstract class BaseEnemy : IEnemy
    {
        protected Rectangle[] hitBoxes = new Rectangle[1];

        public int Health { get; set; } = EnemyConstants.EnemyDefaultHealth;
        public bool IsDamaged => _isDamaged;
        public Direction FacingDirection { get; set; } = Direction.Left;
        public ISprite Sprite { get; protected set; }
        public Vector2 position;

        protected bool _isDamaged;
        protected double _damagedTimer;
        protected Vector2 _knockbackVelocity;
        public bool IsGrounded { get; protected set; }

        public Vector2 knightPosition = new Vector2(
            GameConstants.InvalidPositionSentinel, GameConstants.InvalidPositionSentinel);

        public abstract bool IsActive { get; }
        public Rectangle Bounds => new Rectangle(
            (int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);

        // Helpers to avoid deep access chains (e.g. enemy.Sprite.GetSize().X)
        public Vector2 SpriteSize => Sprite.GetSize();
        public void UpdateSpritePosition() => Sprite.SetPosition(position);
        public Vector2 GetCenter() => new Vector2(Bounds.Center.X, Bounds.Center.Y);

        // --- Subclass hooks ---
        protected abstract void ApplyKnockback(CollisionSide side);
        protected abstract void OnDeath(bool grounded);
        protected abstract void OnHealthChanged();
        protected abstract void UpdateAlive(GameTime gameTime, float dt);

        // --- IEnemy defaults (override as needed) ---
        public virtual void SetKnightPosition(Vector2 pos) => knightPosition = pos;
        public virtual void SetNavigationGrid(NavigationGrid grid) { }
        public virtual List<Vector2> GetCurrentPath() => null;
        public virtual float GetDetectionRadius() => 0f;
        public virtual float GetChaseRadius() => 0f;
        public virtual void SetPlatform(IObject platform) { }
        public abstract string GetStateName();

        // --- Shared implementations ---

        public virtual bool TakeDamage() => TakeDamage(CollisionSide.None);

        public virtual bool TakeDamage(CollisionSide side)
        {
            if (_isDamaged) return false;
            _isDamaged = true;
            _damagedTimer = 0;
            ApplyKnockback(side);
            OnHealthChanged();
            return true;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            SpriteEffects effects = FacingDirection == Direction.Right
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;
            Sprite.Draw(spriteBatch, effects);
        }

        public Rectangle[] GetBounds()
        {
            Vector2 size = Sprite.GetSize();
            hitBoxes[0] = new Rectangle(
                (int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            return hitBoxes;
        }

        public Rectangle GetHurtbox()
        {
            Rectangle[] bounds = GetBounds();
            bounds[0].Inflate(EnemyConstants.EnemyHurtboxGrow, EnemyConstants.EnemyHurtboxGrow);
            return bounds[0];
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!IsActive)
            {
                UpdateDead(dt);
                Sprite.SetPosition(position);
                Sprite.Update(gameTime);
                return;
            }

            UpdateDamageTimer(dt);
            UpdateKnockbackDecay(dt);
            UpdateAlive(gameTime, dt);
            Sprite.SetPosition(position);
            Sprite.Update(gameTime);
        }

        protected virtual void UpdateDead(float dt)
        {
            if (IsGrounded) return;

            _knockbackVelocity.Y += EnemyConstants.EnemyDeathGravity * dt;
            _knockbackVelocity.X *= (1f - EnemyConstants.EnemyKnockbackDecay * dt);
            if (Math.Abs(_knockbackVelocity.X) < EnemyConstants.EnemyKnockbackStopThreshold)
                _knockbackVelocity.X = 0;

            position += _knockbackVelocity * dt;

            float spriteHeight = Sprite.GetSize().Y;
            if (position.Y + spriteHeight >= GameConstants.ScreenHeight)
            {
                position.Y = GameConstants.ScreenHeight - spriteHeight;
                _knockbackVelocity = Vector2.Zero;
                IsGrounded = true;
                OnDeath(true);
            }
        }

        protected void UpdateDamageTimer(float dt)
        {
            if (!_isDamaged) return;
            _damagedTimer += dt;
            if (_damagedTimer >= EnemyConstants.EnemyDamagedDuration)
            {
                _isDamaged = false;
                _damagedTimer = 0;
            }
        }

        protected void UpdateKnockbackDecay(float dt)
        {
            if (_knockbackVelocity == Vector2.Zero) return;
            position += _knockbackVelocity * dt;
            _knockbackVelocity *= (1f - EnemyConstants.EnemyKnockbackDecay * dt);
            if (_knockbackVelocity.Length() < EnemyConstants.EnemyKnockbackStopThreshold)
                _knockbackVelocity = Vector2.Zero;
        }
    }
}
