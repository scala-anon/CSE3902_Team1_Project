using Microsoft.Xna.Framework;
using HollowKnight.Collision;

namespace HollowKnight.Projectiles
{
    public enum ProjectileFaction { Player, Enemy }

    //TODO change to ICollidable
    public class Projectile : ICollidable
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int Width = 12;
        public int Height = 12;

        public int Damage = 1;
        public bool Alive = true;
        public bool IsActive => Alive;

        public Rectangle[] hitBoxes = new Rectangle[1];
        public ProjectileFaction Faction;


        //TODO choose one
        /* public Rectangle[] GetBounds()
         {
             Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
             hitBoxes[0] = rectangle;
             return hitBoxes;
         }
     */

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

        public Rectangle[] GetBounds()
        {
            hitBoxes[0] = Bounds;
            return hitBoxes;
        }

        public void Step(Vector2 delta)
        {
            Position += delta;
        }
    }
}