
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Ability_Classes;

public class Spirit : IPickup
{
    private SpriteEffects effect;
    private ISprite sprite;
    public bool IsActive { get; set; } = true;
    public bool Collected;
    private Vector2 position;
    public Rectangle[] hitBoxes = new Rectangle[1];
    public Rectangle Bounds { get; }
    public Spirit(Vector2 _position)
    {
        Collected = false;
        position = _position;
        sprite = SpriteFactory.Instance.CreateSpiritInitialSprite(position);
    }

    public void Update(GameTime gameTime)
    {
        sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, effect);
    }


    public Rectangle[] GetBounds()
    {
        Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 100, 100);
        hitBoxes[0] = rectangle;
        return hitBoxes;
    }

}