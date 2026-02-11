using HollowKnight.Interfaces;
using Microsoft.Xna.Framework.Graphics;

public class VengeflyStateMachine
{
    private bool left;
    

    public void ChangeDirection()
    {
        left = !left;
    }

    

}