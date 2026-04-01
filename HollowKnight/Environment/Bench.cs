using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    // TODO: Need IInteractable 
    public class Bench : IObject
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];

        public bool IsActive => true;
        public Rectangle Bounds => GetBounds()[0];

        public Bench(Vector2 pos)
        {
            position = pos;
            sprite = SpriteFactory.Instance.CreateBenchSprite(position);
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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 100, 32);
            return hitBoxes;
        }
    }
}
