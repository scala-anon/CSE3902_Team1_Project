using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Tutorial_Platform_8: IObject
{
    private ISprite Sprite;

    public Vector2 position;

    public Tutorial_Platform_8()
    {
        Sprite = SpriteFactory.Instance.CreateTutorial_Platform_8Sprite(position);
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