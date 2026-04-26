using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using System;


namespace HollowKnight.Enemies
{
    public enum MantisLordState
    {
        DStabStart,
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
        DStabOffset,
        DStab,
        DStabLandOffset,
        DStabLand,
        DStabLeave,
        WallArrive,
        WallReady,
        WallLeave1,
        WallLeave2,
        Death,
        DeathLeaveOne,
        DeathLeaveTwo,
        Dormant,
        GracePeriod
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
            if (slot == MantisLordSlot.Left)
            {
                FacingDirection = Direction.Right;
            } else if (slot == MantisLordSlot.Right)
            {
                FacingDirection = Direction.Left;
            } 
            else
            {
                FacingDirection = Direction.Right;
            }

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
                [MantisLordState.WallLeave1]      = SpriteFactory.Instance.CreateWallLeave1(position),
                [MantisLordState.WallLeave2]     = SpriteFactory.Instance.CreateWallLeave2(position),
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
            if (newState != MantisLordState.DStabStart && newState != MantisLordState.GracePeriod && newState != MantisLordState.DStabOffset && newState != MantisLordState.DStabLandOffset){
                Sprite = _sprites[newState];
                Sprite.Reset();
            }
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

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, EnemyConstants.mantisLordHitBoxes[State].Width, EnemyConstants.mantisLordHitBoxes[State].Height);
            return hitBoxes;
        }
    }
}
