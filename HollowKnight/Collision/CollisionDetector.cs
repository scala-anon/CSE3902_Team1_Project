using System;
using Microsoft.Xna.Framework;

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
        public static CollisionSide Detect(ICollidable objectA, ICollidable objectB)
        {
            Rectangle boundsA = objectA.GetBounds();
            Rectangle boundsB = objectB.GetBounds();

            if (!boundsA.Intersects(boundsB))
            {
                return CollisionSide.None;
            }

            // Measure overlaps
            int overlapFromLeft   = boundsB.Right  - boundsA.Left;
            int overlapFromRight  = boundsA.Right  - boundsB.Left;
            int overlapFromTop    = boundsB.Bottom - boundsA.Top;
            int overlapFromBottom = boundsA.Bottom - boundsB.Top;

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
}
