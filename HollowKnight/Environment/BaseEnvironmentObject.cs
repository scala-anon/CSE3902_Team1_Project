using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public abstract class BaseEnvironmentObject : IObject
    {
        protected ISprite sprite;
        public Vector2 position;
        protected Rectangle[] hitBoxes;

        public abstract string Label { get; }
        public virtual bool IsActive => true;
        public virtual Rectangle Bounds => hitBoxes.Length > 0 ? GetBounds()[0] : Rectangle.Empty;

        public virtual void Update(GameTime gameTime) => sprite.Update(gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
            => sprite.Draw(spriteBatch, spriteEffects, layerDepth);

        public abstract Rectangle[] GetBounds();
    }

    public abstract class SizedEnvironmentObject : BaseEnvironmentObject
    {
        protected int hitWidth;
        protected int hitHeight;
        protected int hitOffsetY;

        public override Rectangle Bounds => GetBounds()[0];

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle(
                (int)position.X, (int)position.Y + hitOffsetY, hitWidth, hitHeight);
            return hitBoxes;
        }
    }
}
