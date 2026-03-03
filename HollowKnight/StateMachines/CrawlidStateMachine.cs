using HollowKnight.Factories;
using Microsoft.Xna.Framework;

public class CrawlidStateMachine
{
    private Crawlid CurrentCrawlid;
    public CrawlidStateMachine(Crawlid _enemy)
    {
        CurrentCrawlid = _enemy;
    }


    public void Direction()
    {
        CurrentCrawlid.left = !CurrentCrawlid.left;
    }

    public void ChangeHealth()
    {
        CurrentCrawlid.alive = !CurrentCrawlid.alive;
    }


    public void Update(GameTime _gameTime)
    {
        if (CurrentCrawlid.state == 1)
        {
            CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidTurnSprite(CurrentCrawlid.position);

        }
        
        if (CurrentCrawlid.state == 2)
        {
            CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidDeathAirSprite(CurrentCrawlid.position);
        }

        if (CurrentCrawlid.state == 3)
        {
            CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidDeathLandSprite(CurrentCrawlid.position);
        }

        if (CurrentCrawlid.state == 4)
        {
            CurrentCrawlid.Sprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(CurrentCrawlid.position);
        }
    }

public string GetStateName()
    {
        return CurrentCrawlid.state switch
        {
            0 => "Idle",
            1 => "Startle",
            2 => "Chase",
            3 => "Death",
            _ => "Unknown"
        };
    }
    

}