using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;

namespace HollowKnight.Collision
{
    
    public static class DebugRenderer
    {
        // The 1x1 white pixel that gets stretched to fill any rectangle
        private static Texture2D _pixel;
        private static SpriteFont _font;
        public static bool hitboxEnabled { get; set; } = false;

        public static readonly Color ColorKnight      = CollisionConstants.DebugColorKnight;
        public static readonly Color ColorEnemy       = CollisionConstants.DebugColorEnemy;
        public static readonly Color ColorEnvironment = CollisionConstants.DebugColorEnvironment;
        public static readonly Color ColorTrigger     = CollisionConstants.DebugColorTrigger;
        public static readonly Color ColorMidpoint    = CollisionConstants.DebugColorMidpoint;
        public static readonly Color ColorSword       = CollisionConstants.DebugColorSword;

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
            Rectangle bounds = obj.GetBounds()[0];
            spriteBatch.DrawString(_font, label, new Vector2(bounds.Left, bounds.Top - CollisionConstants.DebugTextOffsetY), Color.White);
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            if (!hitboxEnabled || _font == null) return;
            spriteBatch.DrawString(_font, text, position, color);
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

        public static void DrawRadius(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, int segments = CollisionConstants.DebugCircleSegments)
        {
            if(!hitboxEnabled || _pixel == null) return;

            float step = MathHelper.TwoPi / segments;
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

        public static void DrawPath(SpriteBatch spriteBatch, ICollidable obj, List<Vector2> path, Color lineColor, Color pointColor)
        {
            if (!hitboxEnabled || _pixel == null || path == null || path.Count == 0) return;

            Vector2 currentPos = new Vector2(obj.GetBounds()[0].Center.X, obj.GetBounds()[0].Center.Y);
            foreach (Vector2 waypoint in path)
            {
                DrawLine(spriteBatch, currentPos, waypoint, lineColor);
                DrawPoint(spriteBatch, waypoint, pointColor, 4);
                currentPos = waypoint;
            }
        }


        private static void DrawHitbox(SpriteBatch spriteBatch, Rectangle[] rect, Color color)
        {
            foreach (Rectangle rectangle in rect)
            {
                spriteBatch.Draw(_pixel, rectangle, color * 0.25f);

                // Solid border
                spriteBatch.Draw(_pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, CollisionConstants.DebugBorderThickness), color);
                spriteBatch.Draw(_pixel, new Rectangle(rectangle.Left, rectangle.Bottom - CollisionConstants.DebugBorderThickness, rectangle.Width, CollisionConstants.DebugBorderThickness), color);
                spriteBatch.Draw(_pixel, new Rectangle(rectangle.Left, rectangle.Top, CollisionConstants.DebugBorderThickness, rectangle.Height), color);
                spriteBatch.Draw(_pixel, new Rectangle(rectangle.Right - CollisionConstants.DebugBorderThickness, rectangle.Top, CollisionConstants.DebugBorderThickness, rectangle.Height), color);
            }
        }

        public static void DrawPoint(SpriteBatch spriteBatch, Vector2 center, Color color, int size = CollisionConstants.DebugPointDefaultSize)
        {
            if(!hitboxEnabled || _pixel == null) return;
            int halfSize = size / 2;
            spriteBatch.Draw(_pixel, new Rectangle((int)(center.X - halfSize), (int)(center.Y - halfSize), size, size), color);
        }
    }
}
