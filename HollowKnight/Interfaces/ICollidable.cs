using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces
{
    /// Takes in 4 parameter: x, y, width, height
    public interface ICollidable
    {
        Rectangle[] GetBounds();
    }
}
