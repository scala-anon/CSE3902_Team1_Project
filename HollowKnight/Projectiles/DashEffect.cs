using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Projectiles
{
    public class DashEffect : Projectile, ICollidable
    {
        private readonly ISprite _sprite;
        private readonly Direction _direction;

        public DashEffect(Vector2 position, Direction direction)
            : base(position, Vector2.Zero, ProjectileFaction.Player)
        {
            _direction = direction;
            Vector2 offset = direction == Direction.Left
                ? new Vector2(KnightConstants.DashEffectOffsetLeftX, KnightConstants.DashEffectOffsetY)
                : new Vector2(KnightConstants.DashEffectOffsetRightX, KnightConstants.DashEffectOffsetY);
            _sprite = SpriteFactory.Instance.CreateDashEffect(position + offset);
            Damage = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
            if (_sprite.IsFinished)
                Alive = false;
        }

        public override void Draw(SpriteBatch spriteBatch, Direction facing, float layerDepth = 0f)
        {
            if (!Alive) return;
            SpriteEffects effects = _direction == Direction.Left
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            _sprite.Draw(spriteBatch, effects, layerDepth);
        }

        public override void OnCollide(ICollidable target, CollisionSide side) { }
    }
}
