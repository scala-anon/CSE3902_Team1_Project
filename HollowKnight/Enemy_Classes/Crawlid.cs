using System;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Crawlid : IEnemy, HollowKnight.Interfaces.ICollidable
{
    public int state = 0;

    private CrawlidStateMachine stateMachine;

    public ISprite CrawlidSprite;

    public bool alive = true;
    public int health = 3;

    public bool IsDamaged => _isDamaged;

    private bool _isDamaged;
    private double _damagedTimer;
    private const double DamagedDuration = 0.4;

    private Vector2 _knockbackVelocity;
    private const float KnockbackSpeed = 950f; //TODO: edit this to make it closer to the actual game
    private const float KnockbackDecay = 8f;
    private const float DeathGravity = 600f;
    private const float ScreenFloor = 720f;

    public bool IsGrounded { get; private set; } = true;
    public bool IsActive => alive;

    public Vector2 position;

    public Direction facingDirection = Direction.Right;

    // Crawlid patrols surfaces and turns at edges — it does not chase the knight
    public Crawlid(Vector2 _position)
    {
        position = _position;
        CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
        stateMachine = new CrawlidStateMachine(this);
    }
    
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, CrawlidSprite.Width, CrawlidSprite.Height);

    // Crawlid does not react to the knight — required by IEnemy interface
    public void SetKnightPosition(Vector2 knightPosition) { }
    public float GetDetectionRadius() => 0f;

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

        // Set knockback and grounded state before ChangeHealth so death sprite picks correctly
        switch (side)
        {
            case CollisionSide.Left: _knockbackVelocity = new Vector2(-KnockbackSpeed, 0f); break;
            case CollisionSide.Right: _knockbackVelocity = new Vector2(KnockbackSpeed, 0f); break;
            case CollisionSide.Top: _knockbackVelocity = new Vector2(0, 0f); break; //no vertical knockback
            case CollisionSide.Bottom: _knockbackVelocity = new Vector2(0, 0f); break; //no vertical knockback, should not be possible to be hit from the bottom
        }
        if (_knockbackVelocity.Y < 0) IsGrounded = false;

        ChangeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {

        SpriteEffects effects = facingDirection == Direction.Right
        ? SpriteEffects.FlipHorizontally
        : SpriteEffects.None;
        CrawlidSprite.Draw(_spriteBatch, effects);
    }

    public Rectangle GetBounds()
    {
        Vector2 size = CrawlidSprite.GetSize();
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }

    public void Update(GameTime _gameTime)
    {
        float dt = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        if (!alive)
        {
            if (!IsGrounded)
            {
                _knockbackVelocity.Y += DeathGravity * dt;
                _knockbackVelocity.X *= (1f - KnockbackDecay * dt);
                if (Math.Abs(_knockbackVelocity.X) < 1f) _knockbackVelocity.X = 0;
                
                position += _knockbackVelocity * dt;

                float spriteHeight = CrawlidSprite.GetSize().Y;
                if (position.Y + spriteHeight >= ScreenFloor)
                {
                    position.Y = ScreenFloor - spriteHeight;
                    _knockbackVelocity = Vector2.Zero;
                    IsGrounded = true;
                    state = 3;
                    CrawlidSprite = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(position);
                }
            }
            CrawlidSprite.SetPosition(position);
            CrawlidSprite.Update(_gameTime);
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
        CrawlidSprite.SetPosition(position);
        CrawlidSprite.Update(_gameTime);
    }
}
