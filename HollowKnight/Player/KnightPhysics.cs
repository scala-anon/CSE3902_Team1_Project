using Microsoft.Xna.Framework;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Player
{
    public class KnightPhysics
    {
        public Vector2 Velocity;
        public bool IsGrounded { get; private set; }

        private float knockbackTimer = KnightConstants.KnightKnockbackTimer;


        public KnightPhysics()
        {
            Velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public void Update(float dt)
        {
            Velocity.Y += KnightConstants.KnightGravity * dt;

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
            Velocity.X = KnightConstants.KnightMoveSpeed;
        }

        public void MoveLeft()
        {
            Velocity.X = -KnightConstants.KnightMoveSpeed;
        }

        public bool Jump()
        {
            if (IsGrounded)
            {
                Velocity.Y = KnightConstants.KnightJumpSpeed;
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
            knockbackTimer = (float)KnightConstants.KnightCastPulseDuration;
            Velocity.X = facing == Direction.Right
                ? -KnightConstants.KnightCastKnockbackSpeed
                : KnightConstants.KnightCastKnockbackSpeed;
        }

        public void ApplyDashVelocity(Direction direction)
        {
            Velocity.X = (direction == Direction.Right)
            ? KnightConstants.KnightDashSpeed
            : -KnightConstants.KnightDashSpeed;
        }

        public void ApplyKnockback(CollisionSide side)
        {
            knockbackTimer = KnightConstants.KnightKnockbackDuration;
            switch (side)
            {
                case CollisionSide.Left:
                    Velocity.X = -KnightConstants.KnightKnockbackSpeed;
                    Velocity.Y = KnightConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Right:
                    Velocity.X = KnightConstants.KnightKnockbackSpeed;
                    Velocity.Y = KnightConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Top:
                    Velocity.Y = KnightConstants.KnightKnockbackUpwards;
                    break;
                case CollisionSide.Bottom:
                    Velocity.Y = -KnightConstants.KnightKnockbackUpwards;
                    break;
            }
        }
    }
}
