
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Crawlid : IEnemy
{
    public int state = 0;

    private CrawlidStateMachine stateMachine;

    public ISprite CrawlidSprite;

    public bool left;

    public bool alive;

    public Vector2 position;

    // Crawlid patrols surfaces and turns at edges — it does not chase the knight
    private static readonly Dictionary<string, int> CrawlidStates = new()
    {
        { "Idle",      0 },
        { "Turn",      1 },  
        { "DeathAir",  2 },  // Crawlid death while airborne
        { "DeathLand", 3 }   // Crawlid death on ground
    };

    public Crawlid(Vector2 _position)
    {
        position = _position;
        left = true;
        alive = true;
        CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
        stateMachine = new CrawlidStateMachine(this);
    }

    // Crawlid does not react to the knight — required by IEnemy interface
    public void SetKnightPosition(Vector2 knightPosition) { }
    public float GetDetectionRadius() => 0f;

    public void Direction()
    {
        stateMachine.Direction();
    }

    public void ChangeHealth()
    {
        stateMachine.ChangeHealth();
    }

    public void Update(GameTime _gameTime)
    {
        // TODO: Add wall/edge detection to trigger Turn state while patrolling
        CrawlidSprite.Update(_gameTime);
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
}
