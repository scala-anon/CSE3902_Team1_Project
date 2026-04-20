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
                                              | CollisionLayer.Interactable | CollisionLayer.EnemyProjectile | CollisionLayer.Pickup,
            [CollisionLayer.PlayerAttack]     = CollisionLayer.Enemy | CollisionLayer.Interactable,
            [CollisionLayer.Enemy]            = CollisionLayer.Player | CollisionLayer.Terrain | CollisionLayer.Hazard
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.PlayerAttack,
            [CollisionLayer.Hazard]           = CollisionLayer.Player | CollisionLayer.Enemy,
            [CollisionLayer.Terrain]          = CollisionLayer.Player | CollisionLayer.Enemy
                                              | CollisionLayer.PlayerProjectile | CollisionLayer.EnemyProjectile,
            [CollisionLayer.Interactable]     = CollisionLayer.Player | CollisionLayer.PlayerAttack,
            [CollisionLayer.PlayerProjectile] = CollisionLayer.Enemy | CollisionLayer.Terrain,
            [CollisionLayer.EnemyProjectile]  = CollisionLayer.Player | CollisionLayer.Terrain,
            [CollisionLayer.Pickup]           = CollisionLayer.Player,
            [CollisionLayer.None]             = CollisionLayer.None,
        };

        public static CollisionLayer GetLayer(ICollidable obj)
        {
            switch (obj)
            {
                case TheKnight _:    return CollisionLayer.Player;
                case SwordHitbox _:  return CollisionLayer.PlayerAttack;
                case Spike _:        return CollisionLayer.Hazard;
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
            return (_matrix[layerA] & layerB) != 0
                && (_matrix[layerB] & layerA) != 0;
        }
    }
}
