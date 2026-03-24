using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public enum SpikeVariant
    {
        Floor1,
        Floor2,
        Ceiling
    }

    public class Spike : IObject
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[2];
        private SpikeVariant variant;

        public bool IsActive => true;
        public Rectangle Bounds { get; }

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
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + 50, 165, 45);
                    hitBoxes[1] = new Rectangle((int)position.X + 60, (int)position.Y, 75, 50);
                    break;
                case SpikeVariant.Floor2:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + 40, 140, 60);
                    hitBoxes[1] = new Rectangle((int)position.X + 40, (int)position.Y, 100, 40);
                    break;
                case SpikeVariant.Ceiling:
                    hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 230, 70);
                    hitBoxes[1] = new Rectangle((int)position.X + 50, (int)position.Y + 70, 115, 60);
                    break;
            }
            return hitBoxes;
        }
    }
}
