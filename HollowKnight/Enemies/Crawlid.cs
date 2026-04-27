using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using HollowKnight.Audio;

namespace HollowKnight.Enemies
{
    public enum CrawlidState { Idle, Turn, DeathAir, DeathLand }

    public class Crawlid : BaseEnemy
    {
        public CrawlidState State { get; set; } = CrawlidState.Idle;
        public bool Alive { get; set; } = true;
        public override bool IsActive => Alive;

        private new Rectangle[] hitBoxes = new Rectangle[2];
        private readonly Dictionary<CrawlidState, ISprite> sprites;
        private CrawlidStateMachine stateMachine;

        public Crawlid(Vector2 position)
        {
            this.position = position;
            IsGrounded = false;
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

        public override Rectangle[] GetBounds()
        {
            Vector2 size = Sprite.GetSize();
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            int feetWidth = (int)size.X - CollisionConstants.CrawlidFeetWidthShrink;
            int feetX = (int)position.X + CollisionConstants.CrawlidFeetWidthShrink / 2;
            int feetY = (int)position.Y + (int)size.Y - CollisionConstants.CrawlidFeetHeight;
            hitBoxes[1] = new Rectangle(feetX, feetY, feetWidth, CollisionConstants.CrawlidFeetHeight);
            return hitBoxes;
        }

        public Rectangle FeetRect => GetBounds()[1];

        public void SetPlatforms(List<IObject> platforms) => stateMachine.SetPlatforms(platforms);
        public void OnWallHit() => stateMachine.OnWallHit();

        public override string GetStateName() => stateMachine.GetStateName();
        public void ChangeHealth() => stateMachine.ChangeHealth();

        public void Kill()
        {
            if (!Alive) return;
            
            Health = 0;
            Alive = false;
            _isDamaged = false;
            SetState(IsGrounded ? CrawlidState.DeathLand : CrawlidState.DeathAir);
        }

        public override void Land()
        {
            IsGrounded = true;
            _knockbackVelocity = Vector2.Zero;
            stateMachine.ResetVerticalVelocity();
        }

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

        protected override void OnDeath(bool grounded) => SetState(grounded ? CrawlidState.DeathLand : CrawlidState.DeathAir);
        protected override void OnHealthChanged() => ChangeHealth();
        protected override void UpdateAlive(GameTime gameTime, float dt) => stateMachine.Update(gameTime);
    }
}
