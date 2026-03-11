using System.Runtime.CompilerServices;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Spike : IObject
{
    public ISprite Sprite;

    public Vector2 position;

    public Spike()
    {
        Sprite = SpriteFactory.Instance.CreateSpikeSprite(position);
    }

     public Spike(Vector2 pos)
    {
        position = pos;
        Sprite = SpriteFactory.Instance.CreateSpikeSprite(position);
    }

    public void Update(GameTime _gameTime)
    {
        Sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        Sprite.Draw(_spriteBatch, _spriteEffects);
    
    }
}