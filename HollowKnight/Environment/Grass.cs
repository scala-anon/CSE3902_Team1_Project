using HollowKnight.Factories;
using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Player;

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
            if(_chopped) return;

            _chopped = true;
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1ChoppedSprite(position)
                : SpriteFactory.Instance.CreatePlant2ChoppedSprite(position);
        }

        public Rectangle[] GetInteractionBounds() => GetBounds();

        public bool IsInteractable(TheKnight knight) => !_chopped;

        public void OnInteract(TheKnight knight)
        {
            Chop();
        }
    }
}
