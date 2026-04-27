using System;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class BreakableWall : BreakableEnvironmentObject
    {
        private readonly int variant;
        private bool _isPlayingBreakAnim;
        private Vector2 _animSpritePos;
        private float _breakAnimTimer;
        private Action _onDestroyed;

        public void SetDestroyedCallback(Action onDestroyed) => _onDestroyed = onDestroyed;

        public override string Label => $"BreakableWall_{variant}";
        public override InteractionType InteractionType => InteractionType.SwordHit;
        public override bool IsActive => !_broken;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_isPlayingBreakAnim)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animSpritePos.X -= EnvironmentConstants.BreakableBreakDriftSpeed * dt;
                sprite.SetPosition(_animSpritePos);

                _breakAnimTimer += dt;
                if (_breakAnimTimer >= EnvironmentConstants.BreakableBreakAnimDuration)
                {
                    _broken = true;
                    _isPlayingBreakAnim = false;
                    _onDestroyed?.Invoke();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
        {
            if (_isPlayingBreakAnim)
                sprite.Draw(spriteBatch, SpriteEffects.FlipHorizontally, layerDepth);
            else
                base.Draw(spriteBatch, spriteEffects, layerDepth);
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

            DebugLogger.LogObject($"BreakableWall hit: {Label} ({_hitCount}/3)");

            if (_hitCount >= 3)
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
