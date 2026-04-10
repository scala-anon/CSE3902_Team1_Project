using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public enum CrawlidState { Idle, Turn, DeathAir, DeathLand }

    public class Crawlid : BaseEnemy
    {
        public CrawlidState State { get; set; } = CrawlidState.Idle;
        public bool Alive { get; set; } = true;
        public override bool IsActive => Alive;

        private readonly Dictionary<CrawlidState, ISprite> sprites;
        private CrawlidStateMachine stateMachine;

        public Crawlid(Vector2 position)
        {
            this.position = position;
            IsGrounded = true;
            FacingDirection = Direction.Right;
            sprites = new Dictionary<CrawlidState, ISprite>
            {
                [CrawlidState.Idle] = SpriteFactory.Instance.CreateCrawlidIdleSprite(position),
                [CrawlidState.Turn] = SpriteFactory.Instance.CreateCrawlidTurnSprite(position),
                [CrawlidState.DeathAir] = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(position),
                [CrawlidState.DeathLand] = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(position),
            };
            Sprite = sprites[CrawlidState.Idle];
            stateMachine = new CrawlidStateMachine(this);
        }

        public void SetState(CrawlidState newState)
        {
            State = newState;
            Sprite = sprites[newState];
        }

        public override void SetPlatform(IObject platform) => stateMachine.SetPlatform(platform);
        public override string GetStateName() => stateMachine.GetStateName();
        public void ChangeHealth() => stateMachine.ChangeHealth();

        protected override void ApplyKnockback(CollisionSide side)
        {
            switch (side)
            {
                case CollisionSide.Left: _knockbackVelocity = new Vector2(-EnemyConstants.EnemyKnockbackSpeed, 0f); break;
                case CollisionSide.Right: _knockbackVelocity = new Vector2(EnemyConstants.EnemyKnockbackSpeed, 0f); break;
                default: _knockbackVelocity = Vector2.Zero; break;
            }
            if (_knockbackVelocity.Y < 0) IsGrounded = false;
        }

        protected override void OnDeath(bool grounded) => SetState(CrawlidState.DeathLand);
        protected override void OnHealthChanged() => ChangeHealth();
        protected override void UpdateAlive(GameTime gameTime, float dt) => stateMachine.Update(gameTime);
    }
}
