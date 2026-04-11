using Microsoft.Xna.Framework;
namespace HollowKnight.Interfaces
{
    public interface IInteractable
    {
        Rectangle Bounds { get; }
        Rectangle[] GetBounds();
    }
}