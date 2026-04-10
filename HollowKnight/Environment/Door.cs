using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    // TODO: Need IInteractable 
    public enum DoorState { Full, Half, Broken}
    
    // TODO: Use these inside the spritefactory
    public class Door : IObject
    {
        public string Label => "Door";
        private ISprite sprite;
        private readonly int variant;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];
        private int hitWidth;
        private int hitHeight;
        private int hitOffsetY;
        private DoorState currentState = DoorState.Full;

        public bool IsActive => true;
        public int Health => 1;
        public Rectangle Bounds => GetBounds()[0];

        public Door(Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            sprite = SpriteFactory.Instance.CreateDoorSprite(position);
        }


        public void Break()
        {
            currentState = DoorState.Full;
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1ChoppedSprite(position)
                : SpriteFactory.Instance.CreatePlant2ChoppedSprite(position);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            sprite.Draw(spriteBatch, spriteEffects);
        }

        public Rectangle[] GetBounds()
        {
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y + hitOffsetY, hitWidth, hitHeight);
            return hitBoxes;
        }
    }
}