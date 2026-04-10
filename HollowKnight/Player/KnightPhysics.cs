using Microsoft.Xna.Framework;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Player
{
    public class KnightPhysics
    {
        public Vector2 Velocity;
        public bool IsGrounded { get; private set; }

        private float knockbackTimer = GameConstants.KnightKnockbackTimer;


        public KnightPhysics()
        {
            Velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public void Update(float dt)
        {
            Velocity.Y += GameConstants.KnightGravity * dt;

            if(knockbackTimer > 0f)
            {
                knockbackTimer -=dt;
                if(knockbackTimer <= 0f)
                {
                    knockbackTimer = 0f;
                    Velocity.X = 0f;
                }
            }
        }

        public void MoveRight()
        {
            Velocity.X = GameConstants.KnightMoveSpeed;
        }

        public void MoveLeft()
        {
            Velocity.X = -GameConstants.KnightMoveSpeed;
        }

        public bool Jump()
        {
            if (IsGrounded)
            {
                Velocity.Y = GameConstants.KnightJumpSpeed;
                IsGrounded = false;
                return true;
            }
            return false;
        }

        public void StopJump()
        {
            if (Velocity.Y < 0) // Still ascending
            {
                Velocity.Y *= GameConstants.JumpMomentumCut;
            }
        }

        public void Land()
        {
            Velocity.Y = 0;
            IsGrounded = true;
        }

        public void SetAirborne()
        {
            IsGrounded = false;
        }

        public void StopMovingHorizontal()
        {
            Velocity.X = 0;
        }

        public void StopMovingVertical()
        {
            Velocity.Y = 0;
        }

        public void StopAllMovement()
        {
            Velocity = Vector2.Zero;
        }

        public void ApplyCastKnockback(Direction facing)
        {
            knockbackTimer = (float)GameConstants.KnightCastPulseDuration;
            Velocity.X = facing == Direction.Right
                ? -GameConstants.KnightCastKnockbackSpeed
                : GameConstants.KnightCastKnockbackSpeed;
        }

        public void ApplyDashVelocity(Direction direction)
        {
            Velocity.X = (direction == Direction.Right)
            ? GameConstants.KnightDashSpeed
            : -GameConstants.KnightDashSpeed;
        }

        public void ApplyKnockback(CollisionSide side)
        {
            knockbackTimer = GameConstants.KnightKnockbackDuration;
            switch (side)
            {
                case CollisionSide.Left:
                    Velocity.X = -GameConstants.KnightKnockbackSpeed;
                    Velocity.Y = GameConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Right:
                    Velocity.X = GameConstants.KnightKnockbackSpeed;
                    Velocity.Y = GameConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Top:
                    Velocity.Y = GameConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Bottom:
                    Velocity.Y = -GameConstants.KnightKnockbackUpwards;
                    break;
            }
        }
    }
}
