using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Path : IObject
    {
        private ISprite sprite;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];
        private int hitWidth;
        private int hitHeight;
        private int hitOffsetY;
        private int variant;

        public bool IsActive => true;
        public string Label => $"Path_{variant}";
        public Rectangle Bounds => GetBounds()[0];

        public Path(int variant, Vector2 pos, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            position = pos;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            sprite = SpriteFactory.Instance.CreatePathSprite(variant, position);
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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + hitOffsetY, hitWidth, hitHeight);
            return hitBoxes;
        }
    }
}
