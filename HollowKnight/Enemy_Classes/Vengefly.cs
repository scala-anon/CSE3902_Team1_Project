
using System;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Vengefly : IEnemy, ICollidable
{
    public Rectangle[] hitBoxes = new Rectangle[1];
    private int frameCounter = 0;
    public int state = 0;
    public bool dead;
    public bool startleAnimationPlayed;
    public bool left;
    public bool knightFound;

    //TODO change this default knight position to something more reasonable
    public Vector2 knightPosition = new Vector2(-9999, -9999);

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

    public void SetKnightPosition(Vector2 knightPosition) => this.knightPosition = knightPosition;
    public float GetDetectionRadius() => stateMachine.GetDetectionRadius();
    public bool IsActive => true;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, VengeflySprite.Width, VengeflySprite.Height);

/* TODO Fix this later
    public void changeDirection()
    {
        stateMachine.ChangeDirection();
    }


    public void changeMovingState()
    {
        stateMachine.ChangeMovingState();
    }
*/
    public void ChangeHealth()
    {
        stateMachine.changeHealth();
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        VengeflySprite.Draw(_spriteBatch, _spriteEffects);
    }

    public void Update(GameTime _gameTime)
    {
        stateMachine.Update(_gameTime);
        VengeflySprite.Update(_gameTime);
    }

    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }
    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle[] GetBounds()
    {
        Vector2 size = VengeflySprite.GetSize();
        //Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, VengeflySprite.Width, VengeflySprite.Height);
        hitBoxes[0] = Bounds;
        //Console.WriteLine($"Vengefly Bounds: {rectangle}");
        return hitBoxes;
    }
}
