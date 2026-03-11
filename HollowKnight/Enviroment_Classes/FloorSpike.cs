using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FloorSpike : IObject
{
    public ISprite Sprite;

    public Vector2 position;

    public FloorSpike()
    {
        Sprite = SpriteFactory.Instance.CreateSpikeFloor2Sprite(position);
    }

    public FloorSpike(Vector2 pos)
    {
        position = pos;
        Sprite = SpriteFactory.Instance.CreateSpikeFloor2Sprite(position);
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