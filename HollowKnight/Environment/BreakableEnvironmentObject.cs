using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public abstract class BreakableEnvironmentObject
        : SizedEnvironmentObject, IInteractable, IBreakable
    {
        protected int _hitCount;
        protected bool _broken;
        private double _hitCooldownTimer;
        private bool _cooldownActive;

        public override bool IsActive => !_broken;
        public int Health => CollisionConstants.BreakableHitsToBreak - _hitCount;

        public abstract InteractionType InteractionType { get; }
        public abstract Rectangle[] GetInteractionBounds();
        public abstract bool IsInteractable(TheKnight knight);

        protected abstract void ApplyBrokenSprite();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_cooldownActive)
            {
                _hitCooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_hitCooldownTimer >= CollisionConstants.BreakableHitCooldownSeconds)
                {
                    _cooldownActive = false;
                    _hitCooldownTimer = 0;
                }
            }
        }

        public virtual void OnInteract(TheKnight knight)
        {
            if (_broken || _cooldownActive) return;
            _hitCount++;
            _cooldownActive = true;
            _hitCooldownTimer = 0;
            if (_hitCount >= CollisionConstants.BreakableHitsToBreak)
                Break();
        }

        public virtual void Break()
        {
            if (_broken) return;
            _broken = true;
            ApplyBrokenSprite();
        }
    }
}
