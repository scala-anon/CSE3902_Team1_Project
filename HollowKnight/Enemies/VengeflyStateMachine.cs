using HollowKnight.Shared;
using HollowKnight.Pathfinding;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using HollowKnight.Audio;

namespace HollowKnight.Enemies
{
public class VengeflyStateMachine
{
    private Vengefly CurrentVengeFly;

    private const float DetectionRadius = GameConstants.VengeflyDetectionRadius;
    private const float PatrolSpeed = GameConstants.VengeflyPatrolSpeed;
    private const float ChaseSpeed = GameConstants.VengeflyChaseSpeed;
    private const double StartleDuration = GameConstants.VengeflyStartleDuration;

    private Direction _patrolDirection = Direction.Right;
    private double _startleTimer = 0;
    private NavigationGrid _grid;
    private List<Vector2> _currentPath = new List<Vector2>();
    private float _pathUpdateTimer = 0f;

    public void SetNavigationGrid(NavigationGrid grid) { _grid = grid; }
    public List<Vector2> GetCurrentPath() { return _currentPath; }

    public VengeflyStateMachine(Vengefly vengeFly)
    {
        CurrentVengeFly = vengeFly;
    }

    public float GetDetectionRadius() => DetectionRadius;

    public void ChangeHealth()
    {
        CurrentVengeFly.Health--;
        SoundEffect damage = AudioLoader.Instance.Get_Enemy_Damage();
        AudioManager.Instance.PlaySoundEffect(damage, GameConstants.MaxVolume, GameConstants.Pitch, GameConstants.Pan, false);
        if (CurrentVengeFly.Health <= 0)
        {
            CurrentVengeFly.Dead = true;
            CurrentVengeFly.SetState(VengeflyState.Death);
        }
    }

    public void Update(GameTime gameTime)
    {
        if (CurrentVengeFly.Dead || CurrentVengeFly.IsDamaged) return;
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 enemyCenter = CurrentVengeFly.GetBounds()[0].Center.ToVector2();
        float distanceFromKnight = Vector2.Distance(enemyCenter, CurrentVengeFly.knightPosition);
        bool knightInRange = distanceFromKnight <= DetectionRadius;

        // State transitions
        if (knightInRange && CurrentVengeFly.State == VengeflyState.Idle)
        {
            CurrentVengeFly.SetState(VengeflyState.Startle);
            _startleTimer = 0;
        }
        else if (CurrentVengeFly.State == VengeflyState.Startle)
        {
            _startleTimer += elapsedTime;
            if (_startleTimer >= StartleDuration)
                CurrentVengeFly.SetState(VengeflyState.Chase);
        }
        else if (CurrentVengeFly.State == VengeflyState.Chase && !knightInRange)
        {
            CurrentVengeFly.SetState(VengeflyState.Idle);
        }

        // Movement
        if (CurrentVengeFly.State == VengeflyState.Idle)
        {
            CurrentVengeFly.position.X += (_patrolDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
            CurrentVengeFly.FacingDirection = _patrolDirection;
            float spriteWidth = CurrentVengeFly.Sprite.GetSize().X;

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
        else if (CurrentVengeFly.State == VengeflyState.Chase)
        {
            _pathUpdateTimer += elapsedTime;

            // Try A* if grid exists and both positions are within grid bounds
            bool useAStar = false;
            if (_grid != null && _pathUpdateTimer >= GameConstants.VengeflyPathUpdateInterval || _currentPath.Count == 0)
            {
                _pathUpdateTimer = 0f;
                int gridPixelWidth = _grid.cols * _grid.cellSize;
                int gridPixelHeight = _grid.rows * _grid.cellSize;

                bool enemyInGrid = enemyCenter.X >= 0 && enemyCenter.X < gridPixelWidth
                                && enemyCenter.Y >= 0 && enemyCenter.Y < gridPixelHeight;
                bool knightInGrid = CurrentVengeFly.knightPosition.X >= 0 && CurrentVengeFly.knightPosition.X < gridPixelWidth
                                 && CurrentVengeFly.knightPosition.Y >= 0 && CurrentVengeFly.knightPosition.Y < gridPixelHeight;

                if (enemyInGrid && knightInGrid)
                {
                    _currentPath = AStarPathFinder.FindPath(_grid, enemyCenter, CurrentVengeFly.knightPosition);
                    useAStar = _currentPath.Count > 0;
                }
                else
                {
                    _currentPath.Clear();
                }
            }
            else if (_currentPath.Count > 0)
            {
                useAStar = true;
            }

            if (useAStar && _currentPath.Count > 0)
            {
                Vector2 targetWaypoint = _currentPath[0];
                if (Vector2.Distance(enemyCenter, targetWaypoint) < GameConstants.PathReachedThreshold)
                {
                    _currentPath.RemoveAt(0);
                    if (_currentPath.Count > 0) targetWaypoint = _currentPath[0];
                }

                Vector2 dir = targetWaypoint - enemyCenter;
                if (dir != Vector2.Zero)
                {
                    dir.Normalize();
                    CurrentVengeFly.position += dir * ChaseSpeed * elapsedTime;
                    if (dir.X > 0) CurrentVengeFly.FacingDirection = Direction.Right;
                    else if (dir.X < 0) CurrentVengeFly.FacingDirection = Direction.Left;
                }
            }
            else
            {
                // Straight-line fallback
                Vector2 dir = CurrentVengeFly.knightPosition - enemyCenter;
                if (dir != Vector2.Zero)
                {
                    dir.Normalize();
                    CurrentVengeFly.position += dir * ChaseSpeed * elapsedTime;
                    if (dir.X > 0) CurrentVengeFly.FacingDirection = Direction.Right;
                    else if (dir.X < 0) CurrentVengeFly.FacingDirection = Direction.Left;
                }
            }
        }

        CurrentVengeFly.Sprite.SetPosition(CurrentVengeFly.position);
    }

    public string GetStateName() => CurrentVengeFly.State.ToString();
}
}
