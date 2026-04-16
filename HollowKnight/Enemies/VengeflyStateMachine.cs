using HollowKnight.Shared;
using HollowKnight.Pathfinding;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HollowKnight.Audio;

namespace HollowKnight.Enemies
{
    public class VengeflyStateMachine
    {
        private Vengefly CurrentVengeFly;

        private Direction _patrolDirection = Direction.Right;
        private double _startleTimer = 0;
        private NavigationGrid _grid;
        private List<Vector2> _currentPath = new List<Vector2>();
        private float _pathUpdateTimer = 0f;
        private int _consecutivePathFailures = 0;
        private float _currentBackoffDelay = 0f;
        private int frameCounter = 0;

        public void SetNavigationGrid(NavigationGrid grid) => _grid = grid;
        public List<Vector2> GetCurrentPath() => _currentPath;

        public VengeflyStateMachine(Vengefly vengeFly)
        {
            CurrentVengeFly = vengeFly;
        }

        public float GetDetectionRadius() => EnemyConstants.VengeflyDetectionRadius;
        public float GetChaseRadius() => EnemyConstants.VengeflyChaseRadius;

        public void ChangeHealth()
        {
            CurrentVengeFly.Health--;
            AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Enemy_Damage());
            if (CurrentVengeFly.Health <= 0)
            {
                CurrentVengeFly.Dead = true;
                CurrentVengeFly.SetState(CurrentVengeFly.IsGrounded
                    ? VengeflyState.DeathLand
                    : VengeflyState.DeathAir);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentVengeFly.Dead || CurrentVengeFly.IsDamaged) return;
            if (frameCounter % 120 == 0)
            {
                AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Vengefly_Fly());
            }
            frameCounter++;

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 enemyCenter = CurrentVengeFly.GetCenter();
            float distanceFromKnight = Vector2.Distance(enemyCenter, CurrentVengeFly.knightPosition);
            bool knightInDetectionRange = distanceFromKnight <= EnemyConstants.VengeflyDetectionRadius;
            bool knightInChaseRange = distanceFromKnight <= EnemyConstants.VengeflyChaseRadius;

