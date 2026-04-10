using HollowKnight.Enemies;
using HollowKnight.Projectiles;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using System;

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

                // Blocks first: projectile dies on impact
                for (int i = 0; i < blocks.Length && p.Alive; i++)
                {
                    if (ProjectileHitsCollidable(p, blocks[i]))
                    {
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
                        onPlayerHit?.Invoke();
                        p.Alive = false;
                    }
                }
            }

            pm.CullDead();
        }

        private static (int left, int right, int top, int bottom) CalculateOverlaps(Rectangle a, Rectangle b)
        {
            return (
                a.Right  - b.Left,
                b.Right  - a.Left,
                a.Bottom - b.Top,
                b.Bottom - a.Top
            );
        }

        public static void ResolveEnemyBlockCollision(Vengefly vengefly, ICollidable block)
        {
            if (vengefly == null || block == null) return;
            if (!block.IsActive || vengefly.IsGrounded) return;

            Rectangle enemyBounds = vengefly.Bounds;
            Rectangle blockBounds = block.Bounds;

            if (!enemyBounds.Intersects(blockBounds)) return;

            var (overlapLeft, overlapRight, overlapTop, overlapBottom) = CalculateOverlaps(enemyBounds, blockBounds);

            if (overlapLeft <= 0 || overlapRight <= 0 || overlapTop <= 0 || overlapBottom <= 0) return;

            if (Math.Min(overlapLeft, overlapRight) < Math.Min(overlapTop, overlapBottom))
            {
                if (overlapLeft < overlapRight)
                    vengefly.position.X -= overlapLeft;
                else
                    vengefly.position.X += overlapRight;
            }
            else
            {
                if (overlapTop < overlapBottom)
                {
                    vengefly.position.Y -= overlapTop;
                    if (vengefly.Dead)
                        vengefly.Land();
                }
                else
                {
                    vengefly.position.Y += overlapBottom;
                }
            }
        }

        public static void ResolvePlayerBlockCollision(TheKnight player, ICollidable block)
        {
            if (player == null || block == null)
                return;

            if (!player.IsActive || !block.IsActive)
                return;

            Rectangle playerBounds = player.Bounds;
            Rectangle blockBounds = block.Bounds;

            Rectangle probe = new Rectangle(playerBounds.X, playerBounds.Y, playerBounds.Width, playerBounds.Height + GameConstants.GroundProbeExtension);
            if (!probe.Intersects(blockBounds))
                return;

            var (overlapLeft, overlapRight, overlapTop, overlapBottom) = CalculateOverlaps(playerBounds, blockBounds);

            // Skip if no actual overlap on original bounds (only the probe touched)
            // This means the knight is resting on top — just re-confirm grounding
            if (overlapTop <= 0 && overlapBottom > 0 && playerBounds.Right > blockBounds.Left && playerBounds.Left < blockBounds.Right)
            {
                player.position.Y = blockBounds.Top - playerBounds.Height;
                player.Land();
                return;
            }

            // No real overlap at all
            if (overlapLeft <= 0 || overlapRight <= 0 || overlapTop <= 0 || overlapBottom <= 0)
                return;

            int minOverlapX = Math.Min(overlapLeft, overlapRight);
            int minOverlapY = Math.Min(overlapTop, overlapBottom);

            if (minOverlapX < minOverlapY)
            {
                // Resolve left/right wall collision
                if (overlapLeft < overlapRight)
                    player.position.X -= overlapLeft;
                else
                    player.position.X += overlapRight;

                player.StopMovingHorizontal();
            }
            else
            {
                if (overlapTop < overlapBottom)
                {
                    // Player landed on top of block
                    player.position.Y = blockBounds.Top - playerBounds.Height;
                    player.Land();
                }
                else
                {
                    // Player hit underside of block
                    player.position.Y = blockBounds.Bottom;
                    player.StopMovingVertical();
                }
            }
        }
    }
}