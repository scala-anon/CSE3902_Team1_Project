using HollowKnight.Collision;
using HollowKnight.Projectiles;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public class EnemyProjectile
    {
        private readonly MantisLord _enemy;
        private readonly ProjectileSpawner _spawner;

        
        private float _timer = 0f;
        private readonly float _interval = EnemyConstants.EnemyProjectileInterval;
        private readonly float _speed = EnemyConstants.EnemyProjectileSpeed;

        

        public EnemyProjectile(MantisLord enemy, ProjectileSpawner spawner)
        {
            _enemy = enemy;
            _spawner = spawner;
        }

        public void Update(GameTime gameTime)
        {
            //In MantisLordsProjectile.cs
        }

        public void Fire()
        {
            Vector2 direction = _enemy._direction == Direction.Right
                            ? new Vector2(1f, 0f)
                            : new Vector2(-1f, 0f);

                        Vector2 spawn = _enemy.FacingDirection == Direction.Right
                            ? new Vector2(_enemy.Bounds.Right, _enemy.Bounds.Top + _enemy.Bounds.Height / 2f)
                            : new Vector2(_enemy.Bounds.Left, _enemy.Bounds.Top + _enemy.Bounds.Height / 2f);

                        _spawner.SpawnMantisLordProjectile(spawn, direction, EnemyConstants.EnemyProjectileSpeed, ProjectileFaction.Enemy);
        }
    }
}