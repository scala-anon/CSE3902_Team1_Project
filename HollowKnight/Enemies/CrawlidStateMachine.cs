using System.Reflection;
using HollowKnight.Factories;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

public class CrawlidStateMachine
{
    private Crawlid CurrentCrawlid;

    private const float PatrolSpeed = 120f; // TODO: change speed accordingly
    private Direction _movementDirection = Direction.Right;
    private bool _isTurning = false;
    private float _turnTimer = 0f;
    private const float TurnDuration = 0.08f;

    public CrawlidStateMachine(Crawlid _enemy)
    {
        CurrentCrawlid = _enemy;
    }

    public void ChangeHealth()
    {
        CurrentCrawlid.health--;
        if (CurrentCrawlid.health <= 0)
        {
            CurrentCrawlid.alive = false;
            if (!CurrentCrawlid.IsGrounded)
            {
                CurrentCrawlid.state = 2;
                CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(CurrentCrawlid.position);
            }
            else
            {
                CurrentCrawlid.state = 3;
                CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(CurrentCrawlid.position);
            }
        }
    }

    public void Update(GameTime _gameTime)
    {
        if (!CurrentCrawlid.alive || CurrentCrawlid.IsDamaged) return;
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        if (_isTurning)
        {
            _turnTimer += elapsedTime;
            if (_turnTimer >= TurnDuration)
            {
                _isTurning = false;
                _turnTimer = 0f;
                CurrentCrawlid.state = 0;
                CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(CurrentCrawlid.position);
            }
            CurrentCrawlid.CrawlidSprite.SetPosition(CurrentCrawlid.position);
            return;
        }

        CurrentCrawlid.position.X += (_movementDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
        float spriteWidth = CurrentCrawlid.CrawlidSprite.GetSize().X;

        if (CurrentCrawlid.position.X + spriteWidth >= 3200)
        {
            CurrentCrawlid.position.X = 3200 - spriteWidth;
            _movementDirection = Direction.Left;
            CurrentCrawlid.facingDirection = Direction.Left;
            CrawlidTurn();
        }
        else if (CurrentCrawlid.position.X <= 0)
        {
            CurrentCrawlid.position.X = 0;
            _movementDirection = Direction.Right;
            CurrentCrawlid.facingDirection = Direction.Right;
            CrawlidTurn();
        }
        
        CurrentCrawlid.CrawlidSprite.SetPosition(CurrentCrawlid.position);
    }

    private void CrawlidTurn()
    {
        _isTurning = true;
        _turnTimer = 0f;
        CurrentCrawlid.state = 1;
        CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidTurnSprite(CurrentCrawlid.position);
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
