using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CeilingSpike : IObject
{
    public ISprite Sprite;

    public Rectangle[] hitBoxes = new Rectangle[2];

    public Vector2 position;


    public CeilingSpike(Vector2 _position)
    {
        position = _position;
        Sprite = SpriteFactory.Instance.CreateSpikeCeilingSprite(_position);
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
    public Rectangle[] GetBounds()
    {
        Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 230, 70);
        Rectangle rectangle_2 = new Rectangle((int)position.X + 50, (int)position.Y + 70, 115, 60);
        hitBoxes[0] = rectangle;
        hitBoxes[1] = rectangle_2;
        return hitBoxes;
    }
}