using Microsoft.Xna.Framework;

namespace HollowKnight.Collision
{
    // TODO: Extract collision registration and per-frame detection from Game1 into this class.
    // Should own: CollisionHandler, all Register<> calls (currently in Game1.LoadContent),
    // and the per-frame collision loops (currently in Game1.Update lines ~170-230).
    // Game1 should call collisionSystem.Update(gameTime) once per frame.
    public class CollisionSystem
    {
    }
}
