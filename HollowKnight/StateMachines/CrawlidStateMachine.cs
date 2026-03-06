using HollowKnight.Factories;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

public class CrawlidStateMachine
{
    private Crawlid CurrentCrawlid;

    private const float PatrolSpeed = 60f; // TODO: change speed accordingly
    private Direction _movementDirection = Direction.Right;

    public CrawlidStateMachine(Crawlid _enemy)
    {
        CurrentCrawlid = _enemy;
    }

    public void ChangeHealth()
    {
        CurrentCrawlid.alive = !CurrentCrawlid.alive;
    }

    public void Update(GameTime _gameTime)
    {
        // TODO: Add wall/edge detection to trigger Turn state while patrolling
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        CurrentCrawlid.position.X += (_movementDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
        float spriteWidth = CurrentCrawlid.CrawlidSprite.GetSize().X;

        if (CurrentCrawlid.position.X + spriteWidth >= 1280)
        {
            CurrentCrawlid.position.X = 1280 - spriteWidth;
            _movementDirection = Direction.Left;
        }
        else if (CurrentCrawlid.position.X <= 0)
        {
            CurrentCrawlid.position.X = 0;
            _movementDirection = Direction.Right;
        }

        CurrentCrawlid.CrawlidSprite.SetPosition(CurrentCrawlid.position);
    }

    public string GetStateName()
    {
        return CurrentCrawlid.state switch
        {
            0 => "Idle",
            1 => "Turn",
            2 => "DeathAir",
            3 => "DeathLand",
            _ => "Unknown"
        };
    }
}
