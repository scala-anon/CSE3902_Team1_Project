using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Floor : BaseEnvironmentObject
    {
        private int id;
        private int width;
        private int height;
        public override string Label => $"Floor_{id}";

        public Floor(int id, Vector2 pos, int width, int height)
        {
            this.id = id;
            position = pos;
            this.width = width;
            this.height = height;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateFloorSprite(id, position);
        }

        public override Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, width, height);
            return hitBoxes;
        }
    }
}