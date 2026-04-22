using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class BreakableWall : BreakableEnvironmentObject
    {
        private readonly int variant;

        public override string Label => $"BreakableWall_{variant}";

        public override InteractionType InteractionType => InteractionType.SwordHit;

        public BreakableWall(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateWallSprite(variant, position);
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
            sprite = SpriteFactory.Instance.CreateBrokenWallSprite(variant, position);
            DebugLogger.LogObject($"BreakableWall broken: {Label}");
        }

        public override void OnInteract(TheKnight knight)
        {
            DebugLogger.LogObject($"BreakableWall hit: {Label} ({_hitCount + 1}/{CollisionConstants.BreakableHitsToBreak})");
            base.OnInteract(knight);
        }
    }
}
