using HollowKnight.Factories;
using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;

namespace HollowKnight.Environment
{
    public enum PlantState { Alive, Chopped }
    public enum PlantVariant { Plant1, Plant2 }

    public class Grass : SizedEnvironmentObject, IInteractable
    {
        private readonly int variant;
        private bool _chopped;

        public override string Label => $"Grass_{variant}";
        public InteractionType InteractionType => InteractionType.SwordHit;
        public override bool IsActive => !_chopped;

        public Grass(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1IdleSprite(position)
                : SpriteFactory.Instance.CreatePlant2IdleSprite(position);
        }

        public void Chop()
        {
            if (_chopped) return;

            DebugLogger.LogObject($"Grass chopped: {Label}");
            _chopped = true;
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1ChoppedSprite(position)
                : SpriteFactory.Instance.CreatePlant2ChoppedSprite(position);
        }

        /// <summary>
        /// Collision bounds (for sword/rendering) — the actual grass sprite area
        /// </summary>
        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle(
                (int)position.X,
                (int)position.Y + hitOffsetY,
                hitWidth,
                hitHeight
            );
            return hitBoxes;
        }

        /// <summary>
        /// Interactive bounds (for sword strike triggers) — slightly expanded for easier interaction
        /// </summary>
        public Rectangle[] GetInteractionBounds()
        {
            // Expand the hitbox slightly to make sword interaction more forgiving
            int expandX = 10;
            int expandY = 5;
            return new[]
            {
                new Rectangle(
                    (int)position.X - expandX,
                    (int)position.Y + hitOffsetY - expandY,
                    hitWidth + (expandX * 2),
                    hitHeight + (expandY * 2)
                )
            };
        }

        public bool IsInteractable(TheKnight knight) => !_chopped;

        public void OnInteract(TheKnight knight)
        {
            Chop();
        }
    }
}
