using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public enum DoorState { Full, Half, Broken }

    public class Door : BreakableEnvironmentObject
    {
        public override string Label => "Door";

        public override InteractionType InteractionType => InteractionType.SwordHit;

        public Door(Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateDoorSprite(position);
        }

        public override Rectangle[] GetInteractionBounds()
        {
            int expand = CollisionConstants.BreakableWallInteractionExpand;
            Rectangle b = GetBounds()[0];
            return new[]
            {
                new Rectangle(b.X - expand, b.Y - expand, b.Width + expand * 2, b.Height + expand * 2)
            };
        }

        public override bool IsInteractable(TheKnight knight) => !_broken;

        protected override void ApplyBrokenSprite()
        {
            // TODO: swap to broken door sprite once art asset exists; Door_1 used as placeholder
            sprite = SpriteFactory.Instance.CreateDoorHitSprite(position);
            DebugLogger.LogObject($"Door broken: {Label}");
        }

        public override void OnInteract(TheKnight knight)
        {
            DebugLogger.LogObject($"Door hit: {Label} ({_hitCount + 1}/{CollisionConstants.BreakableHitsToBreak})");
            base.OnInteract(knight);
        }
    }
}
