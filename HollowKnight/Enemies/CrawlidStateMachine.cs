using HollowKnight.Factories;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
public class CrawlidStateMachine
{
    private Crawlid CurrentCrawlid;

    private const float PatrolSpeed = GameConstants.CrawlidPatrolSpeed;
    private Direction _movementDirection = Direction.Right;
    private bool _isTurning = false;
    private float _turnTimer = 0f;
    private const float TurnDuration = GameConstants.CrawlidTurnDuration;

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
                CurrentCrawlid.state = CrawlidState.DeathAir;
                CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(CurrentCrawlid.position);
            }
            else
            {
                CurrentCrawlid.state = CrawlidState.DeathLand;
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
                CurrentCrawlid.state = CrawlidState.Idle;
                CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(CurrentCrawlid.position);
            }
            CurrentCrawlid.CrawlidSprite.SetPosition(CurrentCrawlid.position);
            return;
        }

        CurrentCrawlid.position.X += (_movementDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
        float spriteWidth = CurrentCrawlid.CrawlidSprite.GetSize().X;

        if (CurrentCrawlid.position.X + spriteWidth >= GameConstants.DefaultLevelWidth)
        {
            CurrentCrawlid.position.X = GameConstants.DefaultLevelWidth - spriteWidth;
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
        CurrentCrawlid.state = CrawlidState.Turn;
        CurrentCrawlid.CrawlidSprite = SpriteFactory.Instance.CreateCrawlidTurnSprite(CurrentCrawlid.position);
    }

    public string GetStateName() => CurrentCrawlid.state.ToString();
}
}
