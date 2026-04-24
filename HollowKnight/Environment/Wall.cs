using HollowKnight.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Wall : SizedEnvironmentObject
    {
        private readonly int variant;
        private readonly bool _flipped;
        public override string Label => $"Wall_{variant}";

        public Wall(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0, bool flipped = false, int hitOffsetX = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetX = hitOffsetX;
            this.hitOffsetY = hitOffsetY;
            this._flipped = flipped;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateWallSprite(variant, position);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
        {
            var effects = _flipped ? SpriteEffects.FlipHorizontally : spriteEffects;
            sprite.Draw(spriteBatch, effects, layerDepth);
        }
    }
}
