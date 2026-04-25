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
        private float _attackCooldownTimer;

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
            if (_frozen) return; // add this line
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateTimer += dt;
            _attackCooldownTimer += dt;

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
                    switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY);
                        break;
                    }
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.ThroneLeave);
                    break;

                case MantisLordState.ThroneLeave:
                    switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.FacingDirection = Direction.Left;
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY+72+18);
                        break;

                        case MantisLordSlot.Right:
                            _owner.FacingDirection = Direction.Right;
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY+72+18);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.FacingDirection = Direction.Right;
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY+72+18);
                        break;
                    }
                    if(_owner.Sprite.IsFinished)
                    {
                        CommandBeginAttackLoop();
                    }
                    break;


                case MantisLordState.ThroneArrive:
                    switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY);
                        break;
                    }
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
                        _owner.position = new Vector2(EnemyConstants.MantisWallHangLeftX, EnemyConstants.MantisWallHangY);
                    } else
                    {
                        _owner.position = new Vector2(EnemyConstants.MantisWallHangRightX, EnemyConstants.MantisWallHangY);
                    }

                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.WallReady);
                    break;



                case MantisLordState.WallReady:
                    _owner.position.Y = EnemyConstants.MantisWallHangY + EnemyConstants.MantisWallHangOffset;
                    // Loops until controller forces a throw or duration elapses.
                    // TODO: spawn projectile here (when WallReady times out)
                    if(_owner.Sprite.IsFinished){
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
                        _owner.position = new Vector2(EnemyConstants.MantisDashArriveRightX, EnemyConstants.MantisDashY);
                    } else
                    {
                        _owner.position = new Vector2(EnemyConstants.MantisDashArriveLeftX, EnemyConstants.MantisDashY);
                    }
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: audio hook — play dash anticipate sound
                        EnterState(MantisLordState.DashAnticipate);
                    }
                    break;

                case MantisLordState.DashAnticipate:
                    _owner.position.Y = EnemyConstants.MantisDashY + EnemyConstants.MantisDashArriveSpriteHeigthOffset;
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: damage hitbox — activate dash contact damage here
                        EnterState(MantisLordState.Dash);
                    }
                    break;

                case MantisLordState.Dash:
                    _owner.position.Y = EnemyConstants.MantisDashY + EnemyConstants.MantisDashAnticipateSpriteHeightOffset + 100;
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
                    _owner.position.Y = EnemyConstants.MantisDashY;
                    if (_owner.Sprite.IsFinished)
                        StartAttackCooldown();
                    break;

                // ---- DStab sequence ----
                case MantisLordState.DStabStart:
                //- (765/2)
                     _owner.position = new Vector2(_owner.knightPosition.X-765/2+35 , _owner.knightPosition.Y - 950);
                     EnterState(MantisLordState.DStabArrive);
                     break;
                case MantisLordState.DStabArrive:
                    // _owner.position.X = _owner.knightPosition.X - 60;
                    if (_owner.Sprite.IsFinished)
                    {
                        // TODO: damage hitbox — activate DStab contact damage here
                        EnterState(MantisLordState.DStabOffset);
                    }
                    break;
                case MantisLordState.DStabOffset:
                    _owner.position.X += 250;
                    EnterState(MantisLordState.DStab);
                    break;
                case MantisLordState.DStab:
                    _owner.position.Y += EnemyConstants.MantisStabSpeed * elapsedTime;
                    if (_owner.Sprite.IsFinished)
                        EnterState(MantisLordState.DStabLandOffset);
                    break;
                case MantisLordState.DStabLandOffset:
                    _owner.position.X-= 120;
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
                    switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX-5, EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY+10);
                        break;
                    }
                    if (_owner.Sprite.IsFinished)
                        IsBowComplete = true;
                    break;

                // States with no automatic transition handled here:
                // ThroneWounded, Dormant — wait for external command.
                case MantisLordState.ThroneWounded:
                    switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.FacingDirection = Direction.Left;
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX-24, EnemyConstants.SecondaryMantisStandingY-100);
                        break;

                        case MantisLordSlot.Right:
                            _owner.FacingDirection = Direction.Right;
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX-4,EnemyConstants.SecondaryMantisStandingY-100);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY-200+90);
                        break;
                    }         
                    if (_owner.Sprite.IsFinished)
                    {
                        
                    }
                    break;
                    case MantisLordState.GracePeriod:
                        _owner.position = new Vector2(0,0);
                        if(_attackCooldownTimer >= EnemyConstants.MantisAttackCooldown)
                        {
                            PickNextAttack();
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
            _attackCooldownTimer = 0f;
            _owner.SetState(MantisLordState.GracePeriod);
        }

        /// <summary>
        /// Randomly selects the next attack from {Throw (via Wall), Dash, DStab}.
        /// </summary>
        private void PickNextAttack()
        {
            IsAttacking = true;
            int roll = _rng.Next(10);
            
            switch (roll)
            {
                case 0:
                case 1:
                    EnterState(MantisLordState.WallArrive);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    EnterState(MantisLordState.DashArrive);
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    EnterState(MantisLordState.DStabStart);
                    break;
            }



        }
        // Add this field at the top with the other fields
        private bool _frozen = false;

        // Add these public methods
        public void ToggleFreeze() => _frozen = !_frozen;
        public bool IsFrozen => _frozen;

        public void ResetDebugState()
        {
            target_knight = false;
        }

        public void ForceStartFight()
        {
            _fightStarted = true;
            _owner.Activate();
        }

        // Add this method to snap position instantly when toggling states
        public void SnapPositionForState(MantisLordState state)
        {
            switch (state)
            {
                case MantisLordState.ThroneStand:
                    switch(_owner.Slot)
                        {
                            case MantisLordSlot.Left:
                                _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY);
                            break;

                            case MantisLordSlot.Right:
                                _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                            break;

                            case MantisLordSlot.Middle:
                                _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY);
                            break;
                    }
                break;
                case MantisLordState.IdleOnThrone:
                case MantisLordState.ThroneLeave:
                switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.FacingDirection = Direction.Left;
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Right:
                            _owner.FacingDirection = Direction.Right;
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.FacingDirection = Direction.Right;
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY);
                        break;
                    }
                    break;
                case MantisLordState.ThroneArrive:
                switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX, EnemyConstants.SecondaryMantisStandingY+72+18);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY+72+18);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY+72+18);
                        break;
                    }
                break;
                case MantisLordState.ThroneWounded:
                switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX-24, EnemyConstants.SecondaryMantisStandingY-100);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX-4,EnemyConstants.SecondaryMantisStandingY-100);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY-200+90);
                        break;
                    }
                break;
                case MantisLordState.ThroneBow:
                switch(_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(EnemyConstants.LeftMantisStandingX-5, EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(EnemyConstants.RightMantisStandingX,EnemyConstants.SecondaryMantisStandingY);
                        break;

                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(EnemyConstants.MiddleMantisStandingX,EnemyConstants.PrimaryMantisStandingY+10);
                        break;
                    }
                break;
                case MantisLordState.Dormant:
                    switch (_owner.Slot)
                    {
                        case MantisLordSlot.Left:
                            _owner.position = new Vector2(3800, 5000);
                            break;
                        case MantisLordSlot.Middle:
                            _owner.position = new Vector2(4518, 4900);
                            break;
                        case MantisLordSlot.Right:
                            _owner.position = new Vector2(5200, 5000);
                            break;
                    }
                    break;

                case MantisLordState.WallArrive:
                case MantisLordState.WallReady:
                case MantisLordState.Throw:
                case MantisLordState.WallLeave:
                    _owner.position = new Vector2(3600, 4546);
                    break;

                case MantisLordState.DashArrive:
                case MantisLordState.DashAnticipate:
                case MantisLordState.Dash:
                case MantisLordState.DashRecover:
                case MantisLordState.DashLeave:
                    _owner.position = _owner.FacingDirection == Direction.Right
                        ? new Vector2(3880, 5000)
                        : new Vector2(5140, 5000);
                    break;

                case MantisLordState.DStabArrive:
                case MantisLordState.DStab:
                case MantisLordState.DStabLand:
                case MantisLordState.DStabLeave:
                    target_knight = false;
                    _owner.position = new Vector2(_owner.knightPosition.X, _owner.knightPosition.Y - 800);
                    break;
            }
        }

        // Guard the update loop with the freeze check — replace the top of Update:
    }
}
