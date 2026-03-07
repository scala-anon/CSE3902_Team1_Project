using System.IO;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Path_3 : IObject, ICollidable
{
    private ISprite Sprite;

    public Vector2 position = new Vector2(400, 300);
    public bool IsActive => true;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);

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
}