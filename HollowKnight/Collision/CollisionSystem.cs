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

namespace HollowKnight.Collision
{
    public class CollisionSystem
    {
        private readonly CollisionHandler _handler = new();
        private TheKnight _currentKnight;

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

                // Hitting spikes damages the knight
                _handler.Register<Spike, TheKnight>(side, (a, b) => ((TheKnight)b).TakeDamage(side));

                // Spikes instantly kill vengefly
                _handler.Register<Spike, Vengefly>(side, (a, b) => ((Vengefly)b).Kill());

                // Sword damages enemies
                _handler.Register<SwordHitbox, Crawlid>(side, (a, b) => { if (((Crawlid)b).TakeDamage(side) && _currentKnight != null) _currentKnight.GainSoul(); });
                _handler.Register<SwordHitbox, Vengefly>(side, (a, b) => { if (((Vengefly)b).TakeDamage(side) && _currentKnight != null) _currentKnight.GainSoul(); });

                // Item pickup
                _handler.Register<Spirit, TheKnight>(side, (a, b) => ((TheKnight)b).Collect(side));
            }
        }

        public void Update(
            TheKnight knight,
            List<IObject> platforms,
            List<IInteractable> interactables,
            List<IEnemy> enemies,
            List<Spirit> items,
            ProjectileManager projectileManager,
            NavigationGrid navigationGrid)
        {
            _currentKnight = knight;
            Vector2 knightPosition = knight.GetBounds()[0].Center.ToVector2();

            // Platform collisions
            foreach (IObject obj in platforms)
            {
                if (obj == null) continue;
                if (!CollisionLayerMatrix.ShouldCollide(obj, knight)) continue;
                CollisionSide side = CollisionDetector.Detect(obj, knight);
                if (side != CollisionSide.None)
                    DebugLogger.LogCollision($"{obj.GetType().Name} vs Knight side={side}");
                _handler.HandleCollision(obj, knight, side);
            }

            // Enemy collisions
            foreach (IEnemy enemy in enemies)
            {
                enemy.SetKnightPosition(knightPosition);
                enemy.SetNavigationGrid(navigationGrid);
                if (!enemy.IsActive) continue;
                if (!CollisionLayerMatrix.ShouldCollide(enemy, knight)) continue;
                CollisionSide side = CollisionDetector.Detect(enemy, knight);
                if (side != CollisionSide.None)
                    DebugLogger.LogCollision($"{enemy.GetType().Name} vs Knight side={side}");
                _handler.HandleCollision(enemy, knight, side);
            }

            // Spike collisions with enemies
            foreach (IEnemy enemy in enemies)
            {
                if (!enemy.IsActive) continue;
                foreach (IObject obj in platforms)
                {
                    if (!(obj is Spike spike)) continue;
                    if (!CollisionLayerMatrix.ShouldCollide(spike, enemy)) continue;
                    CollisionSide side = CollisionDetector.Detect(spike, enemy);
                    _handler.HandleCollision(spike, enemy, side);
                }
            }

            // Sword collisions / interactions
            SwordHitbox swordHitbox = knight.GetSwordHitbox();
            if (swordHitbox != null)
            {
                foreach (IEnemy enemy in enemies)
                {
                    if (!enemy.IsActive) continue;
                    if (!CollisionLayerMatrix.ShouldCollide(swordHitbox, enemy)) continue;
                    CollisionSide side = CollisionDetector.Detect(swordHitbox, enemy);
                    if (side != CollisionSide.None)
                        DebugLogger.LogCollision($"SwordHitbox vs {enemy.GetType().Name} side={side}");
                    _handler.HandleCollision(swordHitbox, enemy, side);
                }

                // Sword vs BreakableTerrain: process IBreakable objects here via the
                // PlayerAttack<->BreakableTerrain matrix pairing so they are handled in
                // a single canonical place (prevents double-hit if they also appear in interactables).
                foreach (IObject obj in platforms)
                {
                    if (obj == null || !obj.IsActive) continue;
                    if (obj is not IBreakable) continue;
                    if (!(obj is IInteractable breakableInteractable)) continue;
                    if (!CollisionLayerMatrix.ShouldCollide(swordHitbox, obj)) continue;
                    CollisionSide side = CollisionDetector.Detect(swordHitbox, obj);
                    if (side != CollisionSide.None && breakableInteractable.IsInteractable(_currentKnight))
                    {
                        DebugLogger.LogInteraction(obj.GetType().Name, "SwordHit-Breakable", "platforms");
                        breakableInteractable.OnInteract(_currentKnight);
                    }
                }

                foreach (IInteractable interactable in interactables)
                {
                    if (interactable == null ||
                        !interactable.IsActive ||
                        interactable.InteractionType != InteractionType.SwordHit)
                    {
                        continue;
                    }

                    // IBreakable objects are processed via the PlayerAttack<->BreakableTerrain
                    // layer pairing in the platforms loop above. Skipping here prevents a single
                    // sword swing from decrementing _hitCount twice.
                    if (interactable is IBreakable) continue;

                    if (!CollisionLayerMatrix.ShouldCollide(swordHitbox, interactable)) continue;
                    CollisionSide side = CollisionDetector.Detect(swordHitbox, interactable);
                    if (side != CollisionSide.None && interactable.IsInteractable(_currentKnight))
                    {
                        DebugLogger.LogInteraction(interactable.GetType().Name, "SwordHit", "Z");
                        interactable.OnInteract(_currentKnight);
                    }
                }
            }

            // Block resolution pass
            knight.SetAirborne();
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i] is ICollidable blockObj)
                {
                    if (!CollisionLayerMatrix.ShouldCollide(blockObj, knight)) continue;
                    CollisionManager.ResolvePlayerBlockCollision(knight, blockObj);
                }
            }

            // Block resolution for vengefly enemies (includes dead vengeflies falling)
            foreach (IEnemy enemy in enemies)
            {
                if (!(enemy is Vengefly vengefly)) continue;
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i] is ICollidable blockObj)
                    {
                        if (!CollisionLayerMatrix.ShouldCollide(blockObj, vengefly)) continue;
                        CollisionManager.ResolveEnemyBlockCollision(vengefly, blockObj);
                    }
                }
            }
            
            // Projectile collisions
            ICollidable player = knight;
            ICollidable[] enemyCollidables = CollisionGroupBuilder.GetCollidables(enemies.ToArray());
            ICollidable[] blockCollidables = CollisionGroupBuilder.GetCollidables(platforms.ToArray());

            CollisionManager.ResolveProjectileCollisions(
                projectileManager,
                player,
                enemyCollidables,
                blockCollidables,
                onPlayerHit: () => knight.TakeDamage(),
                onEnemyHit: (enemyIndex) => { enemies[enemyIndex].TakeDamage(); }
            );

            // Item collisions
            foreach (Spirit item in items)
            {
                if (!CollisionLayerMatrix.ShouldCollide(item, knight)) continue;
                CollisionSide side = CollisionDetector.Detect(item, knight);
                _handler.HandleCollision(item, knight, side);
                if (side != CollisionSide.None)
                    item.IsActive = false;
            }
        }
    }
}
