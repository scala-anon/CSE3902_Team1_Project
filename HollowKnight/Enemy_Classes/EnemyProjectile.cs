using HollowKnight.Collision;
using HollowKnight.Projectiles;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemy_Classes
{
    public class EnemyProjectile
    {
        private readonly ICollidable _enemy;
        private readonly ProjectileSpawner _spawner;

        private float _timer = 0f;
        private readonly float _interval = 3f;
        private readonly float _speed = 250f;

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

                Vector2 spawn = new Vector2(
                    _enemy.Bounds.Center.X,
                    _enemy.Bounds.Center.Y
                );

                _spawner.Spawn(spawn, direction, _speed, ProjectileFaction.Enemy);
            }
        }
    }
}