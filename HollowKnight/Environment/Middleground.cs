using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Middleground : BaseEnvironmentObject
    {
        public override string Label => "Middleground";
        public override Rectangle Bounds => Rectangle.Empty;

        public Middleground(int variant, Vector2 position)
        {
            this.position = position;
            hitBoxes = new Rectangle[0];
            // TODO: add cases for variants to make it the correct kind of sprite
            sprite = SpriteFactory.Instance.CreateBossMiddlegroundSprite(variant, position);
        }

        public override Rectangle[] GetBounds() => hitBoxes;
    }
}
