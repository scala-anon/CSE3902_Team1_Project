using System.Collections;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Enemies
{
    /// <summary>
    /// Drives all MantisLord animation transitions and attack sequencing.
    /// </summary>
    public class MantisLordStateMachine
    {
        private readonly MantisLord _owner;
        private float _stateTimer;

        // Shared RNG across all MantisLord instances; seeded once from environment tick count.
        private static readonly System.Random _rng = new System.Random(System.Environment.TickCount);

        // ---- Public surface consumed by BossFightController ----

        /// <summary>True while any attack animation is playing.</summary>
        public bool IsAttacking { get; private set; }

        /// <summary>True once the lord has returned to throne in wounded pose.</summary>
        public bool IsInWoundedPose { get; private set; }

        /// <summary>True once the bow animation has finished.</summary>
        public bool IsBowComplete { get; private set; }

        // ---- Internal state ----
        private bool _healthDepleted;
        private bool _postDeathTimer;
        private bool _fightStarted;

        private bool target_knight = false;

        public MantisLordStateMachine(MantisLord owner)
        {
            _owner = owner;
        }

        // ---- Commands called by BossFightController ----

        /// <summary>Kicks off the throne-stand-then-leave sequence.</summary>
        public void CommandActivate()
        {
            _fightStarted = true;
            _owner.Activate();
            EnterState(MantisLordState.ThroneStand);
        }

        /// <summary>After ThroneLeave completes, start the attack loop.</summary>
        public void CommandBeginAttackLoop()
        {
            PickNextAttack();
        }

        /// <summary>Force an immediate wall attack (used by BossFightController for simultaneous throws).</summary>
        public void CommandForceWallAttack()
        {
            if (_healthDepleted) return;  // do not interrupt death sequence
            IsAttacking = true;
            // TODO: audio hook — play wall-arrive sound
            EnterState(MantisLordState.WallArrive);
        }

        /// <summary>Called when sibling / middle has been defeated and this lord should return wounded.</summary>
        public void CommandReturnToThroneWounded()
        {
            IsAttacking = false;
            _postDeathTimer = true;
            _stateTimer = 0f;
        }

        /// <summary>Trigger the bow sequence (all three lords bow together).</summary>
        public void CommandBow()
        {
            EnterState(MantisLordState.ThroneBow);
        }

        /// <summary>Called by MantisLord.OnHealthChanged when HP reaches zero.</summary>
        public void OnHealthDepleted()
        {
            if (_healthDepleted) return;
            _healthDepleted = true;
            IsAttacking = false;
            // TODO: audio hook — play death sound
            EnterState(MantisLordState.Death);
        }

        // ---- Main update ----

        public void Update(GameTime gameTime, float dt)
        {
            if (!_fightStarted) return;

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateTimer += dt;

            // Waiting for post-death delay before entering ThroneWounded.
            if (_postDeathTimer)
            {
                if (_stateTimer >= EnemyConstants.MantisPostDeathToWoundedDelay)
                {
                    _postDeathTimer = false;
                    _stateTimer = 0f;
                    IsInWoundedPose = true;
                    EnterState(MantisLordState.ThroneWounded);
                }
                return;
            }

            switch (_owner.State)
            {
                case MantisLordState.ThroneStand:
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.ThroneLeave);
                    break;

                case MantisLordState.ThroneLeave:
                    if (_owner.Sprite.IsFinished)
                        CommandBeginAttackLoop();
                    break;

                case MantisLordState.ThroneArrive:
                    if (_owner.Sprite.IsFinished)
                    {
                        IsInWoundedPose = true;
                        EnterState(MantisLordState.ThroneWounded);
                    }
                    break;

                // ---- Throw sequence ----
                case MantisLordState.Throw:
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: spawn projectile here
                        EnterState(MantisLordState.WallLeave);
                    }
                    break;

                case MantisLordState.WallArrive:
                    if (_owner.FacingDirection == Direction.Right)
                    {
                        _owner.position = new Vector2(3600, 4546);
                    } else
                    {
                        _owner.position = new Vector2(3600, 4546);
                    }

                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.WallReady);
                    break;

                case MantisLordState.WallReady:
                    // Loops until controller forces a throw or duration elapses.
                    // TODO: spawn projectile here (when WallReady times out)
                    if(_owner.Sprite.IsFinished){
                    if (_stateTimer >= EnemyConstants.MantisWallReadyDuration)
                        EnterState(MantisLordState.Throw);
                    }
                    break;

                case MantisLordState.WallLeave:
                    if (_owner.Sprite.IsFinished)
                        StartAttackCooldown();
                    break;

                // ---- Dash sequence ----
                case MantisLordState.DashArrive:
                    if(_owner.FacingDirection == Direction.Right)
                    {
                        _owner.position = new Vector2(3880, 5000);
                    } else
                    {
                        _owner.position = new Vector2(5140, 5000);
                    }
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: audio hook — play dash anticipate sound
                        EnterState(MantisLordState.DashAnticipate);
                    }
                    break;

                case MantisLordState.DashAnticipate:
                    _owner.position.Y = 5000 + (556/2);
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: damage hitbox — activate dash contact damage here
                        EnterState(MantisLordState.Dash);
                    }
                    break;

                case MantisLordState.Dash:
                    if(_owner.FacingDirection == Direction.Right)
                    {
                        _owner.position.X -= EnemyConstants.MantisDashSpeed * elapsedTime;
                    } else
                    {
                        _owner.position.X += EnemyConstants.MantisDashSpeed * elapsedTime;
                    }
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DashRecover);
                    break;

                case MantisLordState.DashRecover:
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DashLeave);
                    break;

                case MantisLordState.DashLeave:
                    if (_owner.Sprite.IsFinished)
                        StartAttackCooldown();
                    break;

                // ---- DStab sequence ----
                case MantisLordState.DStabArrive:

                    if (target_knight == false){
                        target_knight = true;
                        _owner.position = new Vector2(_owner.knightPosition.X, _owner.knightPosition.Y - 800);
                    }

                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: damage hitbox — activate DStab contact damage here
                        EnterState(MantisLordState.DStab);
                    }
                    break;

                case MantisLordState.DStab:
                    _owner.position.Y += EnemyConstants.MantisStabSpeed * elapsedTime;
                    if (_owner.Sprite.IsFinished)
                        target_knight = false;
                        EnterState(MantisLordState.DStabLand);
                    break;

                case MantisLordState.DStabLand:
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DStabLeave);
                    break;

                case MantisLordState.DStabLeave:
                    if (_owner.Sprite.IsFinished)
                        StartAttackCooldown();
                    break;

                // ---- Attack cooldown (idle state after attack completes) ----
                case MantisLordState.IdleOnThrone:
                    if (_stateTimer >= EnemyConstants.MantisAttackCooldown)
                        PickNextAttack();
                    break;

                // ---- Death sequence ----
                case MantisLordState.Death:
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DeathLeaveOne);
                    break;

                case MantisLordState.DeathLeaveOne:
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DeathLeaveTwo);
                    break;

                case MantisLordState.DeathLeaveTwo:
                    if (_owner.Sprite.IsFinished)
                        CommandReturnToThroneWounded();
                    break;

                // ---- Bow sequence ----
                case MantisLordState.ThroneBow:
                    if (_owner.Sprite.IsFinished)
                        IsBowComplete = true;
                    break;

                // States with no automatic transition handled here:
                // ThroneWounded, Dormant — wait for external command.
                case MantisLordState.ThroneWounded:
                    if (_owner.Sprite.IsFinished)
                    {
                        
                    }
                    break;
                default:
                    break;
            }
        }

        // ---- Private helpers ----

        private void EnterState(MantisLordState state)
        {
            _stateTimer = 0f;
            _owner.SetState(state);
        }

        /// <summary>Starts the cooldown pause before the next attack.</summary>
        private void StartAttackCooldown()
        {
            IsAttacking = false;
            _stateTimer = 0f;
            // Re-use IdleOnThrone as the "between attacks" state.
            _owner.SetState(MantisLordState.IdleOnThrone);
            _owner.Sprite.Reset();
        }

        /// <summary>
        /// Randomly selects the next attack from {Throw (via Wall), Dash, DStab}.
        /// </summary>
        private void PickNextAttack()
        {
            IsAttacking = true;
            int roll = _rng.Next(3);
            // TODO: audio hook — play attack start sound
            switch (roll)
            {
                case 0:
                    EnterState(MantisLordState.WallArrive);
                    break;
                case 1:
                    EnterState(MantisLordState.DashArrive);
                    break;
                default:
                    EnterState(MantisLordState.DStabArrive);
                    break;
            }
        }
    }
}
