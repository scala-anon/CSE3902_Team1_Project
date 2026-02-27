using Microsoft.Xna.Framework;

namespace HollowKnight.Collision
{
    /// Takes in 4 parameter: x, y, width, height
    public interface ICollidable
    {
        Rectangle GetBounds();
    }
}
