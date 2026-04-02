using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;

namespace HollowKnight.Interfaces
{
    public interface IObject : HollowKnight.Collision.ICollidable
    {
        public string Label { get; }
        public void Update(GameTime _gameTime);

        public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects);
    }
}