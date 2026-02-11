using System.Numerics;
using System.Runtime.CompilerServices;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework.Graphics;

public class Vengefly
{
    private VengeflyStateMachine stateMachine;
    public ISprite VengeFly;

    private Vector2 position;
    public Vengefly()
    {
        stateMachine = new VengeflyStateMachine();
        VengeFly = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);

    }

    public void changeDirection()
    {
        
    }
}