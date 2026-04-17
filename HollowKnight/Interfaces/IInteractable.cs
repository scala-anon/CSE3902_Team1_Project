using System;
using HollowKnight.Player;
using Microsoft.Xna.Framework;
namespace HollowKnight.Interfaces
{

    public enum InteractionType
    {
        None,
        KeyBoardInput,
        SwordHit,
        Touch
    }
    public interface IInteractable
    {
        InteractionType InteractionType { get; }
        Rectangle GetInteractionBounds();
        bool IsInteractable(TheKnight knight);
        void OnInteract(TheKnight knight);
    }
}