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
        private int width;
        private int height;
        
        public bool IsActive => true;
        public string Label => $"TutorialPlatform_{id}";
        public Rectangle Bounds => GetBounds()[0];

        public TutorialPlatform(int id, Vector2 pos, int width, int height)
        {
            this.id = id;
            position = pos;
            this.width = width;
            this.height = height;
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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, width, height);
            return hitBoxes;
        }
    }
}
