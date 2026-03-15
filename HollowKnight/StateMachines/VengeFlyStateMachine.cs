
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private Vengefly CurrentVengeFly;

    private const float DetectionRadius = 500f;
    private const float PatrolSpeed = 50f;
    private const float ChaseSpeed = 75f;
    private const double StartleDuration = 0.5;

    private Direction _patrolDirection = Direction.Right;
    private double _startleTimer = 0;

    public VengeflyStateMachine(Vengefly _vengeFly)
    {
        CurrentVengeFly = _vengeFly;
    }

    public float GetDetectionRadius() => DetectionRadius;

    public void changeHealth()
    {
        CurrentVengeFly.health--;
        if (CurrentVengeFly.health <= 0)
            CurrentVengeFly.dead = true;
    }

    public void Update(GameTime _gameTime)
    {
        if (CurrentVengeFly.dead || CurrentVengeFly.IsDamaged) return;
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 enemyCenter = CurrentVengeFly.GetBounds().Center.ToVector2();
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
            float spriteWidth = CurrentVengeFly.VengeflySprite.GetSize().X;

            if (CurrentVengeFly.position.X + spriteWidth >= 1280)
            {
                CurrentVengeFly.position.X = 1280 - spriteWidth;
                _patrolDirection = Direction.Left;
            }
            else if (CurrentVengeFly.position.X <= 0)
            {
                CurrentVengeFly.position.X = 0;
                _patrolDirection = Direction.Right;
            }
        }
        else if (CurrentVengeFly.state == 2)
        {
            Vector2 dir = CurrentVengeFly.knightPosition - enemyCenter;
            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                CurrentVengeFly.position += dir * ChaseSpeed * elapsedTime;
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
