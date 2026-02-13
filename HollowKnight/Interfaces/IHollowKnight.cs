using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Interfaces
{
    public interface IHollowKnight
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
        void Attack();
    }
}