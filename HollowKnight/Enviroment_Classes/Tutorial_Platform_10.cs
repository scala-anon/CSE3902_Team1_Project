using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Tutorial_Platform_10: IObject
{
    private ISprite Sprite;

    public Vector2 position;

    public Rectangle[] hitBoxes = new Rectangle[1];

    public bool IsActive => true;
    public Rectangle Bounds { get; }

    public Tutorial_Platform_10()
    {
        Sprite = SpriteFactory.Instance.CreateTutorial_Platform_10Sprite(position);
    }

    public Tutorial_Platform_10(Vector2 pos)
    {
        position = pos;
        Sprite = SpriteFactory.Instance.CreateTutorial_Platform_10Sprite(position);
    }


    public void Update(GameTime _gameTime)
    {
        Sprite.Update(_gameTime);
    }

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects)
    {
        Sprite.Draw(_spriteBatch, _spriteEffects);
    }

    public Rectangle[] GetBounds()
    {
        Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 264, 79);
        hitBoxes[0] = rectangle;
        return hitBoxes;
    }
}