using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IEnemy
{
    public void Update(GameTime _gameTime);

    public void Draw(SpriteBatch _spriteBatch);
}