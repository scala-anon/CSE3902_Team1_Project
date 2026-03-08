using Microsoft.Xna.Framework;
using HollowKnight.Collision;

namespace HollowKnight.Projectiles
{
    public enum ProjectileFaction { Player, Enemy }

    public class Projectile : ICollidable
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Width = 12;
        public int Height = 12;

        public int Damage = 1;
        public bool Alive = true;

        public ProjectileFaction Faction;

        public bool IsActive => Alive;

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        public Projectile(Vector2 position, Vector2 velocity, ProjectileFaction faction)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
        }

        public Rectangle GetBounds()
        {
            return Bounds;
        }

        public void Step(Vector2 delta)
        {
            Position += delta;
        }
    }
}