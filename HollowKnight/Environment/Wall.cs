using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    // TODO: Use these inside the spritefactory
    public enum WallVariant
    {
        Wall1,
        Wall2
    }

    public class Wall : IObject
    {
        public string Label => $"Wall_{variant}";
        private ISprite sprite;
        private readonly int variant;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];
        private int hitWidth;
        private int hitHeight;
        private int hitOffsetY;

        public bool IsActive => true;
        public Rectangle Bounds => GetBounds()[0];

        public Wall(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            sprite = SpriteFactory.Instance.CreateWallSprite(variant, position);
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
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + hitOffsetY, hitWidth, hitHeight);
            return hitBoxes;
        }
    }
}