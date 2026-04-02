using System;
using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;

namespace HollowKnight.Collision
{
   
    public static class CollisionDetector
    {
        /// <summary>
        /// Checks whether objectA and objectB overlap, and if so, returns which side
        /// of objectA was hit by objectB.
        ///
        /// Returns CollisionSide.None if the two rectangles do not intersect.
        /// </summary>
        /// 
        /// /*
        public static CollisionSide Detect(ICollidable objectA, ICollidable objectB)
        {
            Rectangle[] boundsA = objectA.GetBounds();
            Rectangle[] boundsB = objectB.GetBounds();
            
            foreach(Rectangle rectangleA in boundsA!)
            {
                foreach(Rectangle rectangleB in boundsB)
                {
                    if (!rectangleA.Intersects(rectangleB))
                    {
                        continue;
                    }
            //return Detect(objectA.GetBounds(), objectB.GetBounds());
        //}
        
       // public static CollisionSide Detect(Rectangle boundsA, Rectangle boundsB)
        //{
            //if (!boundsA.Intersects(boundsB))
            //{
            //    return CollisionSide.None;
            //}

                     // Measure overlaps
                    int overlapFromLeft   = rectangleB.Right  - rectangleA.Left;
                    int overlapFromRight  = rectangleA.Right  - rectangleB.Left;
                    int overlapFromTop    = rectangleB.Bottom - rectangleA.Top;
                    int overlapFromBottom = rectangleA.Bottom - rectangleB.Top;

                    // Find the smallest overlap, which indicates the side of collision
                    int minOverlap = Math.Min(
                        Math.Min(overlapFromLeft, overlapFromRight),
                        Math.Min(overlapFromTop,  overlapFromBottom)
                    );

                    if (minOverlap == overlapFromLeft)   return CollisionSide.Left;
                    if (minOverlap == overlapFromRight)  return CollisionSide.Right;
                    if (minOverlap == overlapFromTop)    return CollisionSide.Top;
                    return CollisionSide.Bottom;
                }
            }
            return CollisionSide.None;
            
        }
    }
}
