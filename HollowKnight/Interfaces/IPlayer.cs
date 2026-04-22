using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;

namespace HollowKnight.Interfaces
{
    public interface IPlayer : HollowKnight.Collision.ICollidable
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, float layerDepth = 0f);
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