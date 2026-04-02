using System;
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    private readonly Dictionary<VengeflyState, ISprite> sprites;

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
    public ISprite Sprite { get; private set; }
    public Vector2 position;

    public Vengefly(Vector2 position)
    {
        this.position = position;
        sprites = new Dictionary<VengeflyState, ISprite>
        {
            [VengeflyState.Idle] = SpriteFactory.Instance.CreateVengeflyIdleSprite(position),
            [VengeflyState.Startle] = SpriteFactory.Instance.CreateVengeflyStartleSprite(position),
            [VengeflyState.Chase] = SpriteFactory.Instance.CreateVengeflyChaseSprite(position),
            [VengeflyState.Death] = SpriteFactory.Instance.CreateVengeflyDeathSprite(position),
        };
        Sprite = sprites[VengeflyState.Idle];
        stateMachine = new VengeflyStateMachine(this);
    }

    public void SetState(VengeflyState newState)
    {
        State = newState;
        Sprite = sprites[newState];
    }

    public void SetKnightPosition(Vector2 knightPosition) => this.knightPosition = knightPosition;
    public void SetNavigationGrid(NavigationGrid grid) => stateMachine.SetNavigationGrid(grid);
    public List<Vector2> GetCurrentPath() => stateMachine.GetCurrentPath();
    public float GetDetectionRadius() => stateMachine.GetDetectionRadius();
    public bool IsActive => !Dead;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);

    public void ChangeHealth() => stateMachine.ChangeHealth();

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

    public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
    {
        SpriteEffects effects = FacingDirection == Direction.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Sprite.Draw(spriteBatch, effects);
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            Sprite.Update(gameTime);
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

        stateMachine.Update(gameTime);
        Sprite.SetPosition(position);
        Sprite.Update(gameTime);
    }

    public Rectangle[] GetBounds()
    {
        Vector2 size = Sprite.GetSize();
        hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        return hitBoxes;
    }

    public Rectangle GetHurtbox()
    {
        Rectangle[] bounds = GetBounds();
        bounds[0].Inflate(GameConstants.EnemyHurtboxGrow, GameConstants.EnemyHurtboxGrow);
        return bounds[0];
    }

    public string GetStateName() => stateMachine.GetStateName();
}
}
