using System;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Player;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HollowKnight.Enemies
{
public enum VengeflyState
{
    Idle,
    Startle,
    Chase,
    Death
}

public class Vengefly : IEnemy
{
    private Rectangle[] hitBoxes = new Rectangle[1];
    public VengeflyState State { get; set; } = VengeflyState.Idle;
    public bool Dead { get; set; }
    public Direction FacingDirection { get; set; } = Direction.Left;

    public int Health { get; set; } = GameConstants.EnemyDefaultHealth;
    public bool IsDamaged => _isDamaged;

    private bool _isDamaged;
    private double _damagedTimer;
    private const double DamagedDuration = GameConstants.EnemyDamagedDuration;

    private Vector2 _knockbackVelocity;
    private const float KnockbackSpeed = GameConstants.EnemyKnockbackSpeed;
    private const float KnockbackDecay = GameConstants.EnemyKnockbackDecay;
    private const float DeathGravity = GameConstants.EnemyDeathGravity;
    private const float ScreenFloor = GameConstants.ScreenHeight;
    public bool IsGrounded { get; private set; } = false;

    public Vector2 knightPosition = new Vector2(-9999, -9999);

    private VengeflyStateMachine stateMachine;
    public ISprite Sprite { get; set; }
    public Vector2 position;

    public Vengefly(Vector2 position)
    {
        this.position = position;
        Sprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);
        stateMachine = new VengeflyStateMachine(this);
    }

    public void SetKnightPosition(Vector2 knightPosition) => this.knightPosition = knightPosition;

    public void SetNavigationGrid(NavigationGrid grid)
    {
        stateMachine.SetNavigationGrid(grid);
    }

    public List<Vector2> GetCurrentPath()
    {
        return stateMachine.GetCurrentPath();
    }
    public float GetDetectionRadius() => stateMachine.GetDetectionRadius();
    public bool IsActive => !Dead;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);
    public void ChangeHealth()
    {
        stateMachine.ChangeHealth();
    }

    public void TakeDamage() => TakeDamage(CollisionSide.None);

    public void TakeDamage(CollisionSide side)
    {
        if (_isDamaged) return;
        _isDamaged = true;
        _damagedTimer = 0;
        switch (side)
        {
            case CollisionSide.Left: _knockbackVelocity = new Vector2(-KnockbackSpeed, GameConstants.VengeflyKnockbackUpComponent); break;
            case CollisionSide.Right: _knockbackVelocity = new Vector2(KnockbackSpeed, GameConstants.VengeflyKnockbackUpComponent); break;
            case CollisionSide.Top: _knockbackVelocity = new Vector2(0, -KnockbackSpeed); break;
            case CollisionSide.Bottom: _knockbackVelocity = new Vector2(0, KnockbackSpeed); break;
        }
        ChangeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        SpriteEffects effects = FacingDirection == Direction.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Sprite.Draw(_spriteBatch, effects);
    }

    public void Update(GameTime _gameTime)
    {
        float dt = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        if (Dead)
        {
            if (!IsGrounded)
            {
                _knockbackVelocity.Y += DeathGravity * dt;
                _knockbackVelocity.X *= (1f - KnockbackDecay * dt);
                if (Math.Abs(_knockbackVelocity.X) < 1f) _knockbackVelocity.X = 0;

                position += _knockbackVelocity * dt;

                float spriteHeight = Sprite.GetSize().Y;
                if (position.Y + spriteHeight >= ScreenFloor)
                {
                    position.Y = ScreenFloor - spriteHeight;
                    _knockbackVelocity = Vector2.Zero;
                    IsGrounded = true;
                }
            }

            Sprite.SetPosition(position);
            Sprite.Update(_gameTime);
            return;
        }

        if (_isDamaged)
        {
            _damagedTimer += dt;
            if (_damagedTimer >= DamagedDuration)
            {
                _isDamaged = false;
                _damagedTimer = 0;
            }
        }

        if (_knockbackVelocity != Vector2.Zero)
        {
            position += _knockbackVelocity * dt;
            _knockbackVelocity *= (1f - KnockbackDecay * dt);
            if (_knockbackVelocity.Length() < 1f)
                _knockbackVelocity = Vector2.Zero;
        }

        stateMachine.Update(_gameTime);
        Sprite.SetPosition(position);
        Sprite.Update(_gameTime);
    }

    public Rectangle[] GetBounds()
    {
        Vector2 size = Sprite.GetSize();
        hitBoxes[0] =  new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        return hitBoxes;
    }

    //bigger hitbox for enemy collision
    public Rectangle GetHurtbox()
    {
        Rectangle[] bounds = GetBounds();
        bounds[0].Inflate(GameConstants.EnemyHurtboxGrow, GameConstants.EnemyHurtboxGrow);
        return bounds[0];
    }

    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }
}
}
