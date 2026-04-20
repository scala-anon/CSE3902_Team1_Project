using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class BreakableWall : SizedEnvironmentObject, IInteractable, IBreakable
    {
        private readonly int variant;
        private int _hitCount = 0;
        private bool _broken = false;
        private const int HitsToBreak = 3;

        public override string Label => $"BreakableWall_{variant}";
        public override bool IsActive => !_broken;

        public int Health => HitsToBreak - _hitCount;
        public InteractionType InteractionType => InteractionType.SwordHit;

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

        public void Break()
        {
            if (_broken) return;
            _broken = true;
            sprite = SpriteFactory.Instance.CreateBrokenWallSprite(variant, position);
            DebugLogger.LogObject($"BreakableWall broken: {Label}");
        }

        public Rectangle[] GetInteractionBounds()
        {
            int expand = CollisionConstants.BreakableWallInteractionExpand;
            Rectangle b = GetBounds()[0];
            return new[]
            {
                new Rectangle(b.X - expand, b.Y - expand, b.Width + expand * 2, b.Height + expand * 2)
            };
        }

        public bool IsInteractable(TheKnight knight) => !_broken;

        public void OnInteract(TheKnight knight)
        {
            if (_broken) return;
            _hitCount++;
            DebugLogger.LogObject($"BreakableWall hit: {Label} ({_hitCount}/{HitsToBreak})");
            if (_hitCount >= HitsToBreak)
                Break();
        }
    }
}
