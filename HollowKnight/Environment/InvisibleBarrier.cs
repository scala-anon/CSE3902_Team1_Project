using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class InvisibleBarrier : SizedEnvironmentObject
    {
        public override string Label => "InvisibleBarrier";

        public InvisibleBarrier(Vector2 position, float width, float height)
        {
            this.position  = position;
            this.hitWidth  = (int)width;
            this.hitHeight = (int)height;
            hitBoxes       = new Rectangle[1];
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f, float opacity = Shared.GameConstants.DefaultSpriteOpacity) { }
    }
}
