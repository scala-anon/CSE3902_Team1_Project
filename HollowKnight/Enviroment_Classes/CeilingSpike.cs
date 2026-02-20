using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CeilingSpike : IObject
{
    public ISprite Sprite;

    public Vector2 position;

    public CeilingSpike()
    {
        Sprite = SpriteFactory.Instance.CreateSpikeCeilingSprite(position);
    }

    public void Update(GameTime _gameTime)
    {
        Sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        Sprite.Draw(_spriteBatch);
    } 
}