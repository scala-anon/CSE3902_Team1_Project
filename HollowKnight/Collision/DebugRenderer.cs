using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Collision
{
    
    public static class DebugRenderer
    {
        // The 1x1 white pixel that gets stretched to fill any rectangle
        private static Texture2D _pixel;
        private static SpriteFont _font;
        public static bool hitboxEnabled { get; set; } = true;


        public static readonly Color ColorKnight      = Color.DodgerBlue;
        public static readonly Color ColorEnemy       = Color.OrangeRed;
        public static readonly Color ColorEnvironment = Color.LimeGreen;
        public static readonly Color ColorTrigger     = Color.Yellow;

        /// <summary>
        /// Creates the internal 1x1 pixel texture.
        /// Must be called once before any drawing, typically in Game.LoadContent().
        /// </summary>
        /// <param name="graphicsDevice">The game's GraphicsDevice.</param>
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static void LoadFont(SpriteFont font)
        {
            _font = font;
        }   

        public static void DrawStateLabel(SpriteBatch spriteBatch, ICollidable obj, string label, Color color)
        {
            if(!hitboxEnabled || _font ==null) return;
            Rectangle bounds = obj.GetBounds();
            spriteBatch.DrawString(_font, label, new Vector2(bounds.Left, bounds.Top -16 ), Color.White); //16 for offset above hitbox
        }


        // Draws the bounding box of a single ICollidable object.
        public static void DrawBounds(SpriteBatch spriteBatch, ICollidable obj, Color color)
        {
            if (!hitboxEnabled || _pixel == null)
            {
                return;
            }

            DrawHitbox(spriteBatch, obj.GetBounds(), color);
        }


        // Draws bounding boxes for every object in a collection, all in the same color.
        public static void DrawAll(SpriteBatch spriteBatch, IEnumerable<ICollidable> objects, Color color)
        {
            if (!hitboxEnabled || _pixel == null)
            {
                return;
            }

            foreach (ICollidable obj in objects)
            {
                DrawHitbox(spriteBatch, obj.GetBounds(), color);
            }
        }

        // TODO: Add a DrawBounds(SpriteBatch, Rectangle, Color) overload so callers can pass
        // any raw rectangle directly — e.g. sprite visual bounds, attack zones, or damage hitboxes
        // that are separate from the entity's GetBounds() collision box.


        private static void DrawHitbox(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            const int BorderThickness = 2;

            spriteBatch.Draw(_pixel, rect, color * 0.25f);

            // Solid border
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, BorderThickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom - BorderThickness,rect.Width, BorderThickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, BorderThickness, rect.Height), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Right - BorderThickness, rect.Top, BorderThickness,rect.Height),color);
        }
    }
}
