
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Vengefly : IEnemy
{
    private int frameCounter = 0;
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;
    
    private VengeflyStateMachine stateMachine;
    public ISprite VengeflySprite;
    public Vector2 position;
    
    public Vengefly(Vector2 _positon)
    {
        position = _positon;
        dead = false;
        startleAnimationPlayed = true;
        knightFound = false;
        left = true;
        VengeflySprite = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);
        stateMachine = new VengeflyStateMachine(this);
    }

    public void changeDirection()
    {
        stateMachine.ChangeDirection();
    }


    public void changeMovingState()
    {
        stateMachine.ChangeMovingState();
    }

    public void ChangeHealth()
    {
        stateMachine.changeHealth();
    }

    public void Startle()
    {
        stateMachine.startle();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        VengeflySprite.Draw(_spriteBatch, _spriteEffects);
    }

    public void Update(GameTime _gameTime)
    {

        //Start idle

        //TODO impliment actuall state changes
        frameCounter++;
        if (frameCounter >= 500)
        {
            state++;
            stateMachine.Update(_gameTime);
            if(state == 5)
            {
                state = 0;
            }
            frameCounter = 0;
        }
        
        VengeflySprite.Update(_gameTime);

    }

    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle GetBounds()
    {
        Vector2 size = VengeflySprite.GetSize();
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }
}