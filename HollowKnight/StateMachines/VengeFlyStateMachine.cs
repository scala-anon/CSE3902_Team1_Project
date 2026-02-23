
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private Vengefly CVengeFly;

    public VengeflyStateMachine(Vengefly _vengeFly)
    {
        CVengeFly = _vengeFly;
    }
    
    public void ChangeDirection()
    {
        CVengeFly.left = !CVengeFly.left;
        
    }

    public void changeHealth()
    {
        CVengeFly.dead = !CVengeFly.dead; // alive -> dead
    }
    
    public void ChangeMovingState()
    {
        CVengeFly.knightFound = !CVengeFly.knightFound;
    }

    public void startle()
    {
        CVengeFly.startleAnimationPlayed = !CVengeFly.startleAnimationPlayed;
    }


    public void Update(GameTime _gameTime)
    {
        if (CVengeFly.state == 1)
        {
            CVengeFly.VengeFly = SpriteFactory.Instance.CreateVengeflyStartleSprite(CVengeFly.position);
           
        }
        if (CVengeFly.state == 2)
        {
            CVengeFly.VengeFly = SpriteFactory.Instance.CreateVengeflyChaseSprite(CVengeFly.position);
        }

        if (CVengeFly.state == 3)
        {
            CVengeFly.VengeFly = SpriteFactory.Instance.CreateVengeflyDeathSprite(CVengeFly.position);
        }

        if (CVengeFly.state == 4)
        {   
            CVengeFly.VengeFly = SpriteFactory.Instance.CreateVengeflyIdleSprite(CVengeFly.position);
        }

        if (CVengeFly.state == 5)
        {
            CVengeFly.VengeFly = SpriteFactory.Instance.CreateVengeflyIdleSprite(CVengeFly.position);
        }
    }
    

}