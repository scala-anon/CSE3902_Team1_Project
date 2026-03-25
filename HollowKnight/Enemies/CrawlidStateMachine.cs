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

    public CrawlidStateMachine(Crawlid enemy)
    {
        CurrentCrawlid = enemy;
    }

    public void ChangeHealth()
    {
        CurrentCrawlid.Health--;
        if (CurrentCrawlid.Health <= 0)
        {
            CurrentCrawlid.Alive = false;
            CurrentCrawlid.SetState(CurrentCrawlid.IsGrounded
                ? CrawlidState.DeathLand
                : CrawlidState.DeathAir);
        }
    }

    public void Update(GameTime gameTime)
    {
        if (!CurrentCrawlid.Alive || CurrentCrawlid.IsDamaged) return;
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isTurning)
        {
            _turnTimer += elapsedTime;
            if (_turnTimer >= TurnDuration)
            {
                _isTurning = false;
                _turnTimer = 0f;
                CurrentCrawlid.SetState(CrawlidState.Idle);
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
        CurrentCrawlid.SetState(CrawlidState.Turn);
    }

    public string GetStateName() => CurrentCrawlid.State.ToString();
}
}