            UpdateStateTransitions(knightInDetectionRange, knightInChaseRange, elapsedTime);
            UpdateMovement(elapsedTime, enemyCenter);
            CurrentVengeFly.UpdateSpritePosition();
        }

        private void UpdateStateTransitions(bool knightInDetectionRange, bool knightInChaseRange, float elapsedTime)
        {
            if (knightInDetectionRange && CurrentVengeFly.State == VengeflyState.Idle)
            {
                CurrentVengeFly.SetState(VengeflyState.Startle);
                _startleTimer = 0;
            }
            else if (CurrentVengeFly.State == VengeflyState.Startle)
            {
                _startleTimer += elapsedTime;
                if (_startleTimer >= EnemyConstants.VengeflyStartleDuration)
                    CurrentVengeFly.SetState(VengeflyState.Chase);
            }
            else if (CurrentVengeFly.State == VengeflyState.Chase && !knightInChaseRange)
            {
                CurrentVengeFly.SetState(VengeflyState.Idle);
                _currentPath.Clear();
                _consecutivePathFailures = 0;
                _currentBackoffDelay = 0f;
            }
        }

        private void UpdateMovement(float elapsedTime, Vector2 enemyCenter)
        {
            if (CurrentVengeFly.State == VengeflyState.Idle)
                UpdatePatrol(elapsedTime);
            else if (CurrentVengeFly.State == VengeflyState.Chase)
                UpdateChase(elapsedTime, enemyCenter);
        }

        private void UpdatePatrol(float elapsedTime)
        {
            float speed = _patrolDirection == Direction.Right
                ? EnemyConstants.VengeflyPatrolSpeed
                : -EnemyConstants.VengeflyPatrolSpeed;
            CurrentVengeFly.position.X += speed * elapsedTime;
            CurrentVengeFly.FacingDirection = _patrolDirection;

            float spriteWidth = CurrentVengeFly.SpriteSize.X;

            if (CurrentVengeFly.position.X + spriteWidth >= GameConstants.DefaultLevelWidth)
            {
                CurrentVengeFly.position.X = GameConstants.DefaultLevelWidth - spriteWidth;
                _patrolDirection = Direction.Left;
                CurrentVengeFly.FacingDirection = Direction.Left;
            }
            else if (CurrentVengeFly.position.X <= 0)
            {
                CurrentVengeFly.position.X = 0;
                _patrolDirection = Direction.Right;
                CurrentVengeFly.FacingDirection = Direction.Right;
            }
        }

        private void UpdateChase(float elapsedTime, Vector2 enemyCenter)
        {
            _pathUpdateTimer += elapsedTime;
            bool useAStar = TryUpdatePath(enemyCenter, elapsedTime);

            Vector2 dir;
            if (useAStar && _currentPath.Count > 0)
            {
                Vector2 targetWaypoint = _currentPath[0];
                float reachRadius = System.Math.Max(
                    GameConstants.PathReachedThreshold,
                    CurrentVengeFly.Bounds.Width / EnemyConstants.VengeflyWaypointReachDivisor);

                if (Vector2.Distance(enemyCenter, targetWaypoint) < reachRadius)
                {
                    _currentPath.RemoveAt(0);
                    if (_currentPath.Count > 0) targetWaypoint = _currentPath[0];
                }

                dir = targetWaypoint - enemyCenter;
            }
            else
            {
                dir = CurrentVengeFly.knightPosition - enemyCenter;
            }

            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                CurrentVengeFly.position += dir * EnemyConstants.VengeflyChaseSpeed * elapsedTime;
                if (dir.X > 0) CurrentVengeFly.FacingDirection = Direction.Right;
                else if (dir.X < 0) CurrentVengeFly.FacingDirection = Direction.Left;
            }
        }

        private bool TryUpdatePath(Vector2 enemyCenter, float elapsedTime)
        {
            if (_grid == null) return false;

            _currentBackoffDelay = System.Math.Max(0f, _currentBackoffDelay - elapsedTime);

            if (_currentBackoffDelay <= 0f &&
                (_pathUpdateTimer >= EnemyConstants.VengeflyPathUpdateInterval || _currentPath.Count == 0))
            {
                _pathUpdateTimer = 0f;
                int gridPixelWidth = _grid.cols * _grid.cellSize;
                int gridPixelHeight = _grid.rows * _grid.cellSize;
                Vector2 knightPos = CurrentVengeFly.knightPosition;

                bool enemyInGrid = enemyCenter.X >= 0 && enemyCenter.X < gridPixelWidth
                                && enemyCenter.Y >= 0 && enemyCenter.Y < gridPixelHeight;
                bool knightInGrid = knightPos.X >= 0 && knightPos.X < gridPixelWidth
                                 && knightPos.Y >= 0 && knightPos.Y < gridPixelHeight;

                if (enemyInGrid && knightInGrid)
                {
                    Vector2 enemySize = new Vector2(CurrentVengeFly.Bounds.Width, CurrentVengeFly.Bounds.Height);
                    _currentPath = AStarPathFinder.FindPath(_grid, enemyCenter, knightPos, enemySize);

                    if (_currentPath.Count > 0)
                    {
                        _consecutivePathFailures = 0;
                        _currentBackoffDelay = 0f;
                        return true;
                    }

                    _consecutivePathFailures++;
                    if (_consecutivePathFailures >= EnemyConstants.VengeflyPathFailuresBeforeBackoff)
                    {
                        int failuresSinceBackoff = _consecutivePathFailures - EnemyConstants.VengeflyPathFailuresBeforeBackoff;
                        float next = EnemyConstants.VengeflyPathFailBackoffInitial * (float)System.Math.Pow(
                            EnemyConstants.VengeflyPathFailBackoffMultiplier,
                            failuresSinceBackoff);
                        _currentBackoffDelay = System.Math.Min(next, EnemyConstants.VengeflyPathFailBackoffMax);
                    }
                    return false;
                }

                _currentPath.Clear();
                return false;
            }

            return _currentPath.Count > 0;
        }

        public string GetStateName() => CurrentVengeFly.State.ToString();
    }
}
