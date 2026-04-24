using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;


namespace HollowKnight.Enemies
{
    public enum MantisLordState
    {
        IdleOnThrone,
        ThroneStand,
        ThroneLeave,
        ThroneArrive,
        ThroneWounded,
        ThroneBow,
        Throw,
        DashArrive,
        DashAnticipate,
        Dash,
        DashRecover,
        DashLeave,
        DStabArrive,
        DStab,
        DStabLand,
        DStabLeave,
        WallArrive,
        WallReady,
        WallLeave,
        Death,
        DeathLeaveOne,
        DeathLeaveTwo,
        Dormant
    }

    public class MantisLord : BaseEnemy
    {
        public MantisLordState State { get; private set; } = MantisLordState.IdleOnThrone;
        public MantisLordSlot Slot { get; }
        public bool Dead { get; set; }

        // Once activated the lord stays drawn even when HP hits zero (wounded/bow phases).
        // _activated tracks whether the fight has started for this lord.
        private bool _activated;

        public override bool IsActive => !Dead || _activated;

        private readonly Dictionary<MantisLordState, ISprite> _sprites;
        private MantisLordStateMachine _stateMachine;

        // Expose the state machine so BossFightController can command it.
        public MantisLordStateMachine StateMachine => _stateMachine;

        public MantisLord(Vector2 position, MantisLordSlot slot)
        {
            this.position = position;
            Slot = slot;
            IsGrounded = false;

            // Left lord faces right (toward center) — use the existing FacingDirection flip in BaseEnemy.Draw.
            FacingDirection = slot == MantisLordSlot.Left ? Direction.Right : Direction.Left;

            Health = slot == MantisLordSlot.Middle
                ? EnemyConstants.MantisLordMiddleHealth
                : EnemyConstants.MantisLordSideHealth;

            _sprites = new Dictionary<MantisLordState, ISprite>
            {
                [MantisLordState.IdleOnThrone]   = SpriteFactory.Instance.CreateMantisThroneIdle(position),
                [MantisLordState.ThroneStand]    = SpriteFactory.Instance.CreateThroneStand(position),
                [MantisLordState.ThroneLeave]    = SpriteFactory.Instance.CreateThroneLeave(position),
                [MantisLordState.ThroneArrive]   = SpriteFactory.Instance.CreateThroneArrive(position),
                [MantisLordState.ThroneWounded]  = SpriteFactory.Instance.CreateThroneWounded(position),
                [MantisLordState.ThroneBow]      = SpriteFactory.Instance.CreateThroneBow(position),
                [MantisLordState.Throw]          = SpriteFactory.Instance.CreateMantisThrow(position),
                [MantisLordState.DashArrive]     = SpriteFactory.Instance.CreateMantisDashArrive(position),
                [MantisLordState.DashAnticipate] = SpriteFactory.Instance.CreateMantisDashAnticipate(position),
                [MantisLordState.Dash]           = SpriteFactory.Instance.CreateMantisDash(position),
                [MantisLordState.DashRecover]    = SpriteFactory.Instance.CreateMantisDashRecover(position),
                [MantisLordState.DashLeave]      = SpriteFactory.Instance.CreateMantisDashLeave(position),
                [MantisLordState.DStabArrive]    = SpriteFactory.Instance.CreateMantisDStabArrive(position),
                [MantisLordState.DStab]          = SpriteFactory.Instance.CreateMantisDStab(position),
                [MantisLordState.DStabLand]      = SpriteFactory.Instance.CreateMantisDStabLand(position),
                [MantisLordState.DStabLeave]     = SpriteFactory.Instance.CreateMantisDStabLeave(position),
                [MantisLordState.WallArrive]     = SpriteFactory.Instance.CreateWallArrive(position),
                [MantisLordState.WallReady]      = SpriteFactory.Instance.CreateWallReady(position),
                [MantisLordState.WallLeave]      = SpriteFactory.Instance.CreateWallLeave(position),
                [MantisLordState.Death]          = SpriteFactory.Instance.CreateMantisDeath(position),
                [MantisLordState.DeathLeaveOne]  = SpriteFactory.Instance.CreateMantisDeathLeaveOne(position),
                [MantisLordState.DeathLeaveTwo]  = SpriteFactory.Instance.CreateMantisDeathLeaveTwo(position),
                [MantisLordState.Dormant]        = SpriteFactory.Instance.CreateMantisThroneIdle(position),
            };

            Sprite = _sprites[MantisLordState.IdleOnThrone];
            _stateMachine = new MantisLordStateMachine(this);
        }

        public void SetState(MantisLordState newState)
        {
            State = newState;
            Sprite = _sprites[newState];
            Sprite.Reset();
        }

        public void Activate()
        {
            _activated = true;
        }

        public override string GetStateName() => State.ToString();

        protected override void ApplyKnockback(CollisionSide side) { }

        protected override void OnDeath(bool grounded)
        {
            // Boss death is handled entirely by the state machine; do not set Dead here.
        }

        protected override void OnHealthChanged()
        {
            Health--;
            if (Health <= 0)
                _stateMachine.OnHealthDepleted();
        }

        public override bool TakeDamage(CollisionSide side)
        {
            if (!_stateMachine.IsAttacking) return false;
            // TODO: contact-damage hitbox logic would be checked here
            return base.TakeDamage(side);
        }

        protected override void UpdateAlive(GameTime gameTime, float dt)
            => _stateMachine.Update(gameTime, dt);

    }
}
