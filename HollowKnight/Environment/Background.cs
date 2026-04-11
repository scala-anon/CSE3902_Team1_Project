using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Background : BaseEnvironmentObject
    {
        public override string Label => "Background";
        public override Rectangle Bounds => Rectangle.Empty;

        public Background(int variant, Vector2 position)
        {
            this.position = position;
            hitBoxes = new Rectangle[0];
            sprite = SpriteFactory.Instance.CreateBackgroundSprite(variant, position);
        }

        public override Rectangle[] GetBounds() => hitBoxes;
    }
}
