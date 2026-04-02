using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class TutorialPlatform : IObject
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];
        private int id;

        public bool IsActive => true;
        public Rectangle Bounds => GetBounds()[0];

        public TutorialPlatform(int id, Vector2 pos)
        {
            this.id = id;
            position = pos;
            sprite = SpriteFactory.Instance.CreateTutorialPlatformSprite(id, position);
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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 264, 79);
            return hitBoxes;
        }
    }
}
