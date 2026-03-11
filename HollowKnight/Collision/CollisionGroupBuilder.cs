using System.Collections.Generic;
using HollowKnight.Interfaces;

namespace HollowKnight.Collision
{
    public static class CollisionGroupBuilder
    {
        public static ICollidable[] GetCollidables<T>(T[] items) where T : class
        {
            List<ICollidable> collidables = new List<ICollidable>();

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is ICollidable collidable)
                {
                    collidables.Add(collidable);
                }
            }

            return collidables.ToArray();
        }
    }
}