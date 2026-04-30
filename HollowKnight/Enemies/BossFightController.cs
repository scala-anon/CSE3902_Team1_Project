using System;
using System.Collections.Generic;
using HollowKnight.Audio;
using HollowKnight.Environment;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

// Camera lives in the root HollowKnight namespace
using HollowKnight;

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

        private readonly List<InvisibleBarrier> _spawnedBarriers = new();

        // field
        private bool _paused = false;

        // method
        public void TogglePause() => _paused = !_paused;
        
        // Add fields
        private MantisLord _phase2Leader;
        private MantisLord _phase2Follower;
        private bool _phase2FollowerCommanded;
        private float _phase2FollowerDelay;
        private const float FollowerDelayAfterLead = 0.0f;
        private bool _phase2RolesInitialized;
        private static readonly System.Random _phase2Rng = new System.Random();
        private const float DoubleDashChance = 0.5f;  // tune to taste
        private bool _phase2DashRollMade;
        private bool _phase2DoMirrorDash;
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
            EngageArenaLock();
            _phase = BossFightPhase.Phase1_Middle;
            AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Mantis_Lords());
            _middle.StateMachine.CommandActivate();
        }

        public void Update(GameTime gameTime)
        {

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
            Console.WriteLine(
                $"[Phase2] L:state={_left.State},attacking={_left.StateMachine.IsAttacking},pos={_left.position} " +
                $"R:state={_right.State},attacking={_right.StateMachine.IsAttacking},pos={_right.position} " +
                $"leader={_phase2Leader?.Slot},commanded={_phase2FollowerCommanded}");
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


        private void UpdatePhase2Sides(float dt)
        {
            // ---- Wounded checks (unchanged) ----
            bool leftWounded  = _left.StateMachine.IsInWoundedPose;
            bool rightWounded = _right.StateMachine.IsInWoundedPose;
            if (leftWounded && !rightWounded) { _phase = BossFightPhase.Phase3_Solo; ResetPhase2(); return; }
            if (rightWounded && !leftWounded) { _phase = BossFightPhase.Phase3_Solo; ResetPhase2(); return; }
            if (leftWounded && rightWounded)  { BeginVictoryBow(); return; }

            if (!_phase2RolesInitialized)
            {
                AssignRoles(leaderIsLeft: true);
                _phase2RolesInitialized = true;
            }

            bool leaderAttacking   = _phase2Leader.StateMachine.IsAttacking;
            bool followerAttacking = _phase2Follower.StateMachine.IsAttacking;

            // Cycle complete: follower has finished its commanded action.
            // Leader may still be mid-attack — that's fine, it becomes the new follower.
            if (_phase2FollowerCommanded && !followerAttacking)
            {
                AssignRoles(leaderIsLeft: _phase2Leader == _right);
                return;
            }

            // Leader hasn't started attacking yet — wait
            if (!leaderAttacking) return;

            // Don't force-command the new follower if it's still finishing a previous attack
            if (followerAttacking) return;

            // Follower already commanded for this cycle — wait for cycle to finish
            if (_phase2FollowerCommanded) return;

            // Leader is mid-attack; figure out what to tell the follower
            var leadKind = _phase2Leader.StateMachine.CurrentAttackKind;
            switch (leadKind)
            {
                case MantisLordStateMachine.AttackKind.Wall:
                    _phase2Follower.StateMachine.CommandForceWallAttack();
                    _phase2FollowerCommanded = true;
                    break;

                case MantisLordStateMachine.AttackKind.Dash:
                    if (!_phase2DashRollMade)
                    {
                        _phase2DoMirrorDash = _phase2Rng.NextDouble() < DoubleDashChance;
                        _phase2DashRollMade = true;
                    }
                    if (_phase2DoMirrorDash)
                    {
                        _phase2Follower.StateMachine.CommandForceDashAttack();
                        _phase2FollowerCommanded = true;
                    }
                    else
                    {
                        if (_phase2Leader.StateMachine.IsInLeavePhase)
                        {
                            _phase2FollowerDelay += dt;
                            if (_phase2FollowerDelay >= FollowerDelayAfterLead)
                            {
                                _phase2Follower.StateMachine.CommandForceDStabAttack();
                                _phase2FollowerCommanded = true;
                            }
                        }
                    }
                    break;

                case MantisLordStateMachine.AttackKind.DStab:
                    if (_phase2Leader.StateMachine.IsInLeavePhase)
                    {
                        _phase2FollowerDelay += dt;
                        if (_phase2FollowerDelay >= FollowerDelayAfterLead)
                        {
                            _phase2Follower.StateMachine.CommandForceDashAttack();
                            _phase2FollowerCommanded = true;
                        }
                    }
                    break;
            }
        }

        private void AssignRoles(bool leaderIsLeft)
        {
            _phase2Leader   = leaderIsLeft ? _left  : _right;
            _phase2Follower = leaderIsLeft ? _right : _left;
            if (_phase2Leader == null || _phase2Follower == null ||
                _phase2Leader.StateMachine == null || _phase2Follower.StateMachine == null)
            {
                Console.WriteLine($"[AssignRoles] BAILING: leader={_phase2Leader}, leaderSM={_phase2Leader?.StateMachine}, follower={_phase2Follower}, followerSM={_phase2Follower?.StateMachine}");
                return;
            }
            _phase2Leader.StateMachine.SetAutoPickSuppressed(false);
            _phase2Follower.StateMachine.SetAutoPickSuppressed(true);
            _phase2FollowerCommanded = false;
            _phase2FollowerDelay = 0f;
            _phase2DashRollMade = false;   
            _phase2DoMirrorDash = false;   
        }

        private void ResetPhase2()
        {
            // Phase 3 takes over — un-suppress whichever lord is still alive
            _left.StateMachine.SetAutoPickSuppressed(false);
            _right.StateMachine.SetAutoPickSuppressed(false);
            _phase2Leader = null;
            _phase2Follower = null;
            _phase2FollowerCommanded = false;
            _phase2FollowerDelay = 0f;
            _phase2RolesInitialized = false;
            _wallAttackTimer = 0f;
        }

        

        private void UpdatePhase3Solo(float dt)
        {
            _wallAttackTimer += dt;

            bool leftWounded  = _left.StateMachine.IsInWoundedPose;
            bool rightWounded = _right.StateMachine.IsInWoundedPose;

            if (leftWounded && rightWounded)
                BeginVictoryBow();
        }

        private void EngageArenaLock()
        {
            var leftBarrier = new InvisibleBarrier(
                new Vector2(BossArenaConstants.LeftBarrierX - BossArenaConstants.BarrierWidth / 2f, BossArenaConstants.BarrierTopY),
                BossArenaConstants.BarrierWidth,
                BossArenaConstants.BarrierHeight);

            var rightBarrier = new InvisibleBarrier(
                new Vector2(BossArenaConstants.RightBarrierX - BossArenaConstants.BarrierWidth / 2f, BossArenaConstants.BarrierTopY),
                BossArenaConstants.BarrierWidth,
                BossArenaConstants.BarrierHeight);

            _game.LevelPlatforms.Add(leftBarrier);
            _game.LevelPlatforms.Add(rightBarrier);
            _spawnedBarriers.Add(leftBarrier);
            _spawnedBarriers.Add(rightBarrier);

            Camera.Instance.EnterBossClamp(BossArenaConstants.CameraClampCenter);
        }

        private void DisengageArenaLock()
        {
            foreach (var barrier in _spawnedBarriers)
                _game.LevelPlatforms.Remove(barrier);
            _spawnedBarriers.Clear();
            Camera.Instance.ExitBossClamp();
        }

        private void BeginVictoryBow()
        {
            _phase = BossFightPhase.Victory_Bow;
            _left.StateMachine.CommandBow();
            _middle.StateMachine.CommandBow();
            _right.StateMachine.CommandBow();
        }

        private void UpdateVictoryBow()
        {
            if (_left.StateMachine.IsBowComplete &&
                _middle.StateMachine.IsBowComplete &&
                _right.StateMachine.IsBowComplete)
            {
                AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Win());
                DisengageArenaLock();
                _phase = BossFightPhase.Done;
                _game.SetWin();
            }
        }
    }
}
