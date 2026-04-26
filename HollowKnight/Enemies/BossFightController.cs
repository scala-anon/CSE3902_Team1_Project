using System.Collections.Generic;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public enum BossFightPhase
    {
        Dormant,
        Phase1_Middle,
        Phase2_Transition,
        Phase2_Sides,
        Phase3_Solo,
        Victory_Bow,
        Done
    }

    /// <summary>
    /// Coordinates the three Mantis Lords through their fight phases.
    /// Must be updated AFTER all three lords have been individually updated each frame.
    /// </summary>
    public class BossFightController
    {
        private readonly Game1 _game;
        private readonly MantisLord _left;
        private readonly MantisLord _middle;
        private readonly MantisLord _right;

        
        // Added for debugger
        public MantisLord Left   => _left;
        public MantisLord Middle => _middle;
        public MantisLord Right  => _right;

        private BossFightPhase _phase = BossFightPhase.Dormant;
        private float _wallAttackTimer;

        // Sibling lords must stagger their activation slightly after the middle dies.
        private bool _siblingsCommandedActive;
        private float _siblingStaggerTimer;

        // field
        private bool _paused = false;

        // method
        public void TogglePause() => _paused = !_paused;
        // Add fields
        // private MantisLord _phase2Leader;
        // private bool _phase2FollowerCommanded;
        // private float _phase2FollowerDelay;
        // private const float FollowerDelayAfterLead = 0.0f;  // small extra delay if you want one

        // Add fields
        private MantisLord _phase2Leader;
        private MantisLord _phase2Follower;
        private bool _phase2FollowerCommanded;
        private float _phase2FollowerDelay;
        private const float FollowerDelayAfterLead = 0.0f;
        private bool _phase2RolesInitialized;
        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                yield return _left;
                yield return _middle;
                yield return _right;
            }
        }

        public BossFightController(Game1 game, MantisLord left, MantisLord middle, MantisLord right)
        {
            _game = game;
            _left = left;
            _middle = middle;
            _right = right;
        }

        /// <summary>
        /// Begins the fight — activates the middle lord and locks the room.
        /// </summary>
        public void Activate()
        {
            if (_phase != BossFightPhase.Dormant) return;
            // TODO: lock room exit here
            _phase = BossFightPhase.Phase1_Middle;
            _middle.StateMachine.CommandActivate();
        }

        public void Update(GameTime gameTime)
        {
            // if (_phase == BossFightPhase.Dormant || _phase == BossFightPhase.Done)
            //     return;

            if (_phase == BossFightPhase.Dormant || _phase == BossFightPhase.Done || _paused)
                return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (_phase)
            {
                case BossFightPhase.Phase1_Middle:
                    UpdatePhase1(dt);
                    break;

                case BossFightPhase.Phase2_Transition:
                    UpdatePhase2Transition(dt);
                    break;

                case BossFightPhase.Phase2_Sides:
                    UpdatePhase2Sides(dt);
                    break;

                case BossFightPhase.Phase3_Solo:
                    UpdatePhase3Solo(dt);
                    break;

                case BossFightPhase.Victory_Bow:
                    UpdateVictoryBow();
                    break;
            }
        }

        // ---- Phase handlers ----

        private void UpdatePhase1(float dt)
        {
            _wallAttackTimer += dt;

            /*if (_wallAttackTimer >= EnemyConstants.MantisWallAttackInterval)
            {
                _wallAttackTimer = 0f;
                _middle.StateMachine.CommandForceWallAttack();
            }
            */

            // Middle lord depleted → enter transition to phase 2
            if (_middle.StateMachine.IsInWoundedPose)
            {
                _phase = BossFightPhase.Phase2_Transition;
                _siblingsCommandedActive = false;
                _siblingStaggerTimer = 0f;
                _wallAttackTimer = 0f;
            }
        }

        private void UpdatePhase2Transition(float dt)
        {
            _siblingStaggerTimer += dt;

            if (!_siblingsCommandedActive &&
                _siblingStaggerTimer >= EnemyConstants.MantisSiblingStaggerDelay)
            {
                _siblingsCommandedActive = true;
                _left.StateMachine.CommandActivate();
                _right.StateMachine.CommandActivate();
                _phase = BossFightPhase.Phase2_Sides;
                _wallAttackTimer = 0f;
            }
        }

        // private void UpdatePhase2Sides(float dt)
        // {
        //     _wallAttackTimer += dt;

            
        //     // TODO: Refacctor to see if mantisleft or right in wall attack and force the other one to go into it
            
               
            
        //     // if (_wallAttackTimer >= EnemyConstants.MantisWallAttackInterval)
        //     // {
        //     //     _wallAttackTimer = 0f;
        //     //     // Both side lords attack simultaneously for the wall attack.
        //     //     _left.StateMachine.CommandForceWallAttack();
        //     //     _right.StateMachine.CommandForceWallAttack();
        //     // }

        //     bool leftWounded  = _left.StateMachine.IsInWoundedPose;
        //     bool rightWounded = _right.StateMachine.IsInWoundedPose;

        //     // If one side lord is defeated before the other, the remaining lord enters Phase3.
        //     if (leftWounded && !rightWounded)
        //     {
        //         _phase = BossFightPhase.Phase3_Solo;
        //         _wallAttackTimer = 0f;
        //         return;
        //     }
        //     if (rightWounded && !leftWounded)
        //     {
        //         _phase = BossFightPhase.Phase3_Solo;
        //         _wallAttackTimer = 0f;
        //         return;
        //     }

        //     // // Both wounded → all three bow
        //     // if (leftWounded && rightWounded)
        //     // {
        //     //     BeginVictoryBow();
        //     // }
        // }


        // was working TODO:
        private void UpdatePhase2Sides(float dt)
        {
            // ---- Wounded checks (unchanged) ----
            bool leftWounded  = _left.StateMachine.IsInWoundedPose;
            bool rightWounded = _right.StateMachine.IsInWoundedPose;
            if (leftWounded && !rightWounded) { _phase = BossFightPhase.Phase3_Solo; ResetPhase2(); return; }
            if (rightWounded && !leftWounded) { _phase = BossFightPhase.Phase3_Solo; ResetPhase2(); return; }
            if (leftWounded && rightWounded)  { BeginVictoryBow(); return; }

            // ---- Coordination ----
            var leftKind  = _left.StateMachine.CurrentAttackKind;
            var rightKind = _right.StateMachine.CurrentAttackKind;

            // Cycle complete: both back to non-attacking → clear leader for next cycle
            if (_phase2Leader != null && !_phase2Leader.StateMachine.IsAttacking)
            {
                var follower = (_phase2Leader == _left) ? _right : _left;
                if (!follower.StateMachine.IsAttacking)
                {
                    _phase2Leader = null;
                    _phase2FollowerCommanded = false;
                }
            }

            // No active leader: pick whichever lord just started attacking
            if (_phase2Leader == null)
            {
                if (leftKind != MantisLordStateMachine.AttackKind.None &&
                    rightKind == MantisLordStateMachine.AttackKind.None)
                    _phase2Leader = _left;
                else if (rightKind != MantisLordStateMachine.AttackKind.None &&
                        leftKind == MantisLordStateMachine.AttackKind.None)
                    _phase2Leader = _right;
                else
                    return;  // neither attacking yet, or both somehow started — wait

                _phase2FollowerCommanded = false;
                _phase2FollowerDelay = 0f;
            }

            // We have a leader — handle the follower
            if (_phase2FollowerCommanded) return;

            var leader   = _phase2Leader;
            var follow   = (leader == _left) ? _right : _left;
            var leadKind = leader.StateMachine.CurrentAttackKind;

            switch (leadKind)
            {
                case MantisLordStateMachine.AttackKind.Wall:
                    // Sync immediately — both attack the wall together
                    follow.StateMachine.CommandForceWallAttack();
                    _phase2FollowerCommanded = true;
                    break;

                case MantisLordStateMachine.AttackKind.Dash:
                    // Wait until leader is in leave phase, then follower does DStab
                    if (leader.StateMachine.IsInLeavePhase)
                    {
                        _phase2FollowerDelay += dt;
                        if (_phase2FollowerDelay >= FollowerDelayAfterLead)
                        {
                            follow.StateMachine.CommandForceDStabAttack();
                            _phase2FollowerCommanded = true;
                        }
                    }
                    break;

                case MantisLordStateMachine.AttackKind.DStab:
                    // Wait until leader is in leave phase, then follower does Dash
                    if (leader.StateMachine.IsInLeavePhase)
                    {
                        _phase2FollowerDelay += dt;
                        if (_phase2FollowerDelay >= FollowerDelayAfterLead)
                        {
                            follow.StateMachine.CommandForceDashAttack();
                            _phase2FollowerCommanded = true;
                        }
                    }
                    break;
            }
        }

        private void ResetPhase2()
        {
            _phase2Leader = null;
            _phase2FollowerCommanded = false;
            _phase2FollowerDelay = 0f;
            _wallAttackTimer = 0f;
        }

        

        private void UpdatePhase3Solo(float dt)
        {
            _wallAttackTimer += dt;

            // if (_wallAttackTimer >= EnemyConstants.MantisWallAttackInterval)
            // {
            //     _wallAttackTimer = 0f;
            //     // Force wall attack on whichever lord is still fighting
            //     if (!_left.StateMachine.IsInWoundedPose)
            //         _left.StateMachine.CommandForceWallAttack();
            //     else if (!_right.StateMachine.IsInWoundedPose)
            //         _right.StateMachine.CommandForceWallAttack();
            // }

            bool leftWounded  = _left.StateMachine.IsInWoundedPose;
            bool rightWounded = _right.StateMachine.IsInWoundedPose;

            if (leftWounded && rightWounded)
                BeginVictoryBow();
        }

        private void BeginVictoryBow()
        {
            _phase = BossFightPhase.Victory_Bow;
            _left.StateMachine.CommandBow();
            _middle.StateMachine.CommandBow();
            _right.StateMachine.CommandBow();
            // TODO: health-bar UI — hide boss health bar here
        }

        private void UpdateVictoryBow()
        {
            if (_left.StateMachine.IsBowComplete &&
                _middle.StateMachine.IsBowComplete &&
                _right.StateMachine.IsBowComplete)
            {
                _phase = BossFightPhase.Done;
                _game.SetWin();
            }
        }
    }
}
