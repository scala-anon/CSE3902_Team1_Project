using System;
using System.Collections.Generic;
using HollowKnight.Interfaces;

namespace HollowKnight.Collision
{

    public class CollisionHandler
    {
        // Key: (type of objectA, type of objectB, collision side) — Value: response action
        private readonly Dictionary<(Type, Type, CollisionSide), Action<ICollidable, ICollidable>> _handlers
            = new Dictionary<(Type, Type, CollisionSide), Action<ICollidable, ICollidable>>();

        /// <summary>
        /// Registers a response for a specific (TypeA, TypeB, side) combination.
        /// Cast objectA and objectB to their real types inside the lambda to call their methods.
        /// </summary>
        public void Register<TObjectA, TObjectB>(CollisionSide side, Action<ICollidable, ICollidable> response)
        {
            var key = (typeof(TObjectA), typeof(TObjectB), side);
            _handlers[key] = response;
        }

        public void HandleCollision(ICollidable objectA, ICollidable objectB, CollisionSide side)
        {
            if (side == CollisionSide.None)
            {
                return;
            }

            var key = (objectA.GetType(), objectB.GetType(), side);

            if (_handlers.TryGetValue(key, out Action<ICollidable, ICollidable> response))
            {
                response(objectA, objectB);
            }
        }
    }
}
