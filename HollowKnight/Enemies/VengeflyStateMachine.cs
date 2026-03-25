using HollowKnight.Factories;
using HollowKnight.Shared;
using HollowKnight.Pathfinding;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

    public VengeflyStateMachine(Vengefly _vengeFly)
    {
        CurrentVengeFly = _vengeFly;
    }

    public float GetDetectionRadius() => DetectionRadius;

    public void changeHealth()
    {
        CurrentVengeFly.health--;
        if (CurrentVengeFly.health <= 0)
        {
            CurrentVengeFly.dead = true;
            CurrentVengeFly.state = VengeflyState.Death;
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyDeathSprite(CurrentVengeFly.position);
        }
    }


    public void Update(GameTime _gameTime)
    {
        if (CurrentVengeFly.dead || CurrentVengeFly.IsDamaged) return;
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 enemyCenter = CurrentVengeFly.GetBounds()[0].Center.ToVector2();
        float distanceFromKnight = Vector2.Distance(enemyCenter, CurrentVengeFly.knightPosition);
        bool knightInRange = distanceFromKnight <= DetectionRadius;

        // State transitions
        if (!CurrentVengeFly.dead)
        {
            if (knightInRange && CurrentVengeFly.state == VengeflyState.Idle)
            {
                CurrentVengeFly.state = VengeflyState.Startle;
                CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyStartleSprite(CurrentVengeFly.position);
                _startleTimer = 0;
            }
            else if (CurrentVengeFly.state == VengeflyState.Startle)
            {
                _startleTimer += elapsedTime;
                if (_startleTimer >= StartleDuration)
                {
                    CurrentVengeFly.state = VengeflyState.Chase;
                    CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyChaseSprite(CurrentVengeFly.position);
                }
            }
            else if (CurrentVengeFly.state == VengeflyState.Chase && !knightInRange)
            {
                CurrentVengeFly.state = VengeflyState.Idle;
                CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(CurrentVengeFly.position);
            }
        }

        // Movement
        if (CurrentVengeFly.state == VengeflyState.Idle)
        {
            CurrentVengeFly.position.X += (_patrolDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
            CurrentVengeFly.facingDirection = _patrolDirection;
            float spriteWidth = CurrentVengeFly.VengeflySprite.GetSize().X;

            if (CurrentVengeFly.position.X + spriteWidth >= GameConstants.DefaultLevelWidth)
            {
                CurrentVengeFly.position.X = GameConstants.DefaultLevelWidth - spriteWidth;
                _patrolDirection = Direction.Left;
                CurrentVengeFly.facingDirection = Direction.Left;
            }
            else if (CurrentVengeFly.position.X <= 0)
            {
                CurrentVengeFly.position.X = 0;
                _patrolDirection = Direction.Right;
                CurrentVengeFly.facingDirection = Direction.Right;
            }
        }
        else if (CurrentVengeFly.state == VengeflyState.Chase)
        {
            _pathUpdateTimer += elapsedTime;
            
            // Recalculate path every 0.3 seconds or if we don't have a path
            if (_pathUpdateTimer >= GameConstants.VengeflyPathUpdateInterval || _currentPath.Count == 0)
            {
                _pathUpdateTimer = 0f;
                if (_grid != null) {
                    _currentPath = AStarPathFinder.FindPath(_grid, enemyCenter, CurrentVengeFly.knightPosition);
                }
            }
            
            if (_currentPath.Count > 0)
            {
                Vector2 targetWaypoint = _currentPath[0];
                if (Vector2.Distance(enemyCenter, targetWaypoint) < GameConstants.PathReachedThreshold)
                {
                    _currentPath.RemoveAt(0); // reached knight
                    if (_currentPath.Count > 0) targetWaypoint = _currentPath[0];
                }

                Vector2 dir = targetWaypoint - enemyCenter;
                if (dir != Vector2.Zero)
                {
                    dir.Normalize();
                    CurrentVengeFly.position += dir * ChaseSpeed * elapsedTime;
                    if (dir.X > 0)
                        CurrentVengeFly.facingDirection = Direction.Right;
                    else if (dir.X < 0)
                        CurrentVengeFly.facingDirection = Direction.Left;
                }
            } else {
                 // Fallback to straight line if no path found
                 Vector2 dir = CurrentVengeFly.knightPosition - enemyCenter;
                 if (dir != Vector2.Zero)
                 {
                    dir.Normalize();
                    CurrentVengeFly.position += dir * ChaseSpeed * elapsedTime;
                    if (dir.X > 0)
                        CurrentVengeFly.facingDirection = Direction.Right;
                    else if (dir.X < 0)
                        CurrentVengeFly.facingDirection = Direction.Left;
                 }
            }
        }

        CurrentVengeFly.VengeflySprite.SetPosition(CurrentVengeFly.position);
    }

    public string GetStateName() => CurrentVengeFly.state.ToString();
}
}
