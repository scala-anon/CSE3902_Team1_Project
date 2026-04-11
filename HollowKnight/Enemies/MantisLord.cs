using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public enum MantisLordState { Idle, Throw, Death, DStabArrive }

    public class MantisLord : BaseEnemy
    {
        public MantisLordState State { get; set; } = MantisLordState.DStabArrive;
        public bool Dead { get; set; }
        public override bool IsActive => !Dead;

        private readonly Dictionary<MantisLordState, ISprite> sprites;

        public MantisLord(Vector2 position)
        {
            this.position = position;
            IsGrounded = false;
            sprites = new Dictionary<MantisLordState, ISprite>
            {
                [MantisLordState.Idle] = SpriteFactory.Instance.CreateMantisThroneIdle(position),
                [MantisLordState.Throw] = SpriteFactory.Instance.CreateMantisThrow(position),
                [MantisLordState.Death] = SpriteFactory.Instance.CreateMantisDeath(position),
                [MantisLordState.DStabArrive] = SpriteFactory.Instance.CreateMantisDStabArrive(position),
            };
            Sprite = sprites[MantisLordState.DStabArrive];
        }

        public void SetState(MantisLordState newState)
        {
            State = newState;
            Sprite = sprites[newState];
        }

        public override string GetStateName() => State.ToString();

        protected override void ApplyKnockback(CollisionSide side) { }
        protected override void OnDeath(bool grounded) => SetState(MantisLordState.Death);
        protected override void OnHealthChanged() { }

        protected override void UpdateAlive(GameTime gameTime, float dt)
        {
            // Boss AI will be implemented via MantisLordStateMachine
        }
    }
}
