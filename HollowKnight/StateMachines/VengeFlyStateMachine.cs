using HollowKnight.Interfaces;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private bool left;
    
    private bool KnightFound;

    public void ChangeDirection()
    {
        left = !left;
    }

    public void ChangeMovingState()
    {
        KnightFound = !KnightFound;
    }


    public void update()
    {
        
    }
    

}