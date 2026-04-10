using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Projectiles
{
    public class VengefulSpiritProjectile : Projectile
    {
        private enum State { Moving, Impacting }
        private State _state = State.Moving;
        private ISprite _currentSprite;
        private Direction _facing;

        private double _impactTimer;
        private readonly double _impactDuration = GameConstants.VengefulSpiritImpactDuration;

        public override bool PiercesEnemies => true;

        public override Rectangle Bounds
        {
            get
            {
                if (_facing == Direction.Right)
                {
                    return new Rectangle((int)Position.X + CollisionConstants.VengefulSpiritNoseOffsetRight, (int)Position.Y, CollisionConstants.VengefulSpiritLeadingHitboxWidth, Height);
                }
                else
                {
                    return new Rectangle((int)Position.X + CollisionConstants.VengefulSpiritNoseOffsetLeft, (int)Position.Y, CollisionConstants.VengefulSpiritLeadingHitboxWidth, Height);
                }
            }
        }

        public VengefulSpiritProjectile(Vector2 position, Vector2 velocity, ProjectileFaction faction)
            : base(position, velocity, faction)
        {
            _facing = velocity.X >= 0 ? Direction.Right : Direction.Left;
            _currentSprite = SpriteFactory.Instance.CreateSpiritMovingSprite(position);

            Vector2 size = _currentSprite.GetSize();
            Width = (int)size.X;
            Height = (int)size.Y;

            // Adjust position so it spawns centered to the knight
            Position.Y -= Height / 2f;
            _currentSprite.SetPosition(Position);
        }

        public override void Update(GameTime gameTime)
        {
            if (_state == State.Impacting)
            {
                _impactTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_impactTimer >= _impactDuration)
                {
                    Alive = false;
                }
            }

            _currentSprite.SetPosition(Position);
            _currentSprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Direction facing)
        {
            if (!Alive) return;
            if (_state == State.Moving && !HasMoved) return;

            SpriteEffects effects;
            if (_state == State.Moving)
            {
                effects = _facing == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else
            {
                // If moving left, use right-facing explosion. If moving right, use left-facing explosion.
                effects = _facing == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            _currentSprite.Draw(spriteBatch, effects);
        }

        public override void OnCollide(ICollidable target, CollisionSide side)
        {
            if (_state == State.Moving)
            {
                _state = State.Impacting;
                _impactTimer = 0;

                // Position the effect at the collision point on the target's edge
                switch (side)
                {
                    case CollisionSide.Left:
                        Position.X = target.Bounds.Right - Width / 2;
                        break;
                    case CollisionSide.Right:
                        Position.X = target.Bounds.Left - Width / 2;
                        break;
                    case CollisionSide.Top:
                        Position.Y = target.Bounds.Bottom - Height / 2;
                        break;
                    case CollisionSide.Bottom:
                        Position.Y = target.Bounds.Top - Height / 2;
                        break;
                }

                // Adjust Y to show effect higher on screen
                Position.Y += CollisionConstants.VengefulSpiritCollisionOffsetY;

                // Stop the projectile from moving further
                Velocity = Vector2.Zero;

                _currentSprite = SpriteFactory.Instance.CreateSpiritCollisionSprite(Position);
            }
        }
    }
}
