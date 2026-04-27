using HollowKnight.Audio;
using HollowKnight.Commands;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
    public class KnightProjectile
    {
        private readonly TheKnight _knight;
        private readonly ProjectileSpawner _spawner;

        private float _projectileTimer = 0f;

        public KnightProjectile(TheKnight knight, ProjectileSpawner spawner)
        {
            _knight = knight;
            _spawner = spawner;
        }

        public void Update(GameTime gameTime)
        {
            // Projectiles now fired with the Fire() method
        }

        public void Fire(bool goofy)
        {
            AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Hero_Fireball());
            Vector2 direction = _knight.Facing == Direction.Right
                ? new Vector2(1f, 0f)
                : new Vector2(-1f, 0f);

            Vector2 spawn = _knight.Facing == Direction.Right
                ? new Vector2(_knight.Bounds.Right - KnightConstants.KnightProjectileSpawnOffsetRight, _knight.Bounds.Top + _knight.Bounds.Height / 2f)
                : new Vector2(_knight.Bounds.Left - KnightConstants.KnightProjectileSpawnOffsetLeft, _knight.Bounds.Top + _knight.Bounds.Height / 2f);


            if (goofy == true)
            {
                _spawner.SpawnGoofy(spawn, direction, KnightConstants.KnightProjectileSpeed, ProjectileFaction.Player);
            } else
            {
                _spawner.Spawn(spawn, direction, KnightConstants.KnightProjectileSpeed, ProjectileFaction.Player);
            }
           
            AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Spirit());
        }
    }
}