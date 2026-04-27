using System;
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
        private bool _isPlayingBreakAnim;
        private Vector2 _animSpritePos;
        private float _breakAnimTimer;
        private Action _onDestroyed;
        private const float BreakDriftSpeed = 400f;
        private const float BreakAnimDuration = 0.3f;

        public void SetDestroyedCallback(Action onDestroyed) => _onDestroyed = onDestroyed;

        public override string Label => "Door";
        public override InteractionType InteractionType => InteractionType.SwordHit;
        public override bool IsActive => !_broken;

        public Door(Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateDoorSprite(position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_isPlayingBreakAnim)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animSpritePos.X += BreakDriftSpeed * dt;
                sprite.SetPosition(_animSpritePos);

                _breakAnimTimer += dt;
                if (_breakAnimTimer >= BreakAnimDuration)
                {
                    _broken = true;
                    _isPlayingBreakAnim = false;
                    _onDestroyed?.Invoke();
                }
            }
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

        public override bool IsInteractable(TheKnight knight) => !_broken && !_isPlayingBreakAnim;

        public override void OnInteract(TheKnight knight)
        {
            if (_broken || _cooldownActive || _isPlayingBreakAnim) return;
            _hitCount++;
            _cooldownActive = true;
            _hitCooldownTimer = 0;

            DebugLogger.LogObject($"Door hit: {Label} ({_hitCount}/2)");

            if (_hitCount == 1)
            {
                sprite = SpriteFactory.Instance.CreateDoorHitSprite(position);
            }
            else if (_hitCount >= 2)
            {
                _isPlayingBreakAnim = true;
                _animSpritePos = position;
                _breakAnimTimer = 0;
                sprite = SpriteFactory.Instance.CreateDoorBreakAnimSprite(position);
            }
        }

        protected override void ApplyBrokenSprite() { }
    }
}
