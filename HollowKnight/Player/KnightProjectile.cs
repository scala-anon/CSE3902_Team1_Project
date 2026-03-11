using HollowKnight.Player;
using HollowKnight.Projectiles;
using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
    public class KnightProjectile
    {
        private readonly TheKnight _knight;
        private readonly ProjectileSpawner _spawner;

        private float _projectileTimer = 0f;
        private readonly float _projectileInterval = 2f;
        private readonly float _projectileSpeed = 400f;

        public KnightProjectile(TheKnight knight, ProjectileSpawner spawner)
        {
            _knight = knight;
            _spawner = spawner;
        }

        public void Update(GameTime gameTime)
        {
            _projectileTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_projectileTimer >= _projectileInterval)
            {
                _projectileTimer = 0f;
                Fire();
            }
        }

        public void Fire()
        {
            Vector2 direction = _knight.Facing == Direction.Right
                ? new Vector2(1f, 0f)
                : new Vector2(-1f, 0f);

            Vector2 spawn = _knight.Facing == Direction.Right
                ? new Vector2(_knight.Bounds.Right, _knight.Bounds.Top + _knight.Bounds.Height / 2f)
                : new Vector2(_knight.Bounds.Left - 12f, _knight.Bounds.Top + _knight.Bounds.Height / 2f);

            _spawner.Spawn(spawn, direction, _projectileSpeed, ProjectileFaction.Player);
        }
    }
}