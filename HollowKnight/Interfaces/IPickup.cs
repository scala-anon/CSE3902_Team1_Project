using System.Data;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Interfaces;

public interface IPickup : ICollidable
{
    public void Update(GameTime gameTime);

    /// <param name="opacity">Normalized in [0,1]; multiplied into the sprite's color tint. 1.0 = fully opaque (default). Caller is responsible for staying in range.</param>
    public void Draw(SpriteBatch spriteBatch, float layerDepth = 0f, float opacity = GameConstants.DefaultSpriteOpacity);

}
