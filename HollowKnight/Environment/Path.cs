using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Path : SizedEnvironmentObject
    {
        private int variant;
        public override string Label => $"Path_{variant}";

        public Path(int variant, Vector2 pos, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            position = pos;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreatePathSprite(variant, position);
        }
    }
}
