using Microsoft.Xna.Framework;

namespace HollowKnight.Collision
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }
        bool IsActive { get; }
        Rectangle[] GetBounds(); //TODO: look into array format vs not for this function
    }
}
