using HollowKnight.Projectiles;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Projectiles
{
    public class ProjectileSpawner
    {
        private readonly ProjectileManager _projectileManager;

        public ProjectileSpawner(ProjectileManager projectileManager)
        {
            _projectileManager = projectileManager;
        }

        public void Spawn(Vector2 spawnPosition, Vector2 direction, float speed, ProjectileFaction faction)
        {
            DebugLogger.LogObject($"VengefulSpiritProjectile spawned at ({spawnPosition.X:F0},{spawnPosition.Y:F0}) faction={faction}");
            _projectileManager.Spawn(
                new VengefulSpiritProjectile(spawnPosition, direction * speed, faction)
            );
        }

        public void SpawnMantisLordProjectile(Vector2 spawnPosition, Vector2 direction, float speed, ProjectileFaction faction)
        {
            DebugLogger.LogObject($"MantisLordsProjectile spawned at ({spawnPosition.X:F0},{spawnPosition.Y:F0}) faction = {faction}");
            _projectileManager.Spawn(new MantisLordsProjectile(spawnPosition, direction * speed, faction));
        }

        
    }
}