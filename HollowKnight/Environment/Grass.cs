using HollowKnight.Factories;
using Microsoft.Xna.Framework;

namespace HollowKnight.Environment
{
    public enum PlantState { Alive, Chopped }
    public enum PlantVariant { Plant1, Plant2 }

    public class Grass : SizedEnvironmentObject
    {
        private readonly int variant;
        public override string Label => $"Grass_{variant}";

        public Grass(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            hitBoxes = new Rectangle[1];
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1IdleSprite(position)
                : SpriteFactory.Instance.CreatePlant2IdleSprite(position);
        }

        public void Chop()
        {
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1ChoppedSprite(position)
                : SpriteFactory.Instance.CreatePlant2ChoppedSprite(position);
        }
    }
}
