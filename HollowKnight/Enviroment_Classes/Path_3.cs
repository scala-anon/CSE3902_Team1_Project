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

    public void Draw(SpriteBatch _spriteBatch)
    {
        Sprite.Draw(_spriteBatch);
    }
}