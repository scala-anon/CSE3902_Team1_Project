using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Bench : BaseEnvironmentObject
    {
        public override string Label => "Bench";

        public Bench(Vector2 pos)
        {
            position = pos;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateBenchSprite(position);
        }

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, 100, 32);
            return hitBoxes;
        }
    }
}
