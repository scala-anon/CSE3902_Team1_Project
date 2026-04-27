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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, CollisionConstants.BenchHitboxWidth, CollisionConstants.BenchHitboxHeight);
            return hitBoxes;
        }

        public Rectangle[] GetInteractionBounds()
        {
            return new[]
            {
                new Rectangle(
                    (int)position.X + CollisionConstants.BenchInteractionMarginX,
                    (int)position.Y + CollisionConstants.BenchInteractionMarginY,
                    CollisionConstants.BenchInteractionWidth,
                    CollisionConstants.BenchInteractionHeight)
            };
        }

        public bool IsInteractable(TheKnight knight)
        {
            return GetInteractionBounds()[0].Intersects(knight.GetBounds()[0]);
        }

        public void OnInteract(TheKnight knight)
        {
            knight.FullHeal();
            knight.SetBenchSpawnPoint(position);
            DebugLogger.LogObject($"Bench used: health restored to {knight.GetMaxHealth()}, spawn point set to {position}.");
        }
    }
}
