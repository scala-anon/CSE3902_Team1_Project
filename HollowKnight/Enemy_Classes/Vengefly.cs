
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

public class Vengefly : IEnemy
{
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;

    private static readonly Dictionary<string, int> VengeflyStates = new()
    {
        { "Idle",    0 },
        { "Startle", 1 },
        { "Chase",   2 },
        { "Death",   3 }
    };

    //TODO change this default knight position to something more reasonable
    private Vector2 _knightPosition = new Vector2(-9999, -9999);

    private const float DetectionRadius = 500f; // TODO:  change range, detection is almost half of screen
    private double _startleTimer = 0;
    private const double StartleDuration = 0.5;

    private const float PatrolSpeed = 50f; // TODO: change speed accordingly
    private const float ChaseSpeed = 75f; // TODO: change speed accordingly

    private Direction _patrolDirection = Direction.Right;

    private VengeflyStateMachine stateMachine;
    public ISprite VengeflySprite;
    public Vector2 position;

    public Vengefly(Vector2 _positon)
    {
        position = _positon;
        dead = false;
        startleAnimationPlayed = true;
        knightFound = false;
        left = true;
        VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);
        stateMachine = new VengeflyStateMachine(this);
    }

    public void SetKnightPosition(Vector2 knightPosition) => _knightPosition = knightPosition;
    public float GetDetectionRadius() => DetectionRadius;

    public void changeDirection()
    {
        stateMachine.ChangeDirection();
    }

    public void changeMovingState()
    {
        stateMachine.ChangeMovingState();
    }

    public void ChangeHealth()
    {
        stateMachine.changeHealth();
    }

    public void Startle()
    {
        stateMachine.startle();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        VengeflySprite.Draw(_spriteBatch, _spriteEffects);
    }

    public void Update(GameTime _gameTime)
    {
        Vector2 enemyCenter = GetBounds().Center.ToVector2();
        float distanceFromKnight = Vector2.Distance(enemyCenter, _knightPosition);
        bool knightInDetectionRange = distanceFromKnight <= DetectionRadius;

        if (!dead)
        {
            if (knightInDetectionRange && state == VengeflyStates["Idle"])
            {
                state = VengeflyStates["Startle"];
                stateMachine.Update(_gameTime);
                _startleTimer = 0;
            }
            else if (state == VengeflyStates["Startle"])
            {
                _startleTimer += _gameTime.ElapsedGameTime.TotalSeconds;
                if (_startleTimer >= StartleDuration)
                {
                    state = VengeflyStates["Chase"];
                    stateMachine.Update(_gameTime);
                }
            }
            else if (state == VengeflyStates["Chase"] && !knightInDetectionRange)
            {
                state = VengeflyStates["Idle"];
                VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);
            }
        }

        //& UPDATE simple vengefly movement unti A* implemented
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        if (state == VengeflyStates["Idle"])
        {
            position.X += (_patrolDirection == Direction.Right ? -PatrolSpeed : PatrolSpeed) * elapsedTime;
            float spriteWidth = VengeflySprite.GetSize().X;

            if (position.X + spriteWidth >= 1280)
            {
                position.X = 1280 - spriteWidth;
                _patrolDirection = Direction.Left;
            }
            else if (position.X <= 0)
            {
                position.X = 0;
                _patrolDirection = Direction.Right;
            }
        }
        else if (state == VengeflyStates["Chase"])
        {
            Vector2 directionToKnight = _knightPosition - GetBounds().Center.ToVector2();
            if (directionToKnight != Vector2.Zero)
            {
                directionToKnight.Normalize();
                position += directionToKnight * ChaseSpeed * elapsedTime;
            }
        }
        VengeflySprite.SetPosition(position);

        VengeflySprite.Update(_gameTime);
    }

    public Rectangle GetBounds()
    {
        Vector2 size = VengeflySprite.GetSize();
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }
}
