using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Path_1 : IObject
{
    private ISprite Sprite;

    public Vector2 position;

    public Path_1()
    {
        Sprite = SpriteFactory.Instance.CreatePath_1Sprite(position);
    }
    public Path_1(Vector2 Pos)
    {
        position = Pos;
        Sprite = SpriteFactory.Instance.CreatePath_1Sprite(position);
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