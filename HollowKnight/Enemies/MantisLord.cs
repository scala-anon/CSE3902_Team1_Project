using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public enum MantisLordState {ThroneIdle, ThroneStand, ThroneLeave, WallArrive, WallReady, Throw, WallLeave, DashArrive, DashAnticipate, Dash, DashRecover, DashLeave, DStabArrive, DStab, DStabLand, DStabLeave, Death, DeathLeave, ThroneWounded, ThroneBow}

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
                [MantisLordState.ThroneIdle] = SpriteFactory.Instance.CreateMantisThroneIdle(position),
                [MantisLordState.ThroneStand] = SpriteFactory.Instance.CreateMantisThroneStand(position),
                [MantisLordState.ThroneLeave] = SpriteFactory.Instance.CreateMantisThroneLeave(position),
                [MantisLordState.WallArrive] = SpriteFactory.Instance.CreateMantisWallArrive(position),
                [MantisLordState.WallReady] = SpriteFactory.Instance.CreateMantisWallReady(position),
                [MantisLordState.Throw] = SpriteFactory.Instance.CreateMantisThrow(position),
                [MantisLordState.WallLeave] = SpriteFactory.Instance.CreateMantisWallLeave(position),
                [MantisLordState.DashArrive] = SpriteFactory.Instance.CreateMantisDashArrive(position),
                [MantisLordState.DashAnticipate] = SpriteFactory.Instance.CreateMantisDashAnticipate(position),
                [MantisLordState.Dash] = SpriteFactory.Instance.CreateMantisDash(position),
                [MantisLordState.DashRecover] = SpriteFactory.Instance.CreateMantisDashRecover(position),
                [MantisLordState.DashLeave] = SpriteFactory.Instance.CreateMantisDashLeave(position),
                [MantisLordState.DStabArrive] = SpriteFactory.Instance.CreateMantisDStabArrive(position),
                [MantisLordState.DStab] = SpriteFactory.Instance.CreateMantisDStab(position),
                [MantisLordState.DStabLand] = SpriteFactory.Instance.CreateMantisDStabLand(position),
                [MantisLordState.DStabLeave] = SpriteFactory.Instance.CreateMantisDStabLeave(position),
                [MantisLordState.Death] = SpriteFactory.Instance.CreateMantisDeath(position),
                [MantisLordState.DeathLeave] = SpriteFactory.Instance.CreateMantisDeathLeaveOne(position),
                [MantisLordState.ThroneWounded] = SpriteFactory.Instance.CreateMantisThroneWounded(position),
                [MantisLordState.ThroneBow] = SpriteFactory.Instance.CreateMantisThroneBow(position),
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
