using System.Data;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Interfaces;

public interface IPickup : ICollidable
{
    public void Update(GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch, float layerDepth = 0f);

}
