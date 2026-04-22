using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Abilities
{
    public class Spirit : IPickup
    {
        private ISprite sprite;
        private Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];

        public bool IsActive { get; set; } = true;
        public bool Collected { get; set; }
        public Rectangle Bounds => GetBounds()[0];

        public Spirit(Vector2 position)
        {
            Collected = false;
            this.position = position;
            sprite = SpriteFactory.Instance.CreateSpiritInitialSprite(position);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth = 0f)
        {
            sprite.Draw(spriteBatch, SpriteEffects.None, layerDepth);
        }

        public Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            return hitBoxes;
        }
    }
}
