using HollowKnight.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Path : SizedEnvironmentObject
    {
        private int variant;
        private readonly bool _flipped;
        public override string Label => $"Path_{variant}";

        public Path(int variant, Vector2 pos, int hitWidth, int hitHeight, int hitOffsetY = 0, bool flipped = false)
        {
            this.variant = variant;
            position = pos;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            _flipped = flipped;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreatePathSprite(variant, position);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
        {
            var effects = _flipped ? SpriteEffects.FlipHorizontally : spriteEffects;
            sprite.Draw(spriteBatch, effects, layerDepth);
        }
    }
}
