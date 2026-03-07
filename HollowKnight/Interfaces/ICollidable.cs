using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }
        bool IsActive { get; }
    }
}