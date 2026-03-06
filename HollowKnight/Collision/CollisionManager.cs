using HollowKnight.Interfaces;
using HollowKnight.Projectiles;

namespace HollowKnight.Collision
{
    public static class CollisionManager
    {
        // Returns true if projectile should be destroyed
        public static bool ProjectileHitsCollidable(Projectile p, ICollidable target)
        {
            if (p == null || target == null)
                return false;

            if (!p.IsActive || !target.IsActive)
                return false;

            return p.Bounds.Intersects(target.Bounds);
        }

        public static void ResolveProjectileCollisions(
            ProjectileManager pm,
            ICollidable player,
            ICollidable[] enemies,
            ICollidable[] blocks,
            System.Action onPlayerHit,
            System.Action<int> onEnemyHit // pass enemy index
        )
        {
            foreach (var p in pm.All)
            {
                if (!p.Alive) continue;

                // 1) Blocks first: projectile dies on impact
                for (int i = 0; i < blocks.Length && p.Alive; i++)
                {
                    if (ProjectileHitsCollidable(p, blocks[i]))
                        p.Alive = false;
                }

                // 2) Enemy hits (player faction only)
                if (p.Faction == ProjectileFaction.Player)
                {
                    for (int i = 0; i < enemies.Length && p.Alive; i++)
                    {
                        if (ProjectileHitsCollidable(p, enemies[i]))
                        {
                            onEnemyHit?.Invoke(i);
                            p.Alive = false;
                        }
                    }
                }

                // 3) Player hits (enemy faction only)
                if (p.Faction == ProjectileFaction.Enemy && p.Alive)
                {
                    if (ProjectileHitsCollidable(p, player))
                    {
                        onPlayerHit?.Invoke();
                        p.Alive = false;
                    }
                }
            }

            pm.CullDead();
        }
    }
}