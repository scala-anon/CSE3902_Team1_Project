using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public enum VengeflyState { Idle, Startle, Chase, DeathAir, DeathLand }

    public class Vengefly : BaseEnemy
    {
        public VengeflyState State { get; set; } = VengeflyState.Idle;
        public bool Dead { get; set; }
        public override bool IsActive => !Dead;

        private readonly Dictionary<VengeflyState, ISprite> sprites;
        private VengeflyStateMachine stateMachine;

        public Vengefly(Vector2 position)
        {
            this.position = position;
            IsGrounded = false;
            sprites = new Dictionary<VengeflyState, ISprite>
            {
                [VengeflyState.Idle] = SpriteFactory.Instance.CreateVengeflyIdleSprite(position),
                [VengeflyState.Startle] = SpriteFactory.Instance.CreateVengeflyStartleSprite(position),
                [VengeflyState.Chase] = SpriteFactory.Instance.CreateVengeflyChaseSprite(position),
                [VengeflyState.DeathAir] = SpriteFactory.Instance.CreateVengeflyDeathAirSprite(position),
                [VengeflyState.DeathLand] = SpriteFactory.Instance.CreateVengeflyDeathLandSprite(position),
            };
            Sprite = sprites[VengeflyState.Idle];
            stateMachine = new VengeflyStateMachine(this);
        }

        public void SetState(VengeflyState newState)
        {
            State = newState;
            Sprite = sprites[newState];
        }

        public override void SetNavigationGrid(NavigationGrid grid) => stateMachine.SetNavigationGrid(grid);
        public override List<Vector2> GetCurrentPath() => stateMachine.GetCurrentPath();
        public override float GetDetectionRadius() => stateMachine.GetDetectionRadius();
        public override float GetChaseRadius() => stateMachine.GetChaseRadius();
        public override string GetStateName() => stateMachine.GetStateName();
        public void ChangeHealth() => stateMachine.ChangeHealth();

        public void Kill()
        {
            if (Dead) return;
            Health = 0;
            Dead = true;
            _isDamaged = false;
            SetState(VengeflyState.DeathAir);
        }

        public void Land()
        {
            _knockbackVelocity = Vector2.Zero;
            IsGrounded = true;
            SetState(VengeflyState.DeathLand);
        }

        protected override void ApplyKnockback(CollisionSide side)
        {
            switch (side)
            {
                case CollisionSide.Left: _knockbackVelocity = new Vector2(-EnemyConstants.EnemyKnockbackSpeed, EnemyConstants.VengeflyKnockbackUpComponent); break;
                case CollisionSide.Right: _knockbackVelocity = new Vector2(EnemyConstants.EnemyKnockbackSpeed, EnemyConstants.VengeflyKnockbackUpComponent); break;
                case CollisionSide.Top: _knockbackVelocity = new Vector2(0, -EnemyConstants.VengeflyVerticalKnockbackSpeed); break;
                case CollisionSide.Bottom: _knockbackVelocity = new Vector2(0, EnemyConstants.VengeflyVerticalKnockbackSpeed); break;
            }
        }

        protected override void OnDeath(bool grounded) => SetState(VengeflyState.DeathLand);
        protected override void OnHealthChanged() => ChangeHealth();
        protected override void UpdateAlive(GameTime gameTime, float dt) => stateMachine.Update(gameTime);
    }
}
