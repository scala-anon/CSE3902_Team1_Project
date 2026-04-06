using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;

namespace HollowKnight.Projectiles
{
    public class VengefulSpiritProjectile : Projectile
    {
        private enum State { Moving, Impacting }
        private State _state = State.Moving;
        private ISprite _currentSprite;
        private Direction _facing;
        
        private double _impactTimer;
        private readonly double _impactDuration = 0.5; // ~ 5 frames

        public override bool PiercesEnemies => true;
        private const int LeadingHitboxWidth = 40; 
        private const int NoseOffsetRight = 220; 
        private const int NoseOffsetLeft = 0;  
        
        // Use these to manually push the explosion graphic around based on facing direction
        private const int CollisionOffsetRight = 152;
        private const int CollisionOffsetLeft = 0;

        public override Rectangle Bounds
        {
            get
            {
                if (_facing == Direction.Right)
                {
                    return new Rectangle((int)Position.X + NoseOffsetRight, (int)Position.Y, LeadingHitboxWidth, Height);
                }
                else
                {
                    return new Rectangle((int)Position.X + NoseOffsetLeft, (int)Position.Y, LeadingHitboxWidth, Height);
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

        public override void OnCollide()
        {
            if (_state == State.Moving)
            {
                _state = State.Impacting;
                _impactTimer = 0;
                
                // We must update the actual Property `Position` here so that
                // the `Update()` method doesn't aggressively snap it back to 
                // the pre-offset coordinates on the next frame!
                if (_facing == Direction.Right)
                {
                    Position.X += CollisionOffsetRight; 
                }
                else
                {
                    Position.X += CollisionOffsetLeft; 
                }
                
                // Stop the projectile from moving further
                Velocity = Vector2.Zero;
                
                _currentSprite = SpriteFactory.Instance.CreateSpiritCollisionSprite(Position);
            }
        }
    }
}
