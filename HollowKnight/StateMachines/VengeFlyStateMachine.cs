
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Pathfinding;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private Vengefly CurrentVengeFly;

    private const float DetectionRadius = 500f;
    private const float PatrolSpeed = 75f;
    private const float ChaseSpeed = 100f;
    private const double StartleDuration = 0.5;

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
            CurrentVengeFly.state = 3;
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
            if (knightInRange && CurrentVengeFly.state == 0)
            {
                CurrentVengeFly.state = 1;
                CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyStartleSprite(CurrentVengeFly.position);
                _startleTimer = 0;
            }
            else if (CurrentVengeFly.state == 1)
            {
                _startleTimer += elapsedTime;
                if (_startleTimer >= StartleDuration)
                {
                    CurrentVengeFly.state = 2;
                    CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyChaseSprite(CurrentVengeFly.position);
                }
            }
            else if (CurrentVengeFly.state == 2 && !knightInRange)
            {
                CurrentVengeFly.state = 0;
                CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(CurrentVengeFly.position);
            }
        }

        // Movement
        if (CurrentVengeFly.state == 0)
        {
            CurrentVengeFly.position.X += (_patrolDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
            CurrentVengeFly.facingDirection = _patrolDirection;
            float spriteWidth = CurrentVengeFly.VengeflySprite.GetSize().X;

            if (CurrentVengeFly.position.X + spriteWidth >= 1280)
            {
                CurrentVengeFly.position.X = 1280 - spriteWidth;
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
        else if (CurrentVengeFly.state == 2)
        {
            _pathUpdateTimer += elapsedTime;
            
            // Recalculate path every 0.3 seconds or if we don't have a path
            if (_pathUpdateTimer >= 0.3f || _currentPath.Count == 0)
            {
                _pathUpdateTimer = 0f;
                if (_grid != null) {
                    _currentPath = AStarPathFinder.FindPath(_grid, enemyCenter, CurrentVengeFly.knightPosition);
                }
            }
            
            if (_currentPath.Count > 0)
            {
                Vector2 targetWaypoint = _currentPath[0];
                if (Vector2.Distance(enemyCenter, targetWaypoint) < 10f)
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

    public string GetStateName()
    {
        return CurrentVengeFly.state switch
        {
            0 => "Idle",
            1 => "Startle",
            2 => "Chase",
            3 => "Death",
            _ => "Unknown"
        };
    }
}
