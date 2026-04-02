using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Environment
{
    // TODO: Need IInteractable 
    public enum PlantState { Alive, Chopped }
    
    // TODO: Use these inside the spritefactory
    public enum PlantVariant
    {
        Plant1,
        Plant2
    }

    public class Grass : IObject
    {
        public string Label => $"Grass_{variant}";
        private ISprite sprite;
        private readonly int variant;
        public Vector2 position;
        private Rectangle[] hitBoxes = new Rectangle[1];
        private int hitWidth;
        private int hitHeight;
        private int hitOffsetY;
        private PlantState currentState = PlantState.Alive;

        public bool IsActive => true;
        public Rectangle Bounds => GetBounds()[0];

        public Grass(int variant, Vector2 position, int hitWidth, int hitHeight, int hitOffsetY = 0)
        {
            this.variant = variant;
            this.position = position;
            this.hitWidth = hitWidth;
            this.hitHeight = hitHeight;
            this.hitOffsetY = hitOffsetY;
            sprite = variant == 1
                ? SpriteFactory.Instance.CreatePlant1IdleSprite(position)
                : SpriteFactory.Instance.CreatePlant2IdleSprite(position);
        }

        public void Chop()
        {
            currentState = PlantState.Chopped;
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

