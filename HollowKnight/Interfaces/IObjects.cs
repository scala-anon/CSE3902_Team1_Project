using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Shared;

namespace HollowKnight.Interfaces
{
    public interface IObject : HollowKnight.Collision.ICollidable
    {
        public string Label { get; }
        public void Update(GameTime _gameTime);

        /// <param name="opacity">Normalized in [0,1]; multiplied into the sprite's color tint. 1.0 = fully opaque (default). Caller is responsible for staying in range.</param>
        public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects, float layerDepth = 0f, float opacity = GameConstants.DefaultSpriteOpacity);
    }
}