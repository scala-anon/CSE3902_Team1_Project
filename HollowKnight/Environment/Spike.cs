using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Environment
{
    public enum SpikeVariant
    {
        Floor1,
        Floor2,
        Ceiling
    }

    public class Spike : IObject, IHazard
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[2];
        private SpikeVariant variant;

        public bool IsActive => true;
        public string Label => $"Spike_{variant}";
        public Rectangle Bounds { get; }
        public int Damage => GameConstants.SpikeDamage;
        public Vector2 Knockback => new Vector2(KnightConstants.KnightKnockbackSpeed, KnightConstants.KnightKnockbackUpwards);

        public Spike(SpikeVariant variant, Vector2 pos)
        {
            this.variant = variant;
            position = pos;
            sprite = SpriteFactory.Instance.CreateSpikeSprite(variant, position);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
        {
            sprite.Draw(spriteBatch, spriteEffects, layerDepth);
        }

        // TODO: Tune width/height to match the actual scaled sprite size
        public Rectangle[] GetBounds()
        {
            switch (variant)
            {
                case SpikeVariant.Floor1:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + CollisionConstants.SpikeFloor1PrimaryY, CollisionConstants.SpikeFloor1PrimaryW, CollisionConstants.SpikeFloor1PrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + CollisionConstants.SpikeFloor1SecondaryX, (int)position.Y, CollisionConstants.SpikeFloor1SecondaryW, CollisionConstants.SpikeFloor1SecondaryH);
                    break;
                case SpikeVariant.Floor2:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + CollisionConstants.SpikeFloor2PrimaryY, CollisionConstants.SpikeFloor2PrimaryW, CollisionConstants.SpikeFloor2PrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + CollisionConstants.SpikeFloor2SecondaryX, (int)position.Y, CollisionConstants.SpikeFloor2SecondaryW, CollisionConstants.SpikeFloor2SecondaryH);
                    break;
                case SpikeVariant.Ceiling:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, CollisionConstants.SpikeCeilingPrimaryW, CollisionConstants.SpikeCeilingPrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + CollisionConstants.SpikeCeilingSecondaryX, (int)position.Y + CollisionConstants.SpikeCeilingSecondaryY, CollisionConstants.SpikeCeilingSecondaryW, CollisionConstants.SpikeCeilingSecondaryH);
                    break;
            }
            return hitBoxes;
        }
    }
}
