using System;
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Pathfinding;

namespace HollowKnight.Enemies
{
public enum CrawlidState
{
    Idle,
    Turn,
    DeathAir,
    DeathLand
}

public class Crawlid : IEnemy
{
    public CrawlidState State { get; set; } = CrawlidState.Idle;

    private CrawlidStateMachine stateMachine;
    private readonly Dictionary<CrawlidState, ISprite> sprites;

    public ISprite Sprite { get; private set; }

    public bool Alive { get; set; } = true;
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

    public bool IsGrounded { get; private set; } = true;
    public bool IsActive => Alive;

    public Vector2 position;

    public Direction FacingDirection { get; set; } = Direction.Right;

    private Rectangle[] hitBoxes = new Rectangle[1];

    public Crawlid(Vector2 position)
    {
        this.position = position;
        sprites = new Dictionary<CrawlidState, ISprite>
        {
            [CrawlidState.Idle] = SpriteFactory.Instance.CreateCrawlidIdleSprite(position),
            [CrawlidState.Turn] = SpriteFactory.Instance.CreateCrawlidTurnSprite(position),
            [CrawlidState.DeathAir] = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(position),
            [CrawlidState.DeathLand] = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(position),
        };
        Sprite = sprites[CrawlidState.Idle];
        stateMachine = new CrawlidStateMachine(this);
    }

    public void SetState(CrawlidState newState)
    {
        State = newState;
        Sprite = sprites[newState];
    }

    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);

    public void SetKnightPosition(Vector2 knightPosition) { }
    public void SetNavigationGrid(NavigationGrid grid) { }
    public List<Vector2> GetCurrentPath() { return null; }
    public float GetDetectionRadius() => 0f;
    public float GetChaseRadius() => 0f;
    public void SetPlatform(IObject platform) => stateMachine.SetPlatform(platform);

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
            case CollisionSide.Left: _knockbackVelocity = new Vector2(-KnockbackSpeed, 0f); break;
            case CollisionSide.Right: _knockbackVelocity = new Vector2(KnockbackSpeed, 0f); break;
            case CollisionSide.Top: _knockbackVelocity = new Vector2(0, 0f); break;
            case CollisionSide.Bottom: _knockbackVelocity = new Vector2(0, 0f); break;
        }
        if (_knockbackVelocity.Y < 0) IsGrounded = false;

        ChangeHealth();
    }

    public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
    {
        SpriteEffects effects = FacingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;
        Sprite.Draw(spriteBatch, effects);
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

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!Alive)
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
                    SetState(CrawlidState.DeathLand);
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
}
}
