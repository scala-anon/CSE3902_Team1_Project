
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Vengefly : IEnemy, HollowKnight.Interfaces.ICollidable
{
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;

    public int health = 5;
    public bool IsDamaged => _isDamaged;

    private bool _isDamaged;
    private double _damagedTimer;
    private const double DamagedDuration = 0.4;

    private Vector2 _knockbackVelocity;
    private const float KnockbackSpeed = 950f;
    private const float KnockbackDecay = 8f;

    //TODO change this default knight position to something more reasonable
    public Vector2 knightPosition = new Vector2(-9999, -9999);

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

    public void SetKnightPosition(Vector2 knightPosition) => this.knightPosition = knightPosition;
    public float GetDetectionRadius() => stateMachine.GetDetectionRadius();
    public bool IsActive => true;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, VengeflySprite.Width, VengeflySprite.Height);
    /*
        public void changeDirection()
        {
            stateMachine.ChangeDirection();
        }


        public void changeMovingState()
        {
            stateMachine.ChangeMovingState();
        }
    */
    public void ChangeHealth()
    {
        stateMachine.changeHealth();
    }

    public void TakeDamage() => TakeDamage(CollisionSide.None);

    public void TakeDamage(CollisionSide side)
    {
        if (_isDamaged) return;
        _isDamaged = true;
        _damagedTimer = 0;  
        switch (side)
        {
            case CollisionSide.Left: _knockbackVelocity = new Vector2(-KnockbackSpeed, -150f); break;
            case CollisionSide.Right: _knockbackVelocity = new Vector2(KnockbackSpeed, -150f); break;
            case CollisionSide.Top: _knockbackVelocity = new Vector2(0, -KnockbackSpeed); break;
            case CollisionSide.Bottom: _knockbackVelocity = new Vector2(0, KnockbackSpeed); break;
        }
        ChangeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        VengeflySprite.Draw(_spriteBatch, _spriteEffects);
    }

    public void Update(GameTime _gameTime)
    {
        float dt = (float)_gameTime.ElapsedGameTime.TotalSeconds;

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
