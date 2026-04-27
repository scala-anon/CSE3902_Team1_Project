using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Layer : BaseEnvironmentObject
    {
        public override string Label => "Layer";
        public override Rectangle Bounds => Rectangle.Empty;

        public Layer(int variant, Vector2 position)
        {
            this.position = position;
            hitBoxes = new Rectangle[0];
            sprite = SpriteFactory.Instance.CreateLayerSprite(variant, position);
        }

        public override Rectangle[] GetBounds() => hitBoxes;
    }
}