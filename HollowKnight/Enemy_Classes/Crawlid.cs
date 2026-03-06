
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Crawlid : IEnemy
{
    public int state = 0;

    private CrawlidStateMachine stateMachine;

    public ISprite CrawlidSprite;

    public bool alive;

    public Vector2 position;

    // Crawlid patrols surfaces and turns at edges — it does not chase the knight
    public Crawlid(Vector2 _position)
    {
        position = _position;
        alive = true;
        CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
        stateMachine = new CrawlidStateMachine(this);
    }

    // Crawlid does not react to the knight — required by IEnemy interface
    public void SetKnightPosition(Vector2 knightPosition) { }
    public float GetDetectionRadius() => 0f;

    public void ChangeHealth()
    {
        stateMachine.ChangeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        CrawlidSprite.Draw(_spriteBatch, _spriteEffects);
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
