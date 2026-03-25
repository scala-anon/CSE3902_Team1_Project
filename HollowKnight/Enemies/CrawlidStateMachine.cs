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
        CurrentCrawlid.Health--;
        if (CurrentCrawlid.Health <= 0)
        {
            CurrentCrawlid.Alive = false;
            if (!CurrentCrawlid.IsGrounded)
            {
                CurrentCrawlid.State = CrawlidState.DeathAir;
                CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(CurrentCrawlid.position);
            }
            else
            {
                CurrentCrawlid.State = CrawlidState.DeathLand;
                CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(CurrentCrawlid.position);
            }
        }
    }

    public void Update(GameTime _gameTime)
    {
        if (!CurrentCrawlid.Alive || CurrentCrawlid.IsDamaged) return;
        float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

        if (_isTurning)
        {
            _turnTimer += elapsedTime;
            if (_turnTimer >= TurnDuration)
            {
                _isTurning = false;
                _turnTimer = 0f;
                CurrentCrawlid.State = CrawlidState.Idle;
                CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(CurrentCrawlid.position);
            }
            CurrentCrawlid.Sprite.SetPosition(CurrentCrawlid.position);
            return;
        }

        CurrentCrawlid.position.X += (_movementDirection == Direction.Right ? PatrolSpeed : -PatrolSpeed) * elapsedTime;
        float spriteWidth = CurrentCrawlid.Sprite.GetSize().X;

        if (CurrentCrawlid.position.X + spriteWidth >= GameConstants.DefaultLevelWidth)
        {
            CurrentCrawlid.position.X = GameConstants.DefaultLevelWidth - spriteWidth;
            _movementDirection = Direction.Left;
            CurrentCrawlid.FacingDirection = Direction.Left;
            CrawlidTurn();
        }
        else if (CurrentCrawlid.position.X <= 0)
        {
            CurrentCrawlid.position.X = 0;
            _movementDirection = Direction.Right;
            CurrentCrawlid.FacingDirection = Direction.Right;
            CrawlidTurn();
        }
        
        CurrentCrawlid.Sprite.SetPosition(CurrentCrawlid.position);
    }

    private void CrawlidTurn()
    {
        _isTurning = true;
        _turnTimer = 0f;
        CurrentCrawlid.State = CrawlidState.Turn;
        CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidTurnSprite(CurrentCrawlid.position);
    }

    public string GetStateName() => CurrentCrawlid.State.ToString();
}
}
