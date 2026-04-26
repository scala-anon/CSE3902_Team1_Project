using System.Collections.Generic;
using HollowKnight.Abilities;
using HollowKnight.Collision;
using HollowKnight.Environment;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Projectiles;

namespace HollowKnight.Shared
{
    public static class CollisionLayerMatrix
    {
        private static readonly Dictionary<CollisionLayer, CollisionLayer> _matrix = new Dictionary<CollisionLayer, CollisionLayer>
        {
            [CollisionLayer.Player]           = CollisionLayer.Enemy | CollisionLayer.Hazard | CollisionLayer.Terrain
                                              | CollisionLayer.Interactable | CollisionLayer.EnemyProjectile | CollisionLayer.Pickup
                                              | CollisionLayer.BreakableTerrain,
            [CollisionLayer.PlayerAttack]     = CollisionLayer.Enemy | CollisionLayer.Interactable
                                              | CollisionLayer.BreakableTerrain,
            [CollisionLayer.Enemy]            = CollisionLayer.Player | CollisionLayer.Terrain | CollisionLayer.Hazard
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.PlayerAttack
                                              | CollisionLayer.BreakableTerrain,
            [CollisionLayer.Hazard]           = CollisionLayer.Player | CollisionLayer.Enemy
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.EnemyProjectile,
            [CollisionLayer.Terrain]          = CollisionLayer.Player | CollisionLayer.Enemy
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.EnemyProjectile,
            [CollisionLayer.Interactable]     = CollisionLayer.Player | CollisionLayer.PlayerAttack,
            [CollisionLayer.PlayerProjectile] = CollisionLayer.Enemy | CollisionLayer.Terrain | CollisionLayer.Hazard
                                              | CollisionLayer.BreakableTerrain,
            [CollisionLayer.EnemyProjectile]  = CollisionLayer.Player | CollisionLayer.Terrain | CollisionLayer.Hazard
                                              | CollisionLayer.BreakableTerrain,
            [CollisionLayer.Pickup]           = CollisionLayer.Player,
            [CollisionLayer.None]             = CollisionLayer.None,
            [CollisionLayer.BreakableTerrain] = CollisionLayer.Player | CollisionLayer.Enemy
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.EnemyProjectile
                                              | CollisionLayer.PlayerAttack,
        };

        public static CollisionLayer GetLayer(ICollidable obj)
        {
            // Order matters: more-specific interfaces (IInteractable) come before
            // broader ones (IEnemy). An enemy that is also IInteractable intentionally
            // routes to the Interactable layer. Change priority here if that's wrong.
            switch (obj)
            {
                case TheKnight _:    return CollisionLayer.Player;
                case SwordHitbox _:  return CollisionLayer.PlayerAttack;
                case Spike _:        return CollisionLayer.Hazard;
                // IBreakable must come before IInteractable: BreakableWall and Door
                // implement both, but route to BreakableTerrain so the
                // PlayerAttack<->BreakableTerrain pairing fires via the platforms loop
                // rather than the sword-vs-interactable loop (prevents double-hit).
                case IBreakable _:   return CollisionLayer.BreakableTerrain;
                case IInteractable _: return CollisionLayer.Interactable;
                case IEnemy _:       return CollisionLayer.Enemy;
                case Spirit _:       return CollisionLayer.Pickup;
                case Projectile p:
                    return p.Faction == ProjectileFaction.Player
                        ? CollisionLayer.PlayerProjectile
                        : CollisionLayer.EnemyProjectile;
                default:             return CollisionLayer.Terrain;
            }
        }

        public static bool ShouldCollide(ICollidable a, ICollidable b)
        {
            CollisionLayer layerA = GetLayer(a);
            CollisionLayer layerB = GetLayer(b);
            if (layerA == CollisionLayer.None || layerB == CollisionLayer.None) return false;
            if (!_matrix.TryGetValue(layerA, out CollisionLayer maskA)) return false;
            return (maskA & layerB) != 0;
        }

        public static void ValidateSymmetry()
        {
            foreach (var kvpA in _matrix)
            {
                CollisionLayer layerA = kvpA.Key;
                CollisionLayer maskA = kvpA.Value;
                foreach (var kvpB in _matrix)
                {
                    CollisionLayer layerB = kvpB.Key;
                    CollisionLayer maskB = kvpB.Value;
                    bool aSaysYes = (maskA & layerB) != 0;
                    bool bSaysYes = (maskB & layerA) != 0;
                    if (aSaysYes != bSaysYes)
                    {
                        throw new System.InvalidOperationException(
                            $"CollisionLayerMatrix asymmetry: {layerA} <-> {layerB} " +
                            $"(A says {aSaysYes}, B says {bSaysYes})");
                    }
                }
            }
        }
    }
}
