using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;

public interface IEnemy : ICollidable
{
    public void Update(GameTime _gameTime);

    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects);

    string GetStateName();

    void SetKnightPosition(Vector2 knightPosition); 
    
    void SetNavigationGrid(NavigationGrid grid); // Enemies need to know the layout for A*

    List<Vector2> GetCurrentPath(); // Exposes the pathway for debug visualization

    Rectangle GetHurtbox(); // hitbox for taking/dealing damage

    float GetDetectionRadius(); //Detection radius for different enemy states

    void TakeDamage();
    void TakeDamage(CollisionSide side);
}