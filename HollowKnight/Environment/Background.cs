using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Background : IObject
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[0];

        public string Label => "Background";
        public bool IsActive => true;
        public Rectangle Bounds => Rectangle.Empty;

        public Background(int variant, Vector2 position)
        {
            this.position = position;
            sprite = SpriteFactory.Instance.CreateBackgroundSprite(variant, position);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            sprite.Draw(spriteBatch, spriteEffects);
        }

        public Rectangle[] GetBounds()
        {
            return hitBoxes;
        }
    }
}