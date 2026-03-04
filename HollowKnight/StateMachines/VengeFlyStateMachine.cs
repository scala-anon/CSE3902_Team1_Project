
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private Vengefly CurrentVengeFly;

    public VengeflyStateMachine(Vengefly _vengeFly)
    {
        CurrentVengeFly = _vengeFly;
    }
    
    public void ChangeDirection()
    {
        CurrentVengeFly.left = !CurrentVengeFly.left;
        
    }

    public void changeHealth()
    {
        CurrentVengeFly.dead = !CurrentVengeFly.dead; // alive -> dead
    }
    
    public void ChangeMovingState()
    {
        CurrentVengeFly.knightFound = !CurrentVengeFly.knightFound;
    }

    public void startle()
    {
        CurrentVengeFly.startleAnimationPlayed = !CurrentVengeFly.startleAnimationPlayed;
    }


    public void Update(GameTime _gameTime)
    {
        if (CurrentVengeFly.state == 1)
        {
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyStartleSprite(CurrentVengeFly.position);
           
        }
        if (CurrentVengeFly.state == 2)
        {
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyChaseSprite(CurrentVengeFly.position);
        }

        if (CurrentVengeFly.state == 3)
        {
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyDeathSprite(CurrentVengeFly.position);
        }

        if (CurrentVengeFly.state == 4)
        {   
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(CurrentVengeFly.position);
        }

        if (CurrentVengeFly.state == 5)
        {
            CurrentVengeFly.VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(CurrentVengeFly.position);
        }
    }
    
    public string GetStateName()
    {
        return CurrentVengeFly.state switch
        {
            0 => "Idle",
            1 => "Startle",
            2 => "Chase",
            3 => "Death",
            _ => "Unknown"
        };
    }

}