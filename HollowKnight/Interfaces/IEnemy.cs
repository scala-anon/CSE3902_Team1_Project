using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HollowKnight.Collision;
using HollowKnight.Pathfinding;
using HollowKnight.Interfaces;
using HollowKnight.Shared;

public interface IEnemy : ICollidable
{
    public void Update(GameTime _gameTime);

    /// <param name="opacity">Normalized in [0,1]; multiplied into the sprite's color tint. 1.0 = fully opaque (default). Caller is responsible for staying in range.</param>
    public void Draw(SpriteBatch _spriteBatch, SpriteEffects _spriteEffects, float layerDepth = 0f, float opacity = GameConstants.DefaultSpriteOpacity);

    string GetStateName();

    void SetKnightPosition(Vector2 knightPosition); 
    
    void SetNavigationGrid(NavigationGrid grid); // Enemies need to know the layout for A*

    List<Vector2> GetCurrentPath(); // Exposes the pathway for debug visualization

    Rectangle GetHurtbox(); // hitbox for taking/dealing damage

    float GetDetectionRadius(); //Detection radius for different enemy states
    float GetChaseRadius(); //Chase radius for tracking out-of-range

    bool TakeDamage();
    bool TakeDamage(CollisionSide side);
    void SetPlatform(IObject platform);
}