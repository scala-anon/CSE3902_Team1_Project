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
        public Vector2 Knockback => new Vector2(GameConstants.KnightKnockbackSpeed, GameConstants.KnightKnockbackUpwards);

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

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            sprite.Draw(spriteBatch, spriteEffects);
        }

        // TODO: Tune width/height to match the actual scaled sprite size
        public Rectangle[] GetBounds()
        {
            switch (variant)
            {
                case SpikeVariant.Floor1:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + GameConstants.SpikeFloor1PrimaryY, GameConstants.SpikeFloor1PrimaryW, GameConstants.SpikeFloor1PrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + GameConstants.SpikeFloor1SecondaryX, (int)position.Y, GameConstants.SpikeFloor1SecondaryW, GameConstants.SpikeFloor1SecondaryH);
                    break;
                case SpikeVariant.Floor2:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + GameConstants.SpikeFloor2PrimaryY, GameConstants.SpikeFloor2PrimaryW, GameConstants.SpikeFloor2PrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + GameConstants.SpikeFloor2SecondaryX, (int)position.Y, GameConstants.SpikeFloor2SecondaryW, GameConstants.SpikeFloor2SecondaryH);
                    break;
                case SpikeVariant.Ceiling:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, GameConstants.SpikeCeilingPrimaryW, GameConstants.SpikeCeilingPrimaryH);
                    hitBoxes[1] = new Rectangle((int)position.X + GameConstants.SpikeCeilingSecondaryX, (int)position.Y + GameConstants.SpikeCeilingSecondaryY, GameConstants.SpikeCeilingSecondaryW, GameConstants.SpikeCeilingSecondaryH);
                    break;
            }
            return hitBoxes;
        }
    }
}
