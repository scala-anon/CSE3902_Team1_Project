using Microsoft.Xna.Framework;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Audio;
using Microsoft.Xna.Framework.Audio;
using System.Linq.Expressions;

namespace HollowKnight.Player
{
    public class KnightPhysics
    {
        public Vector2 Velocity;
        public bool IsGrounded { get; private set; }

        private bool isJumping = false;
        private readonly float moveSpeed = GameConstants.KnightMoveSpeed;
        private readonly float jumpSpeed = GameConstants.KnightJumpSpeed;
        private readonly float gravity = GameConstants.KnightGravity;
        private readonly float knockbackSpeed = GameConstants.KnightKnockbackSpeed;
        private readonly float knockbackUpwards = GameConstants.KnightKnockbackUpwards;

        public KnightPhysics()
        {
            Velocity = Vector2.Zero;
            IsGrounded = false;
        }

        public void Update(float dt)
        {
            Velocity.Y += gravity * dt;
        }

        public void MoveRight()
        {
            Velocity.X = moveSpeed;
        }

        public void MoveLeft()
        {
            Velocity.X = -moveSpeed;
        }

        public void Jump()
        {
            if (IsGrounded)
            {
                Velocity.Y = jumpSpeed;
                IsGrounded = false;
                isJumping = true;
                SoundEffect jump = AudioLoader.Instance.Get_Hero_Jump();
                AudioManager.Instance.PlaySoundEffect(jump);
            }
        }

        public void Land()
        {
            Velocity.Y = 0;
            IsGrounded = true;
            if (isJumping == true)
            {
                SoundEffect land = AudioLoader.Instance.Get_Hero_Land();
                AudioManager.Instance.PlaySoundEffect(land);
                isJumping = false;
            }
            

        }

        public void StopMovingHorizontal()
        {
            Velocity.X = 0;
        }

        public void StopMovingVertical()
        {
            Velocity.Y = 0;
        }

        public void ApplyKnockback(CollisionSide side)
        {
            switch (side)
            {
                case CollisionSide.Left:
                    Velocity.X = -knockbackSpeed;
                    Velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Right:
                    Velocity.X = knockbackSpeed;
                    Velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Top:
                    Velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Bottom:
                    Velocity.Y = -knockbackUpwards;
                    break;
            }
        }
    }
}
