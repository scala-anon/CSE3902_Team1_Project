
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

    public ISprite Sprite;

    public bool left;

    public bool alive;

    public Vector2 position;

    public Crawlid(Vector2 _position)
    {
        position = _position;
        left = true;
        alive = true;
        Sprite = SpriteFactory.Instance.CreateCrawlidIdleSprite(position);
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
            if (state == 3)
            {
                state = 0;
            }
            frameCounter = 0;
        }

        Sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        Sprite.Draw(_spriteBatch);
    }

}