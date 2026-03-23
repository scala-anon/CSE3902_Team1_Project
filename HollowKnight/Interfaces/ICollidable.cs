using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces
{
    /// Takes in 4 parameter: x, y, width, height
    public interface ICollidableTEMP //TODO: fix this e should only have 1 ICollidable interface
    {
        Rectangle[] GetBounds();
        bool IsActive { get; }
    }
}
