using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Shared;

namespace HollowKnight.Interfaces
{
    public interface IPlayer : HollowKnight.Collision.ICollidable
    {
        void Update(GameTime gameTime);
        /// <param name="opacity">Normalized in [0,1]; multiplied into the sprite's color tint. 1.0 = fully opaque (default). Caller is responsible for staying in range.</param>
        void Draw(SpriteBatch spriteBatch, float layerDepth = 0f, float opacity = GameConstants.DefaultSpriteOpacity);
        void MoveRight();
        void MoveLeft();
        void MoveUp();
        void MoveDown();
        void Jump();
        void TakeDamage(CollisionSide side);
        void StopMovingHorizontal();
        void StopMovingVertical();
        void SideSlash();
        void UpSlash();
        void DownSlash();
        Rectangle GetHurtbox();
    }
}