using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;

namespace HollowKnight.Projectiles
{
    public class LowHealthEffect : Projectile, ICollidable
    {

        private readonly ISprite _sprite;
        private readonly Action _onComplete;

        public LowHealthEffect(Vector2 position, Action onComplete)
            : base(position, Vector2.Zero, ProjectileFaction.Player)
        {
            _sprite = SpriteFactory.Instance.CreateLowHealthEffect(position);
            Vector2 size = _sprite.GetSize();
            _sprite.SetPosition(new Vector2(position.X - size.X * KnightConstants.LowHealthEffectXOffsetFactor, position.Y - size.Y / 2f));
            _onComplete = onComplete;
            Damage = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
            if (_sprite.IsFinished)
            {
                _onComplete?.Invoke();
                Alive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Direction facing, float layerDepth = 0f)
        {
            if (!Alive) return;
            _sprite.Draw(spriteBatch, SpriteEffects.None, layerDepth);
        }

        public override void OnCollide(ICollidable target, CollisionSide side) { }
    }
}
