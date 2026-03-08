using HollowKnight.Projectiles;
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
            _projectileManager.Spawn(
                new Projectile(spawnPosition, direction * speed, faction)
            );
        }
    }
}