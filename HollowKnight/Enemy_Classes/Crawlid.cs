
using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Transactions;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Crawlid : IEnemy
{
    private int frameCounter = 0;
    public int state = 0;

    private CrawlidStateMachine stateMachine;

    public ISprite CrawlidSprite;

    public bool left;

    public bool alive;

    public Vector2 position;

    public Crawlid(Vector2 _position)
    {
        position = _position;
        left = true;
        alive = true;
        CrawlidSprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
        stateMachine = new CrawlidStateMachine(this);
    }


    public void Direction()
    {
        stateMachine.Direction();
    }

    public void ChangeHealth()
    {
        stateMachine.ChangeHealth();
    }


    public void Update(GameTime _gameTime)
    {

        //TODO impliment actuall state changes
        frameCounter++;
        if (frameCounter >= 500)
        {
            state++;
            stateMachine.Update(_gameTime);
            if (state == 4)
            {
                state = 0;
            }
            frameCounter = 0;
        }

        CrawlidSprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        CrawlidSprite.Draw(_spriteBatch, _spriteEffects);
    }

    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle GetBounds()
    {
        Vector2 size = CrawlidSprite.GetSize();
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }
    
    public string GetStateName()
    {
        return stateMachine.GetStateName();
    }

}