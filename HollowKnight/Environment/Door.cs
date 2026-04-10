using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public enum DoorState { Full, Half, Broken }

    public class Door : SizedEnvironmentObject
    {
        private readonly int variant;
        public override string Label => "Door";

        public Door(Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateDoorSprite(position);
        }

        public void Break()
        {
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1ChoppedSprite(position)
                : SpriteFactory.Instance.CreatePlant2ChoppedSprite(position);
        }
    }
}
