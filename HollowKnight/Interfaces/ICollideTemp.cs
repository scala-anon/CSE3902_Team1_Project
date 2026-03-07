using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces;

public interface ICollideTemp
{
    Rectangle Bounds { get; }
    bool IsActive { get; }
}