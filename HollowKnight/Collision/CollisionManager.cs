using HollowKnight.Enemies;
using HollowKnight.Environment;
using HollowKnight.Projectiles;
using HollowKnight.Player;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using System;
using static HollowKnight.Shared.CollisionLayerMatrix;

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
                    if (!ShouldCollide(p, blocks[i])) continue;
                    CollisionSide side = CollisionDetector.Detect(p, blocks[i]);
                    if (side != CollisionSide.None)
                    {
                        p.OnCollide(blocks[i], side);
                    }
                }

                // Enemy hits (player projectiles only)
                for (int i = 0; i < enemies.Length && p.Alive; i++)
                {
                    if (!ShouldCollide(p, enemies[i])) continue;
                    CollisionSide side = CollisionDetector.Detect(p, enemies[i]);
                    if (side != CollisionSide.None)
                    {
                        onEnemyHit?.Invoke(i);
                        if (!p.PiercesEnemies)
                        {
                            p.OnCollide(enemies[i], side);
                        }
                    }
                }

                // Player hits (enemy projectiles only)
                if (p.Alive && ShouldCollide(p, player))
                {
                    CollisionSide side = CollisionDetector.Detect(p, player);
                    if (side != CollisionSide.None)
                    {
                        onPlayerHit?.Invoke();
                        p.OnCollide(player, side);
                    }
                }
            }

            pm.CullDead();
        }

        private static (int left, int right, int top, int bottom) CalculateOverlaps(Rectangle a, Rectangle b)
        {
            return (
                a.Right - b.Left,
                b.Right - a.Left,
                a.Bottom - b.Top,
                b.Bottom - a.Top
            );
        }

        public static void ResolveCrawlidBlockCollision(Crawlid crawlid, ICollidable block)
        {
            if (crawlid == null || block == null) return;
            if (!block.IsActive) return;
            if (block is Spike) return;

            Rectangle enemyBounds = crawlid.Bounds;
            Rectangle blockBounds = block.Bounds;

            // Ground probe extends slightly below the sprite so a crawlid sitting exactly
            // on a platform surface (no actual overlap) still re-confirms grounding each frame.
            Rectangle probe = new Rectangle(enemyBounds.X, enemyBounds.Y, enemyBounds.Width, enemyBounds.Height + GameConstants.GroundProbeExtension);
            if (!probe.Intersects(blockBounds)) return;

            var (overlapLeft, overlapRight, overlapTop, overlapBottom) = CalculateOverlaps(enemyBounds, blockBounds);

            // Probe touched but bounds don't actually overlap: crawlid is resting on top.
            if (overlapTop <= 0 && overlapBottom > 0 && enemyBounds.Right > blockBounds.Left && enemyBounds.Left < blockBounds.Right)
            {
                int sink = crawlid.Alive ? 0 : EnemyConstants.EnemyDeadGroundSink;
                crawlid.position.Y = blockBounds.Top - enemyBounds.Height + sink;
                crawlid.Land();
                return;
            }

            if (overlapLeft <= 0 || overlapRight <= 0 || overlapTop <= 0 || overlapBottom <= 0) return;

            if (Math.Min(overlapLeft, overlapRight) < Math.Min(overlapTop, overlapBottom))
            {
                if (overlapLeft < overlapRight)
                    crawlid.position.X -= overlapLeft;
                else
                    crawlid.position.X += overlapRight;
                crawlid.OnWallHit();
            }
            else
            {
                if (overlapTop < overlapBottom)
                {
                    int sink = crawlid.Alive ? 0 : EnemyConstants.EnemyDeadGroundSink;
                    crawlid.position.Y = blockBounds.Top - enemyBounds.Height + sink;
                    crawlid.Land();
                }
                else
                {
                    crawlid.position.Y += overlapBottom;
                }
            }
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
                    int sink = vengefly.Dead ? EnemyConstants.EnemyDeadGroundSink : 0;
                    vengefly.position.Y -= overlapTop - sink;
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