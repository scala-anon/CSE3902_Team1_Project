using HollowKnight.Collision;
using HollowKnight.Projectiles;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public class EnemyProjectile
    {
        private readonly ICollidable _enemy;
        private readonly ProjectileSpawner _spawner;

        private float _timer = 0f;
        private readonly float _interval = EnemyConstants.EnemyProjectileInterval;
        private readonly float _speed = EnemyConstants.EnemyProjectileSpeed;

        public EnemyProjectile(ICollidable enemy, ProjectileSpawner spawner)
        {
            _enemy = enemy;
            _spawner = spawner;
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _interval)
            {
                _timer = 0f;

                Rectangle bounds = _enemy.Bounds;
                Vector2 spawn = bounds.Center.ToVector2();

                _spawner.Spawn(spawn, direction, _speed, ProjectileFaction.Enemy);
            }
        }
    }
}