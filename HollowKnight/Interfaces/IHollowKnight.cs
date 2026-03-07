using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;

namespace HollowKnight.Interfaces
{
    public interface IHollowKnight : ICollidable
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void MoveRight();
        void MoveLeft();
        void MoveUp();
        void MoveDown();
        void Jump();
        void TakeDamage();
        void StopMovingHorizontal();
        void StopMovingVertical();
        void SideSlash();
        void UpSlash();
        void DownSlash();
    }
}