using System;
using System.Collections.Generic;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Player
{
    public class TheKnight : IPlayer
    {
        private readonly Dictionary<KnightSpriteType, ISprite> sprites;
        private ISprite currentSprite;
        private KnightSpriteType currentSpriteType;

        public Rectangle[] hitBoxes = new Rectangle[1];
        public Direction Facing { get; private set; } = Direction.Right;
        public Vector2 position;

        private readonly KnightPhysics physics = new();
        private readonly KnightCombat combat = new();
        private readonly KnightHealth health = new();

        private int currentItem;

        public KnightState CurrentState { get; private set; } = KnightState.Idle;

        public bool IsActive => true;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
        public float VelocityY => physics.Velocity.Y;

        public TheKnight(Dictionary<KnightSpriteType, ISprite> sprites, Vector2 position)
        {
            this.sprites = sprites;
            this.position = position;

            currentSpriteType = KnightSpriteType.Idle;
            currentSprite = this.sprites[currentSpriteType];
            currentSprite.SetPosition(position);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update subsystems
            physics.Update(dt);
            position += physics.Velocity * dt;
            health.Update(gameTime);
            combat.Update(gameTime, position, Facing, currentSprite);

            // Resolve game state and animation state together
            if (health.IsHealing)
            {
                CurrentState = KnightState.Healing;
                KnightSpriteType? healState = health.UpdateHeal(gameTime);
                if (healState.HasValue)
                    currentSpriteType = healState.Value;
                physics.StopMovingHorizontal();
            }
            else if (combat.IsAttacking)
            {
                CurrentState = KnightState.Attacking;
                currentSpriteType = combat.AttackType;
            }
            else if (health.IsDamaged)
            {
                CurrentState = KnightState.Damaged;
                currentSpriteType = KnightSpriteType.Damaged;
            }
            else if (!physics.IsGrounded)
            {
                CurrentState = physics.Velocity.Y > 0 ? KnightState.Falling : KnightState.Jumping;
                currentSpriteType = KnightSpriteType.Jumping;
            }
            else if (physics.Velocity.X != 0)
            {
                CurrentState = KnightState.Running;
                currentSpriteType = KnightSpriteType.Walking;
            }
            else
            {
                CurrentState = KnightState.Idle;
                currentSpriteType = KnightSpriteType.Idle;
            }

            currentSprite = sprites[currentSpriteType];
            currentSprite.SetPosition(position);
            currentSprite.Update(gameTime);
        }

        public Rectangle[] GetBounds()
        {
            Vector2 size = currentSprite.GetSize();
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            return hitBoxes;
        }

        public Rectangle GetHurtbox()
        {
            Rectangle[] bounds = GetBounds();
            bounds[0].Inflate(-GameConstants.KnightHurtboxShrink, -GameConstants.KnightHurtboxShrink);
            return bounds[0];
        }

        public string GetStateName() => CurrentState.ToString();
        public double GetAttackCooldownRemaining() => combat.GetCooldownRemaining();
        public double GetInvincibilityCooldownRemaining() => health.GetInvincibilityCooldownRemaining();

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = Facing == Direction.Right
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            currentSprite.Draw(spriteBatch, effects);
            combat.DrawSlashEffect(spriteBatch, Facing);
        }

        // --- Movement ---
        public void MoveRight()
        {
            if (combat.IsAttacking) return;
            health.CancelHeal();
            Facing = Direction.Right;
            physics.MoveRight();
        }

        public void MoveLeft()
        {
            if (combat.IsAttacking) return;
            health.CancelHeal();
            Facing = Direction.Left;
            physics.MoveLeft();
        }

        public void MoveUp() => Console.WriteLine("Camera Move Up");
        public void MoveDown() => Console.WriteLine("Camera Move Down");

        public void Jump()
        {
            health.CancelHeal();
            physics.Jump();
        }

        public void StopMovingHorizontal() => physics.StopMovingHorizontal();
        public void StopMovingVertical() => physics.StopMovingVertical();
        public void Land() => physics.Land();

        // --- Combat ---
        public void SideSlash()
        {
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.SideSlash, position, physics.IsGrounded);
        }

        public void UpSlash()
        {
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.UpSlash, position, physics.IsGrounded);
        }

        public void DownSlash()
        {
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.DownSlash, position, physics.IsGrounded);
        }

        public SwordHitbox GetSwordHitbox() => combat.GetSwordHitbox(GetBounds()[0], Facing);

        // --- Health ---
        public void TakeDamage() => TakeDamage(CollisionSide.None);

        public void TakeDamage(CollisionSide side)
        {
            if (!health.TakeDamage()) return;
            Console.WriteLine("Knight took damage from " + side + " side");
            physics.ApplyKnockback(side);
        }

        public void StartHeal() => health.StartHeal(combat.IsAttacking, physics.IsGrounded);
        public void CancelHeal() => health.CancelHeal();

        // --- Items ---
        public void UseItem(int itemNumber)
        {
            currentItem = itemNumber;
            Console.WriteLine($"Using item #{currentItem}");
        }

        public void Collect(CollisionSide side) => Console.WriteLine("Knight picked up a power up!");
        public void Block(CollisionSide side) => Console.WriteLine("Knight is colliding with a block");

        // --- Position ---
        public Vector2 GetPosition() => position;

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            physics.Velocity = Vector2.Zero;
            currentSprite.SetPosition(position);
        }
    }
}
