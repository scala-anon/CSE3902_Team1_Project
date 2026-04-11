using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public class Wall : SizedEnvironmentObject
    {
        private readonly int variant;
        public override string Label => $"Wall_{variant}";

        public Wall(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateWallSprite(variant, position);
        }
    }
}
