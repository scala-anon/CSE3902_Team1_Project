using HollowKnight.Interfaces;
using HollowKnight.Projectiles;
using HollowKnight.Player;
using Microsoft.Xna.Framework;
using System;

namespace HollowKnight.Collision
{
    public static class CollisionResponse
    {
        // Returns true if projectile should be destroyed
        public static bool ProjectileHitsCollidable(Projectile p, ICollidable target)
        {
            if (p == null || target == null)
                return false;

            if (!p.IsActive || !target.IsActive)
                return false;

            return p.Bounds.Intersects(target.GetBounds()[0]);
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

                // Blocks first: projectile dies on impact
                for (int i = 0; i < blocks.Length && p.Alive; i++)
                {
                    if (ProjectileHitsCollidable(p, blocks[i]))
                    {
                        Console.WriteLine("Projective Collided with Object");
                        p.Alive = false;
                    }
                }

                // Enemy hits (player faction only)
                if (p.Faction == ProjectileFaction.Player)
                {
                    for (int i = 0; i < enemies.Length && p.Alive; i++)
                    {
                        if (ProjectileHitsCollidable(p, enemies[i]))
                        {
                            Console.WriteLine("Projective Collided with Enemy");
                            onEnemyHit?.Invoke(i);
                            p.Alive = false;
                        }
                    }
                }

                // Player hits (enemy faction only)
                if (p.Faction == ProjectileFaction.Enemy && p.Alive)
                {
                    if (ProjectileHitsCollidable(p, player))
                    {
                        Console.WriteLine("Projective Collided with Knight");
                        onPlayerHit?.Invoke();
                        p.Alive = false;
                    }
                }
            }

            pm.CullDead();
        }

        public static void ResolvePlayerBlockCollision(TheKnight player, ICollidable block)
        {
            if (player == null || block == null)
                return;

            if (!player.IsActive || !block.IsActive)
                return;

            Rectangle playerBounds = player.GetBounds()[0];
            Rectangle blockBounds = block.GetBounds()[0];

            if (!playerBounds.Intersects(blockBounds))
                return;

            int overlapLeft = playerBounds.Right - blockBounds.Left;
            int overlapRight = blockBounds.Right - playerBounds.Left;
            int overlapTop = playerBounds.Bottom - blockBounds.Top;
            int overlapBottom = blockBounds.Bottom - playerBounds.Top;

            int minOverlapX = Math.Min(overlapLeft, overlapRight);
            int minOverlapY = Math.Min(overlapTop, overlapBottom);

            if (minOverlapX < minOverlapY)
            {
                // Resolve left/right wall collision
                if (overlapLeft < overlapRight)
                {
                    player.position.X -= overlapLeft;
                }
                else
                {
                    player.position.X += overlapRight;
                }

                player.StopMovingHorizontal();
            }
            else
            {
                // Resolve top/bottom collision
                if (overlapTop < overlapBottom)
                {
                    // Player landed on top of block
                    player.position.Y -= overlapTop;
                    player.Land();
                }
                else
                {
                    // Player hit underside of block
                    player.position.Y += overlapBottom;
                    player.StopMovingVertical();
                }
            }
        }
    }
}