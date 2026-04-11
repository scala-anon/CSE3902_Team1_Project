using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Flag : BaseEnvironmentObject
    {
        private int id;
        private int width;
        private int height;
        public override string Label => $"Flag_{id}";

        public Flag(int id, Vector2 pos, int width, int height)
        {
            this.id = id;
            position = pos;
            this.width = width;
            this.height = height;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateFlagSprite(id, position);
        }

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle(0,0,0,0);
            return hitBoxes;
        }
    }
}