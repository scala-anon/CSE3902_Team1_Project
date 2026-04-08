using Microsoft.Xna.Framework;
using HollowKnight.Collision;

namespace HollowKnight.Interfaces
{
    public interface IHazard : ICollidable
    {

        int Damage { get; }
        Vector2 Knockback { get; }
    }
}
