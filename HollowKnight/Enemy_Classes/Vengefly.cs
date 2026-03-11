
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Vengefly : IEnemy, ICollidable
{
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;

    private bool _isDamaged;
    private double _damagedTimer;
    private const double DamagedDuration = 0.4;

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

    public void TakeDamage()
    {
        if (_isDamaged) return;
        _isDamaged = true;
        _damagedTimer = 0;
        ChangeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        VengeflySprite.Draw(_spriteBatch, _spriteEffects);
    }

    public void Update(GameTime _gameTime)
    {
        if (_isDamaged)
        {
            _damagedTimer += _gameTime.ElapsedGameTime.TotalSeconds;
            if (_damagedTimer >= DamagedDuration)
            {
                _isDamaged = false;
                _damagedTimer = 0;
            }
        }

        stateMachine.Update(_gameTime);
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
