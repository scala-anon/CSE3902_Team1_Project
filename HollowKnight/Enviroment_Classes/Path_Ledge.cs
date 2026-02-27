using System.IO;
using HollowKnight.Factories;
using HollowKnight.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Path_ledge : IObject
{
    public ISprite sprite;

    public Vector2 position;

    public Path_ledge()
    {
        sprite = SpriteFactory.Instance.CreatePath_LedgeSprite(position);
    }

    public void Update(GameTime _gameTime)
    {
        sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        sprite.Draw(_spriteBatch, _spriteEffects);
    }

    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle GetBounds()
    {
        return new Rectangle((int)position.X, (int)position.Y, 128, 32);
    }
}