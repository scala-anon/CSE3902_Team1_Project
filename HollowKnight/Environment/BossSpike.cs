using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Environment
{

    public class BossSpike : IObject, IHazard
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[2];

        public bool IsActive => true;
        public string Label => $"BossSpike";
        public Rectangle Bounds { get; }
        public int Damage => GameConstants.SpikeDamage;
        public Vector2 Knockback => new Vector2(KnightConstants.KnightKnockbackSpeed, KnightConstants.KnightKnockbackUpwards);

        public BossSpike(Vector2 pos)
        {
            position = pos;
            sprite = SpriteFactory.Instance.CreateBossSpikeIdle(position);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            sprite.Draw(spriteBatch, spriteEffects);
        }

        // TODO: REDO THR HITBOXES
        public Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, CollisionConstants.SpikeCeilingPrimaryW, CollisionConstants.SpikeCeilingPrimaryH);
            hitBoxes[1] = new Rectangle((int)position.X + CollisionConstants.SpikeCeilingSecondaryX, (int)position.Y + CollisionConstants.SpikeCeilingSecondaryY, CollisionConstants.SpikeCeilingSecondaryW, CollisionConstants.SpikeCeilingSecondaryH);
            return hitBoxes;
        }
    }
}
