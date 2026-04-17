using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Bench : BaseEnvironmentObject, IInteractable
    {
        public override string Label => "Bench";
        public InteractionType InteractionType => InteractionType.ButtonPress;

        public Bench(Vector2 pos)
        {
            position = pos;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateBenchSprite(position);
        }

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 100, 32);
            return hitBoxes;
        }

        public Rectangle[] GetInteractionBounds()
        {
            return new[]
            {
                new Rectangle((int)position.X - 16, (int)position.Y - 12, 132, 56)
            };
        }

        public bool IsInteractable(TheKnight knight)
        {
            return GetInteractionBounds()[0].Intersects(knight.GetBounds()[0]);
        }

        public void OnInteract(TheKnight knight)
        {
            DebugLogger.LogObject("Bench interacted with.");
            // Add your bench interaction behavior here.
        }
    }
}
