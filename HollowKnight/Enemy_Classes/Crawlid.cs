
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Crawlid : IEnemy, ICollidable
{
    public int state = 0;

    private CrawlidStateMachine stateMachine;

    public ISprite CrawlidSprite;

    public bool alive;

    public Vector2 position;

    public Direction facingDirection = Direction.Right;

    // Crawlid patrols surfaces and turns at edges — it does not chase the knight
    public Crawlid(Vector2 _position)
    {
        position = _position;
        alive = true;
        CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
        stateMachine = new CrawlidStateMachine(this);
    }
    public bool IsActive => true;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);

    // Crawlid does not react to the knight — required by IEnemy interface
    public void SetKnightPosition(Vector2 knightPosition) { }
    public float GetDetectionRadius() => 0f;

    public void ChangeHealth()
    {
        stateMachine.ChangeHealth();
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
        stateMachine.Update(_gameTime);
        CrawlidSprite.Update(_gameTime);
    }
}
