using HollowKnight.Factories;
using HollowKnight.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Background : BaseEnvironmentObject
    {
        public override string Label => "Background";
        public override Rectangle Bounds => Rectangle.Empty;

        private readonly bool _flipped;

        public Background(int variant, Vector2 position, bool flipped = false, Color? tint = null)
        {
            this.position = position;
            hitBoxes = new Rectangle[0];
            _flipped = flipped;
            sprite = SpriteFactory.Instance.CreateBackgroundSprite(variant, position);
            if (tint.HasValue && sprite is StaticSprite ss)
                ss.SetTint(tint.Value);
        }

        public override Rectangle[] GetBounds() => hitBoxes;

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, float layerDepth = 0f)
        {
            var effects = _flipped ? SpriteEffects.FlipHorizontally : spriteEffects;
            sprite.Draw(spriteBatch, effects, layerDepth);
        }
    }
}
