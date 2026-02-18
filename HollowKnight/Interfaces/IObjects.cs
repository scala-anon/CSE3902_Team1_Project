using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Interfaces
{
    public interface IObject
    {
        public void Update(GameTime _gameTime);

        public void Draw(SpriteBatch _spriteBatch);
    }
}