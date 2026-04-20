using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public enum DoorState { Full, Half, Broken }

    public class Door : SizedEnvironmentObject, IInteractable, IBreakable
    {
        private int _hitCount = 0;
        private bool _broken = false;
        private long _lastHitTime = 0;
        private const int HitsToBreak = 3;
        private const long HitCooldownMs = 500;

        public override string Label => "Door";
        public override bool IsActive => !_broken;

        public int Health => HitsToBreak - _hitCount;
        public InteractionType InteractionType => InteractionType.SwordHit;

        public Door(Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateDoorSprite(position);
        }

        public void Break()
        {
            if (_broken) return;
            _broken = true;
            // TODO: swap to broken door sprite once art asset exists; Door_1 used as placeholder
            sprite = SpriteFactory.Instance.CreateDoorHitSprite(position);
            DebugLogger.LogObject($"Door broken: {Label}");
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
            long now = System.Environment.TickCount64;
            if (now - _lastHitTime < HitCooldownMs) return;
            _lastHitTime = now;
            _hitCount++;
            DebugLogger.LogObject($"Door hit: {Label} ({_hitCount}/{HitsToBreak})");
            if (_hitCount >= HitsToBreak)
                Break();
        }
    }
}
