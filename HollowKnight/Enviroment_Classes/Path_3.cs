using System.IO;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Path_3 : IObject
{
    private ISprite Sprite;

    public Vector2 position;

    public Path_3()
    {
        Sprite = SpriteFactory.Instance.CreatePath_Stone_3Sprite(position);
    }

    public void Update(GameTime _gameTime)
    {
        Sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        Sprite.Draw(_spriteBatch, _spriteEffects);
    }

    // TODO: Tune width/height to match the actual scaled sprite size
    public Rectangle GetBounds()
    {
        return new Rectangle((int)position.X, (int)position.Y, 128, 32);
    }
}