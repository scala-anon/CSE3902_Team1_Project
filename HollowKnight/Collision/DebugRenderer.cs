using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
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
        public static readonly Color ColorMidpoint    = Color.Magenta;

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

        public static void DrawRadius(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, int segments = 32) //more segments -> smoother circle (expensive to draw)
        {
            if(!hitboxEnabled || _pixel == null) return;

            float step = MathHelper.TwoPi / segments; //angle between each segment in radians
            for(int i=0; i<segments; i++)
            {
                //find endpoints of a segment on circle, then draw a line between them
                //once loop is over, a circle is drawn
                Vector2 p1 = new Vector2( 
                    center.X + MathF.Cos(i * step) * radius, 
                    center.Y + MathF.Sin(i * step) * radius); 
                Vector2 p2 = new Vector2(
                    center.X + MathF.Cos((i+1) * step) * radius,
                    center.Y + MathF.Sin((i+1) * step) * radius);
                DrawLine(spriteBatch, p1, p2, color);
            }
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Color color)
        {
            Vector2 diff = p2 - p1;
            float length = diff.Length();
            float angle = MathF.Atan2(diff.Y, diff.X); //angle of the line in radians
            spriteBatch.Draw(_pixel, p1, null, color, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0); 
        }


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

        public static void DrawPoint(SpriteBatch spriteBatch, Vector2 center, Color color, int size = 6)
        {
            if(!hitboxEnabled || _pixel == null) return;
            spriteBatch.Draw(_pixel, new Rectangle((int)(center.X - size/2), (int)(center.Y - size/2), size, size), color);
        }
    }
}
