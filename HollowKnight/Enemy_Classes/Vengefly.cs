
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Vengefly : IEnemy, ICollidable
{
    public Rectangle[] hitBoxes;
    private int frameCounter = 0;
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;
    private VengeflyStateMachine stateMachine;
    public ISprite VengeFly;
    public Vector2 position;
    public Vengefly(Vector2 _positon)
    {
        position = _positon;
        dead = false;
        startleAnimationPlayed = true;
        knightFound = false;
        left = true;
        VengeFly = SpriteFactory.Instance.CreateVengeflyIdleSprite(position);
        stateMachine = new VengeflyStateMachine(this);
    }

    public bool IsActive => true;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, VengeFly.Width, VengeFly.Height);

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
        VengeFly.Draw(_spriteBatch, _spriteEffects);
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
        
        VengeFly.Update(_gameTime);

    }

    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle[] GetBounds()
    {
        Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 48, 32);
        hitBoxes[0] = rectangle;
        return hitBoxes;
    }
}