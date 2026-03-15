using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;

public interface IEnemy : ICollidable
{
    public void Update(GameTime _gameTime);

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects);

    string GetStateName();

    void SetKnightPosition(Vector2 knightPosition); // Enemies need to know where knight is

    float GetDetectionRadius(); //Detection radius for different enemy states

    void TakeDamage();
    void TakeDamage(CollisionSide side);
}