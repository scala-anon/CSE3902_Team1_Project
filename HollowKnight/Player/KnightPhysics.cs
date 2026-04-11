using Microsoft.Xna.Framework;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Audio;
using Microsoft.Xna.Framework.Audio;

namespace HollowKnight.Player
{
    public class KnightPhysics
    {
        public Vector2 Velocity;
        private int frameCounter = 0;
        public bool IsGrounded { get; private set; }
        public float VelocityX => Velocity.X;
        public float VelocityY => Velocity.Y;
        public bool IsMovingHorizontally => Velocity.X != 0;
        public bool IsAscending => Velocity.Y < 0;
        public bool IsFalling => Velocity.Y > 0;
        private SoundEffect walk;
        private SoundEffectInstance walkInstance;
        private bool walkingSound = false;

        private float knockbackTimer = KnightConstants.KnightKnockbackTimer;


        public KnightPhysics()
        {
            Velocity = Vector2.Zero;
            IsGrounded = false;
            walk = AudioLoader.Instance.Get_Hero_Run();
        }

        public void Update(float dt)
        {
            frameCounter++;

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
            if (walkingSound == false && IsGrounded)
            {
                walkInstance = AudioManager.Instance.PlaySoundEffect(walk);
                walkingSound = true;
            }
            else if (frameCounter % KnightConstants.RunFrameCounter == 0 && IsGrounded)
            {
                walkInstance = AudioManager.Instance.PlaySoundEffect(walk);
            }
        }

        public void MoveLeft()
        {
            Velocity.X = -KnightConstants.KnightMoveSpeed;
            if (walkingSound == false && IsGrounded)
            {
                walkInstance = AudioManager.Instance.PlaySoundEffect(walk);
                walkingSound = true;
            }
            else if (frameCounter % KnightConstants.RunFrameCounter == 0 && IsGrounded)
            {
                walkInstance = AudioManager.Instance.PlaySoundEffect(walk);
            }
        }

        public bool Jump()
        {
            if (IsGrounded)
            {
                Velocity.Y = KnightConstants.KnightJumpSpeed;
                IsGrounded = false;
                if (walkingSound)
                {
                    AudioManager.Instance.StopSoundEffect(walkInstance);
                    walkingSound = false;
                }
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
            if (walkingSound == true)
            {
                AudioManager.Instance.StopSoundEffect(walkInstance);
                walkingSound = false;
            }
            
        }

        public void StopMovingVertical() => Velocity.Y = 0;
        public void ZeroVerticalVelocity() => Velocity.Y = 0;

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
