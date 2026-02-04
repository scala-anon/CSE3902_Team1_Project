using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Interfaces
{
    /// <summary>
    /// Interface for all drawable and updatable game sprites.
    /// Implement this for players, enemies, items, projectiles, etc.
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Update sprite logic (animation frames, movement, etc.)
        /// Called once per game tick.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw the sprite to the screen.
        /// Called once per render frame.
        /// </summary>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Reset sprite to initial state (position, animation frame, etc.)
        /// </summary>
        void Reset();

        /// <summary>
        /// Set the sprite's position in world coordinates.
        /// </summary>
        void SetPosition(Vector2 position);

        /// <summary>
        /// Get the sprite's current position.
        /// </summary>
        Vector2 GetPosition();
    }
}
