using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HollowKnight.Enemies;
using HollowKnight.Environment;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Abilities;
using HollowKnight.Pathfinding;
using HollowKnight.Shared;
using IHCCollidable = HollowKnight.Collision.ICollidable;

namespace HollowKnight.Collision
{
    public class CollisionSystem
    {
        private readonly CollisionHandler _handler = new();

        public CollisionSystem()
        {
            RegisterAll();
        }

        private void RegisterAll()
        {
            foreach (CollisionSide side in new[] { CollisionSide.Left, CollisionSide.Right, CollisionSide.Top, CollisionSide.Bottom })
            {
                // Enemy contact damages knight
                _handler.Register<Crawlid, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));
                _handler.Register<Vengefly, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));

                // Sword damages enemies
                _handler.Register<SwordHitbox, Crawlid>(side, (a, b) => ((Crawlid)b).TakeDamage(side));
                _handler.Register<SwordHitbox, Vengefly>(side, (a, b) => ((Vengefly)b).TakeDamage(side));

                // Item pickup
                _handler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            }
        }

        public void Update(
            TheKnight knight,
            List<IObject> platforms,
            List<IEnemy> enemies,
            List<Spirit> items,
            ProjectileManager projectileManager,
            NavigationGrid navigationGrid)
        {
            int screenHeight = GameConstants.ScreenHeight;

            // Level boundary clamping
            Rectangle[] knightBounds = knight.GetBounds();
            Rectangle kb = knightBounds[0];
            if (kb.Bottom >= screenHeight)
            {
                knight.position.Y = screenHeight - kb.Height;
                knight.Land();
            }
            if (knight.position.X < 0) knight.position.X = 0;
            if (kb.Right > GameConstants.DefaultLevelWidth)
                knight.position.X = GameConstants.DefaultLevelWidth - kb.Width;

            Vector2 knightPosition = knight.GetBounds()[0].Center.ToVector2();

            // Platform collisions
            foreach (IObject obj in platforms)
            {
                if (obj == null) continue;
                CollisionSide side = CollisionDetector.Detect(obj, knight);
                _handler.HandleCollision(obj, knight, side);
            }

            // Enemy collisions
            foreach (IEnemy enemy in enemies)
            {
                enemy.SetKnightPosition(knightPosition);
                enemy.SetNavigationGrid(navigationGrid);
                if (!enemy.IsActive) continue;
                CollisionSide side = CollisionDetector.Detect(enemy, knight);
                _handler.HandleCollision(enemy, knight, side);
            }

            // Sword collisions
            SwordHitbox swordHitbox = knight.GetSwordHitbox();
            if (swordHitbox != null)
            {
                foreach (IEnemy enemy in enemies)
                {
                    if (!enemy.IsActive) continue;
                    CollisionSide side = CollisionDetector.Detect(swordHitbox, enemy);
                    _handler.HandleCollision(swordHitbox, enemy, side);
                }
            }

            // Block resolution pass
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i] is IHCCollidable blockObj)
                    CollisionManager.ResolvePlayerBlockCollision(knight, blockObj);
            }

            // Projectile collisions
            IHCCollidable player = knight;
            IHCCollidable[] enemyCollidables = CollisionGroupBuilder.GetCollidables(enemies.ToArray());
            IHCCollidable[] blockCollidables = CollisionGroupBuilder.GetCollidables(platforms.ToArray());

            CollisionManager.ResolveProjectileCollisions(
                projectileManager,
                player,
                enemyCollidables,
                blockCollidables,
                onPlayerHit: () => knight.TakeDamage(),
                onEnemyHit: (enemyIndex) => { }
            );

            // Item collisions
            foreach (Spirit item in items)
            {
                CollisionSide side = CollisionDetector.Detect(item, knight);
                _handler.HandleCollision(item, knight, side);
                if (side != CollisionSide.None)
                    item.IsActive = false;
            }
        }
    }
}
