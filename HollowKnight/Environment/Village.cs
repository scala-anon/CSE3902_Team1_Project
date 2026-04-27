using HollowKnight.Factories;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    public class Village : SizedEnvironmentObject
    {
        private int id;
        private int width;
        private int height;
        public override string Label => $"Village_{id}";

        public Village(int id, Vector2 pos, int width, int height)
        {
            this.id = id;
            position = pos;
            this.width = width;
            this.height = height;
            hitBoxes = new Rectangle[1];
            sprite = SpriteFactory.Instance.CreateVillageSprite(id, position);
        }
        
    }
}