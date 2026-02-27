using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;

public interface IEnemy : ICollidable
{
    public void Update(GameTime _gameTime);

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects);
}