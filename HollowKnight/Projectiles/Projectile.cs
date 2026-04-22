using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Shared;

namespace HollowKnight.Projectiles
{
    public enum ProjectileFaction { Player, Enemy }

    public class Projectile : ICollidable
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Width = GameConstants.ProjectileDefaultSize;
        public int Height = GameConstants.ProjectileDefaultSize;

        public int Damage = GameConstants.ProjectileDefaultDamage;
        public bool Alive = true;
        public bool IsActive => Alive;
        public bool HasMoved = false;

        private Rectangle[] hitBoxes = new Rectangle[1];
        public ProjectileFaction Faction;
        public virtual bool PiercesEnemies { get; } = false;

        public virtual Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        public Projectile(Vector2 position, Vector2 velocity, ProjectileFaction faction)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        //using virtual so we can override with different projectile types (pulsing, collision, etc.)
        public virtual void Update(GameTime gameTime)
        {
            // Position is integrated in ProjectileManager.Step()
        }

        public Rectangle[] GetBounds()
        {
            hitBoxes[0] = Bounds;
            return hitBoxes;
        }

        public void Step(Vector2 delta)
        {
            Position += delta;
            if (delta.LengthSquared() > 0)
            {
                HasMoved = true;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Direction facing, float layerDepth = 0f)
        {
            // Base projectile has no visual representation
        }

        public virtual void OnCollide(ICollidable target, CollisionSide side)
        {
            Alive = false;
        }
    }
}
