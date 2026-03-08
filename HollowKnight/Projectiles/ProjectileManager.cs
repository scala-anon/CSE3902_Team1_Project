using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HollowKnight.Projectiles
{
    public class ProjectileManager
    {
        private readonly List<Projectile> _projectiles = new();

        public IReadOnlyList<Projectile> All => _projectiles;

        public void Spawn(Projectile p) => _projectiles.Add(p);

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                var p = _projectiles[i];
                if (!p.Alive)
                {
                    _projectiles.RemoveAt(i);
                    continue;
                }

                // anti-tunneling: move in small steps
                Vector2 totalMove = p.Velocity * dt;
                const float maxStep = 5f;
                int steps = (int)Math.Ceiling(totalMove.Length() / maxStep);
                if (steps < 1) steps = 1;

                Vector2 stepMove = totalMove / steps;
                for (int s = 0; s < steps && p.Alive; s++)
                    p.Step(stepMove);
            }
        }

        public void CullDead()
        {
            _projectiles.RemoveAll(p => !p.Alive);
        }
    }
}