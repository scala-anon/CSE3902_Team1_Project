using HollowKnight.Player;
using Microsoft.Xna.Framework;

namespace HollowKnight.Interfaces
{
    public enum InteractionType
    {
        None,
        ButtonPress,
        SwordHit,
        Touch
    }

    public interface IInteractable : IObject
    {
        InteractionType InteractionType { get; }
        Rectangle[] GetInteractionBounds();
        bool IsInteractable(TheKnight knight);
        void OnInteract(TheKnight knight);
    }
}