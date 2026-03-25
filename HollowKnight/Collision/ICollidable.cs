using Microsoft.Xna.Framework;

namespace HollowKnight.Collision
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }
        bool IsActive { get; }
        Rectangle[] GetBounds();
    }
}
